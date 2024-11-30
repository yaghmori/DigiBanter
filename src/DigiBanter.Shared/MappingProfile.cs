using AutoMapper;
using DigiBanter.Shared.Extensions;
using DigiBanter.Shared.PagedCollections;
using DigiBanter.Shared.Services.TimeZoneResolver;
using System.Diagnostics;

namespace DigiBanter.Shared;
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





        //================================= [Activities] =================================


    }
}
