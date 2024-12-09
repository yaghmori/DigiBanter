using AutoMapper;
using AutoMapper.Configuration.Conventions;
using Microsoft.AspNetCore.Http;
using ParsLinks.Domain.Entities;
using ParsLinks.Shared.Dto.Response;
using ParsLinks.Shared.Extensions;
using ParsLinks.Shared.PagedCollections;
using ParsLinks.Shared.Services.TimeZoneResolver;
using System.Diagnostics;

namespace ParsLinks.Shared;
public class MappingProfile : AutoMapper.Profile
{

    private readonly IJsonSerializer _jsonService;
    public MappingProfile(ITimeZoneProvider timeZoneProvider, IJsonSerializer jsonService)
    {
        SourceMemberNamingConvention = new LowerUnderscoreNamingConvention();
        DestinationMemberNamingConvention = new PascalCaseNamingConvention();

        _jsonService = jsonService;
        CreateMap(typeof(PagedList<>), typeof(IPagedList<>)).ReverseMap();
        CreateMap<DateTimeOffset, DateTime>().ConvertUsing(s => s.ToTimeZoneDateTime(timeZoneProvider.CurrentTimeZone));
        CreateMap<DateTimeOffset?, DateTime?>().ConvertUsing(s => s.ToTimeZoneDateTime(timeZoneProvider.CurrentTimeZone));
        CreateMap<DateTimeOffset, DateTime?>().ConvertUsing(s => s.ToTimeZoneDateTime(timeZoneProvider.CurrentTimeZone));

        CreateMap<DateTime?, DateTimeOffset?>().ConvertUsing(s => s.ToUtcDateTimeOffset(timeZoneProvider.CurrentTimeZone));
        CreateMap<DateTime, DateTimeOffset>().ConvertUsing(s => s.ToUtcDateTimeOffset(timeZoneProvider.CurrentTimeZone));





        //================================= [Blog] =================================
        CreateMap<PostTranslation, BlogPostResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Post.Id))
            .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Post.Author.DisplayName))
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Post.Image))
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
            .ForMember(dest => dest.PublishedAt, opt => opt.MapFrom(src => src.Post.PublishedAt));
    }



}


