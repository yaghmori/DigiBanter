
using DigiBanter.Shared.Constatns;
using DigiBanter.Shared.Dto.Request;
using DigiBanter.Shared.Dto.Response;
using DigiBanter.Shared.ResultWrapper;
using Microsoft.AspNetCore.Mvc;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this IEndpointRouteBuilder routes)
    {
        var auth = routes.MapGroup(AppEndPoints.Auth.Base);


        auth.MapPost("/login", Login)
            .AllowAnonymous();

        auth.MapPost("/refresh", RefreshToken)
            .AllowAnonymous();

        auth.MapPost("/register", Register)
            .AllowAnonymous();

        auth.MapPost("/logout", Logout)
            .RequireAuthorization();


        auth.MapGet("/principal", GetClaimsPrincipals)
            .RequireAuthorization();

        auth.MapPost("/reset-password", ResetPassword)
            .AllowAnonymous();

        auth.MapPost("/reset-password/request", RequestPasswordReset)
            .AllowAnonymous();
    }

    private static async Task<IResult> RequestPasswordReset(
        [FromBody] string email,
        IAuthService authService,
        CancellationToken cancellationToken)
    {
        var result = await authService.RequestPasswordResetAsync(email, cancellationToken);

        if (!result.IsSuccess)
        {
            return Results.StatusCode(result.StatusCode);
        }

        return Results.Ok(ApiResult.Success("Verification token sent."));
    }







    private static async Task<IResult> Login(
        [FromBody] LoginByEmailRequest request,
        IAuthService authService,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var result = await authService.LoginAsync(request, context, cancellationToken);

        if (!result.IsSuccess)
            return Results.BadRequest(result.ErrorMessage);

        return TypedResults.Ok(ApiResult<TokenResponse>.Success(result.Data));
    }

    private static async Task<IResult> RefreshToken(
        [FromQuery] string refreshToken,
        IAuthService authService,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var result = await authService.RefreshTokenAsync(refreshToken, context, cancellationToken);

        if (!result.IsSuccess)
            return Results.BadRequest(result.ErrorMessage);

        return Results.Ok(ApiResult<TokenResponse>.Success(result.Data));
    }

    private static async Task<IResult> Register(
        [FromBody] RegisterRequest request,
        IAuthService authService,
        HttpContext context,
        CancellationToken cancellationToken)
    {
        var result = await authService.RegisterAsync(request, context, cancellationToken);

        if (!result.IsSuccess)
            return Results.BadRequest(result.ErrorMessage);

        return Results.Ok(ApiResult<TokenResponse>.Success(result.Data));
    }

    private static async Task<IResult> Logout(
        [FromHeader] Guid sessionId,
        IAuthService authService,
        CancellationToken cancellationToken)
    {
        var result = await authService.LogoutAsync(sessionId, cancellationToken);

        if (!result.IsSuccess)
            return Results.BadRequest(result.ErrorMessage);

        return Results.Ok(ApiResult.Success("Logged out successfully."));
    }



    private static async Task<IResult> GetClaimsPrincipals(
      IAuthService authService,
      HttpContext context,
      CancellationToken cancellationToken)
    {
        var result = await authService.GetClaimsPrincipalsAsync(context, cancellationToken);

        if (!result.IsSuccess)
        {
            return Results.StatusCode(result.StatusCode);
        }

        return Results.Ok(ApiResult<List<ClaimResponse>>.Success(result.Data));
    }







    private static async Task<IResult> ResetPassword(
    [FromBody] ResetPasswordRequest request,
    IAuthService authService,
    CancellationToken cancellationToken)
    {
        var result = await authService.ResetPasswordAsync( request, cancellationToken);

        if (!result.IsSuccess)
        {
            return Results.StatusCode(result.StatusCode); // Handle error responses (e.g., 400 or 404)
        }

        return Results.Ok(ApiResult.Success("Password successfully reset."));
    }


}
