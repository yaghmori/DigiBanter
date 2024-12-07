using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using ParsLinks.Domain.Entities;
using ParsLinks.Shared.Constatns;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ParsLinks.Shared.Extensions;
namespace ParsLinks.Api.DbContextInterceptors;

public class SaveChangesInterceptor : ISaveChangesInterceptor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SaveChangesInterceptor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public ValueTask<int> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        var userId = _httpContextAccessor.HttpContext?.User.GetUserId();
        string? ipAddress = _httpContextAccessor.HttpContext?.Request?.Headers[AppConstants.IpAddressHeader].FirstOrDefault();
        var context = eventData.Context;

        if(context == null)
            return new ValueTask<int>(result.Result);

        context.ChangeTracker.DetectChanges();
        var entities = context.ChangeTracker.Entries<IAuditEntity>();

        var entityEntries = entities.ToList();
        foreach (var item in entityEntries.Where(s => s.State == EntityState.Added))
        {
            item.Entity.CreatedDate = DateTime.UtcNow;
            item.Entity.CreatedIpAddress = ipAddress;
            item.Entity.CreatedUserId = userId;
        }

        foreach (var item in entityEntries.Where(s => s.State == EntityState.Modified))
        {
            item.Entity.ModifiedDate = DateTime.UtcNow;
            item.Entity.ModifiedIpAddress = ipAddress;
            item.Entity.ModifiedUserId = userId;
        }

        return new ValueTask<int>(result.Result);
    }

    public int SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        return result.Result;
    }
}