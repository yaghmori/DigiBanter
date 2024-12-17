using AutoMapper;
using ParsLinks.Domain.Entities;
using ParsLinks.Shared.Dto.Request;
using ParsLinks.Shared.Dto.Response;
using ParsLinks.Shared.Extensions;
using ParsLinks.Shared.PagedCollections;
using ParsLinks.Shared.Services.TimeZoneResolver;

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
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PostId))
            .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.Post.Author.DisplayName))
            .ForMember(dest => dest.Image, opt => opt.MapFrom(src => src.Post.Image))
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
            .ForMember(dest => dest.PublishedAt, opt => opt.MapFrom(src => src.Post.PublishedAt))
            .ForMember(dest => dest.EstimatedReadingTime, opt => opt.MapFrom(src => CalculateReadingTime(src.Content)));


        CreateMap<Post, BlogPostRequest>().ReverseMap();
        CreateMap<PostTranslation, BlogPostTranslationRequest>().ReverseMap();
        CreateMap<LanguageResponse, Language>().ReverseMap();
        CreateMap<Category, CategoryResponse>().ReverseMap();
        CreateMap<CategoryTranslation, CategoryResponse>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CategoryId))
            .ReverseMap();
    }


    static int CalculateReadingTime(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            return 0;

        int wordCount = content.Split(new char[] { ' ', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries).Length;
        return (int)Math.Ceiling(wordCount / 200.0); // Assuming 200 words per minute
    }

}


