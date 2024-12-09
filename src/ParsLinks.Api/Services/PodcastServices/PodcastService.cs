﻿

using AutoMapper;
using ParsLinks.Api.Services;
using ParsLinks.DataAccess.DbContext;
using ParsLinks.Domain.Entities;
using ParsLinks.Shared.Dto.Response;
using ParsLinks.Shared.Models;
using ParsLinks.Shared.ResultWrapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Collections.Generic;

public class PodcastService : IPodcastService
{
    private readonly List<PodcastResponse> _podcastList;

    public PodcastService()
    {
        _podcastList = [  new PodcastResponse(Guid.NewGuid(), DateTime.Now,"/assets/images/podcast/1.jpg" , "Perhaps, but perhaps your civilization is merely the sewer of an even greater society above you! I feel like I was mauled by Jesus. Stop! Don’t shoot fire stick in space canoe! Cause explosive decompression!\r\n\r\nWell, let’s just dump it in the sewer and say we delivered it. Fry, you can’t just sit here in the dark listening to classical music. The alien mothership is in orbit here. If we can hit that bullseye, the rest of the dominoes will fall like a house of cards. Checkmate.\r\n\r\nFatal. Daddy Bender, we’re hungry. And so we say goodbye to our beloved pet, Nibbler, who’s gone to a place where I, too, hope one day to go. The toilet. Well, thanks to the Internet, I’m now bored with sex. Is there a place on the web that panders to my lust for violence?\r\n\r\n", "Episode 06: Rise of Nomad Destinations"),
    new PodcastResponse(Guid.NewGuid(), DateTime.Now, "/assets/images/podcast/2.jpg", "Perhaps, but perhaps your civilization is merely the sewer of an even greater society above you! I feel like I was mauled by Jesus. Stop! Don’t shoot fire stick in space canoe! Cause explosive decompression!\r\n\r\nWell, let’s just dump it in the sewer and say we delivered it. Fry, you can’t just sit here in the dark listening to classical music. The alien mothership is in orbit here. If we can hit that bullseye, the rest of the dominoes will fall like a house of cards. Checkmate.\r\n\r\nFatal. Daddy Bender, we’re hungry. And so we say goodbye to our beloved pet, Nibbler, who’s gone to a place where I, too, hope one day to go. The toilet. Well, thanks to the Internet, I’m now bored with sex. Is there a place on the web that panders to my lust for violence?\r\n\r\n", "Episode 07: Goodbye boring, hello adventure"),
    new PodcastResponse(Guid.NewGuid(), DateTime.Now, "/assets/images/podcast/3.jpg", "Perhaps, but perhaps your civilization is merely the sewer of an even greater society above you! I feel like I was mauled by Jesus. Stop! Don’t shoot fire stick in space canoe! Cause explosive decompression!\r\n\r\nWell, let’s just dump it in the sewer and say we delivered it. Fry, you can’t just sit here in the dark listening to classical music. The alien mothership is in orbit here. If we can hit that bullseye, the rest of the dominoes will fall like a house of cards. Checkmate.\r\n\r\nFatal. Daddy Bender, we’re hungry. And so we say goodbye to our beloved pet, Nibbler, who’s gone to a place where I, too, hope one day to go. The toilet. Well, thanks to the Internet, I’m now bored with sex. Is there a place on the web that panders to my lust for violence?\r\n\r\n", "Episode 04: The Digital Nomad Lifestyle Facts"),
    new PodcastResponse(Guid.NewGuid(), DateTime.Now, "/assets/images/podcast/4.jpg", "Perhaps, but perhaps your civilization is merely the sewer of an even greater society above you! I feel like I was mauled by Jesus. Stop! Don’t shoot fire stick in space canoe! Cause explosive decompression!\r\n\r\nWell, let’s just dump it in the sewer and say we delivered it. Fry, you can’t just sit here in the dark listening to classical music. The alien mothership is in orbit here. If we can hit that bullseye, the rest of the dominoes will fall like a house of cards. Checkmate.\r\n\r\nFatal. Daddy Bender, we’re hungry. And so we say goodbye to our beloved pet, Nibbler, who’s gone to a place where I, too, hope one day to go. The toilet. Well, thanks to the Internet, I’m now bored with sex. Is there a place on the web that panders to my lust for violence?\r\n\r\n", "Episode 03: Dating As a Digital Nomad"),
    new PodcastResponse(Guid.NewGuid(), DateTime.Now, "/assets/images/podcast/5.jpg", "Perhaps, but perhaps your civilization is merely the sewer of an even greater society above you! I feel like I was mauled by Jesus. Stop! Don’t shoot fire stick in space canoe! Cause explosive decompression!\r\n\r\nWell, let’s just dump it in the sewer and say we delivered it. Fry, you can’t just sit here in the dark listening to classical music. The alien mothership is in orbit here. If we can hit that bullseye, the rest of the dominoes will fall like a house of cards. Checkmate.\r\n\r\nFatal. Daddy Bender, we’re hungry. And so we say goodbye to our beloved pet, Nibbler, who’s gone to a place where I, too, hope one day to go. The toilet. Well, thanks to the Internet, I’m now bored with sex. Is there a place on the web that panders to my lust for violence?\r\n\r\n", "Episode 02: Getting Rid Of Time Consuming Habits"),
    new PodcastResponse(Guid.NewGuid(), DateTime.Now, "/assets/images/podcast/26.jpg", "Perhaps, but perhaps your civilization is merely the sewer of an even greater society above you! I feel like I was mauled by Jesus. Stop! Don’t shoot fire stick in space canoe! Cause explosive decompression!\r\n\r\nWell, let’s just dump it in the sewer and say we delivered it. Fry, you can’t just sit here in the dark listening to classical music. The alien mothership is in orbit here. If we can hit that bullseye, the rest of the dominoes will fall like a house of cards. Checkmate.\r\n\r\nFatal. Daddy Bender, we’re hungry. And so we say goodbye to our beloved pet, Nibbler, who’s gone to a place where I, too, hope one day to go. The toilet. Well, thanks to the Internet, I’m now bored with sex. Is there a place on the web that panders to my lust for violence?\r\n\r\n", "Episode 06: Rise of Nomad Destinations"),
    new PodcastResponse(Guid.NewGuid(), DateTime.Now, "/assets/images/podcast/7.jpg", "Perhaps, but perhaps your civilization is merely the sewer of an even greater society above you! I feel like I was mauled by Jesus. Stop! Don’t shoot fire stick in space canoe! Cause explosive decompression!\r\n\r\nWell, let’s just dump it in the sewer and say we delivered it. Fry, you can’t just sit here in the dark listening to classical music. The alien mothership is in orbit here. If we can hit that bullseye, the rest of the dominoes will fall like a house of cards. Checkmate.\r\n\r\nFatal. Daddy Bender, we’re hungry. And so we say goodbye to our beloved pet, Nibbler, who’s gone to a place where I, too, hope one day to go. The toilet. Well, thanks to the Internet, I’m now bored with sex. Is there a place on the web that panders to my lust for violence?\r\n\r\n", "Episode 02: Getting Rid Of Time Consuming Habits"),
    new PodcastResponse(Guid.NewGuid(), DateTime.Now, "/assets/images/podcast/8.jpg", "Perhaps, but perhaps your civilization is merely the sewer of an even greater society above you! I feel like I was mauled by Jesus. Stop! Don’t shoot fire stick in space canoe! Cause explosive decompression!\r\n\r\nWell, let’s just dump it in the sewer and say we delivered it. Fry, you can’t just sit here in the dark listening to classical music. The alien mothership is in orbit here. If we can hit that bullseye, the rest of the dominoes will fall like a house of cards. Checkmate.\r\n\r\nFatal. Daddy Bender, we’re hungry. And so we say goodbye to our beloved pet, Nibbler, who’s gone to a place where I, too, hope one day to go. The toilet. Well, thanks to the Internet, I’m now bored with sex. Is there a place on the web that panders to my lust for violence?\r\n\r\n", "Episode 06: Rise of Nomad Destinations"),
    new PodcastResponse(Guid.NewGuid(), DateTime.Now, "/assets/images/podcast/9.jpg", "Perhaps, but perhaps your civilization is merely the sewer of an even greater society above you! I feel like I was mauled by Jesus. Stop! Don’t shoot fire stick in space canoe! Cause explosive decompression!\r\n\r\nWell, let’s just dump it in the sewer and say we delivered it. Fry, you can’t just sit here in the dark listening to classical music. The alien mothership is in orbit here. If we can hit that bullseye, the rest of the dominoes will fall like a house of cards. Checkmate.\r\n\r\nFatal. Daddy Bender, we’re hungry. And so we say goodbye to our beloved pet, Nibbler, who’s gone to a place where I, too, hope one day to go. The toilet. Well, thanks to the Internet, I’m now bored with sex. Is there a place on the web that panders to my lust for violence?\r\n\r\n", "Episode 07: Goodbye boring, hello adventure"),
    new PodcastResponse(Guid.NewGuid(), DateTime.Now, "/assets/images/podcast/24.jpg", "Perhaps, but perhaps your civilization is merely the sewer of an even greater society above you! I feel like I was mauled by Jesus. Stop! Don’t shoot fire stick in space canoe! Cause explosive decompression!\r\n\r\nWell, let’s just dump it in the sewer and say we delivered it. Fry, you can’t just sit here in the dark listening to classical music. The alien mothership is in orbit here. If we can hit that bullseye, the rest of the dominoes will fall like a house of cards. Checkmate.\r\n\r\nFatal. Daddy Bender, we’re hungry. And so we say goodbye to our beloved pet, Nibbler, who’s gone to a place where I, too, hope one day to go. The toilet. Well, thanks to the Internet, I’m now bored with sex. Is there a place on the web that panders to my lust for violence?\r\n\r\n", "Episode 04: The Digital Nomad Lifestyle Facts"),
    new PodcastResponse(Guid.NewGuid(), DateTime.Now, "/assets/images/podcast/23.jpg", "Perhaps, but perhaps your civilization is merely the sewer of an even greater society above you! I feel like I was mauled by Jesus. Stop! Don’t shoot fire stick in space canoe! Cause explosive decompression!\r\n\r\nWell, let’s just dump it in the sewer and say we delivered it. Fry, you can’t just sit here in the dark listening to classical music. The alien mothership is in orbit here. If we can hit that bullseye, the rest of the dominoes will fall like a house of cards. Checkmate.\r\n\r\nFatal. Daddy Bender, we’re hungry. And so we say goodbye to our beloved pet, Nibbler, who’s gone to a place where I, too, hope one day to go. The toilet. Well, thanks to the Internet, I’m now bored with sex. Is there a place on the web that panders to my lust for violence?\r\n\r\n", "Episode 03: Dating As a Digital Nomad"),
    new PodcastResponse(Guid.NewGuid(), DateTime.Now, "/assets/images/podcast/12.jpg", "Perhaps, but perhaps your civilization is merely the sewer of an even greater society above you! I feel like I was mauled by Jesus. Stop! Don’t shoot fire stick in space canoe! Cause explosive decompression!\r\n\r\nWell, let’s just dump it in the sewer and say we delivered it. Fry, you can’t just sit here in the dark listening to classical music. The alien mothership is in orbit here. If we can hit that bullseye, the rest of the dominoes will fall like a house of cards. Checkmate.\r\n\r\nFatal. Daddy Bender, we’re hungry. And so we say goodbye to our beloved pet, Nibbler, who’s gone to a place where I, too, hope one day to go. The toilet. Well, thanks to the Internet, I’m now bored with sex. Is there a place on the web that panders to my lust for violence?\r\n\r\n", "Episode 02: Getting Rid Of Time Consuming Habits"),
    new PodcastResponse(Guid.NewGuid(), DateTime.Now, "/assets/images/podcast/13.jpg", "Perhaps, but perhaps your civilization is merely the sewer of an even greater society above you! I feel like I was mauled by Jesus. Stop! Don’t shoot fire stick in space canoe! Cause explosive decompression!\r\n\r\nWell, let’s just dump it in the sewer and say we delivered it. Fry, you can’t just sit here in the dark listening to classical music. The alien mothership is in orbit here. If we can hit that bullseye, the rest of the dominoes will fall like a house of cards. Checkmate.\r\n\r\nFatal. Daddy Bender, we’re hungry. And so we say goodbye to our beloved pet, Nibbler, who’s gone to a place where I, too, hope one day to go. The toilet. Well, thanks to the Internet, I’m now bored with sex. Is there a place on the web that panders to my lust for violence?\r\n\r\n", "Episode 07: Goodbye boring, hello adventure"),
    new PodcastResponse(Guid.NewGuid(), DateTime.Now, "/assets/images/podcast/14.jpg", "Perhaps, but perhaps your civilization is merely the sewer of an even greater society above you! I feel like I was mauled by Jesus. Stop! Don’t shoot fire stick in space canoe! Cause explosive decompression!\r\n\r\nWell, let’s just dump it in the sewer and say we delivered it. Fry, you can’t just sit here in the dark listening to classical music. The alien mothership is in orbit here. If we can hit that bullseye, the rest of the dominoes will fall like a house of cards. Checkmate.\r\n\r\nFatal. Daddy Bender, we’re hungry. And so we say goodbye to our beloved pet, Nibbler, who’s gone to a place where I, too, hope one day to go. The toilet. Well, thanks to the Internet, I’m now bored with sex. Is there a place on the web that panders to my lust for violence?\r\n\r\n", "Episode 04: The Digital Nomad Lifestyle Facts"),
    new PodcastResponse(Guid.NewGuid(), DateTime.Now, "/assets/images/podcast/15.jpg", "Perhaps, but perhaps your civilization is merely the sewer of an even greater society above you! I feel like I was mauled by Jesus. Stop! Don’t shoot fire stick in space canoe! Cause explosive decompression!\r\n\r\nWell, let’s just dump it in the sewer and say we delivered it. Fry, you can’t just sit here in the dark listening to classical music. The alien mothership is in orbit here. If we can hit that bullseye, the rest of the dominoes will fall like a house of cards. Checkmate.\r\n\r\nFatal. Daddy Bender, we’re hungry. And so we say goodbye to our beloved pet, Nibbler, who’s gone to a place where I, too, hope one day to go. The toilet. Well, thanks to the Internet, I’m now bored with sex. Is there a place on the web that panders to my lust for violence?\r\n\r\n", "Episode 03: Dating As a Digital Nomad"),
    new PodcastResponse(Guid.NewGuid(), DateTime.Now, "/assets/images/podcast/16.jpg", "Perhaps, but perhaps your civilization is merely the sewer of an even greater society above you! I feel like I was mauled by Jesus. Stop! Don’t shoot fire stick in space canoe! Cause explosive decompression!\r\n\r\nWell, let’s just dump it in the sewer and say we delivered it. Fry, you can’t just sit here in the dark listening to classical music. The alien mothership is in orbit here. If we can hit that bullseye, the rest of the dominoes will fall like a house of cards. Checkmate.\r\n\r\nFatal. Daddy Bender, we’re hungry. And so we say goodbye to our beloved pet, Nibbler, who’s gone to a place where I, too, hope one day to go. The toilet. Well, thanks to the Internet, I’m now bored with sex. Is there a place on the web that panders to my lust for violence?\r\n\r\n", "Episode 02: Getting Rid Of Time Consuming Habits"),
    new PodcastResponse(Guid.NewGuid(), DateTime.Now, "/assets/images/podcast/17.jpg", "Perhaps, but perhaps your civilization is merely the sewer of an even greater society above you! I feel like I was mauled by Jesus. Stop! Don’t shoot fire stick in space canoe! Cause explosive decompression!\r\n\r\nWell, let’s just dump it in the sewer and say we delivered it. Fry, you can’t just sit here in the dark listening to classical music. The alien mothership is in orbit here. If we can hit that bullseye, the rest of the dominoes will fall like a house of cards. Checkmate.\r\n\r\nFatal. Daddy Bender, we’re hungry. And so we say goodbye to our beloved pet, Nibbler, who’s gone to a place where I, too, hope one day to go. The toilet. Well, thanks to the Internet, I’m now bored with sex. Is there a place on the web that panders to my lust for violence?\r\n\r\n", "Episode 06: Rise of Nomad Destinations"),
    new PodcastResponse(Guid.NewGuid(), DateTime.Now, "/assets/images/podcast/18.jpg", "Perhaps, but perhaps your civilization is merely the sewer of an even greater society above you! I feel like I was mauled by Jesus. Stop! Don’t shoot fire stick in space canoe! Cause explosive decompression!\r\n\r\nWell, let’s just dump it in the sewer and say we delivered it. Fry, you can’t just sit here in the dark listening to classical music. The alien mothership is in orbit here. If we can hit that bullseye, the rest of the dominoes will fall like a house of cards. Checkmate.\r\n\r\nFatal. Daddy Bender, we’re hungry. And so we say goodbye to our beloved pet, Nibbler, who’s gone to a place where I, too, hope one day to go. The toilet. Well, thanks to the Internet, I’m now bored with sex. Is there a place on the web that panders to my lust for violence?\r\n\r\n", "Episode 02: Getting Rid Of Time Consuming Habits"),
    new PodcastResponse(Guid.NewGuid(), DateTime.Now, "/assets/images/podcast/19.jpg", "Perhaps, but perhaps your civilization is merely the sewer of an even greater society above you! I feel like I was mauled by Jesus. Stop! Don’t shoot fire stick in space canoe! Cause explosive decompression!\r\n\r\nWell, let’s just dump it in the sewer and say we delivered it. Fry, you can’t just sit here in the dark listening to classical music. The alien mothership is in orbit here. If we can hit that bullseye, the rest of the dominoes will fall like a house of cards. Checkmate.\r\n\r\nFatal. Daddy Bender, we’re hungry. And so we say goodbye to our beloved pet, Nibbler, who’s gone to a place where I, too, hope one day to go. The toilet. Well, thanks to the Internet, I’m now bored with sex. Is there a place on the web that panders to my lust for violence?\r\n\r\n", "Episode 06: Rise of Nomad Destinations"),
    new PodcastResponse(Guid.NewGuid(), DateTime.Now, "/assets/images/podcast/20.jpg", "Perhaps, but perhaps your civilization is merely the sewer of an even greater society above you! I feel like I was mauled by Jesus. Stop! Don’t shoot fire stick in space canoe! Cause explosive decompression!\r\n\r\nWell, let’s just dump it in the sewer and say we delivered it. Fry, you can’t just sit here in the dark listening to classical music. The alien mothership is in orbit here. If we can hit that bullseye, the rest of the dominoes will fall like a house of cards. Checkmate.\r\n\r\nFatal. Daddy Bender, we’re hungry. And so we say goodbye to our beloved pet, Nibbler, who’s gone to a place where I, too, hope one day to go. The toilet. Well, thanks to the Internet, I’m now bored with sex. Is there a place on the web that panders to my lust for violence?\r\n\r\n", "Episode 07: Goodbye boring, hello adventure"),
    new PodcastResponse(Guid.NewGuid(), DateTime.Now, "/assets/images/podcast/21.jpg", "Perhaps, but perhaps your civilization is merely the sewer of an even greater society above you! I feel like I was mauled by Jesus. Stop! Don’t shoot fire stick in space canoe! Cause explosive decompression!\r\n\r\nWell, let’s just dump it in the sewer and say we delivered it. Fry, you can’t just sit here in the dark listening to classical music. The alien mothership is in orbit here. If we can hit that bullseye, the rest of the dominoes will fall like a house of cards. Checkmate.\r\n\r\nFatal. Daddy Bender, we’re hungry. And so we say goodbye to our beloved pet, Nibbler, who’s gone to a place where I, too, hope one day to go. The toilet. Well, thanks to the Internet, I’m now bored with sex. Is there a place on the web that panders to my lust for violence?\r\n\r\n", "Episode 04: The Digital Nomad Lifestyle Facts"),
    new PodcastResponse(Guid.NewGuid(), DateTime.Now, "/assets/images/podcast/22.jpg", "Perhaps, but perhaps your civilization is merely the sewer of an even greater society above you! I feel like I was mauled by Jesus. Stop! Don’t shoot fire stick in space canoe! Cause explosive decompression!\r\n\r\nWell, let’s just dump it in the sewer and say we delivered it. Fry, you can’t just sit here in the dark listening to classical music. The alien mothership is in orbit here. If we can hit that bullseye, the rest of the dominoes will fall like a house of cards. Checkmate.\r\n\r\nFatal. Daddy Bender, we’re hungry. And so we say goodbye to our beloved pet, Nibbler, who’s gone to a place where I, too, hope one day to go. The toilet. Well, thanks to the Internet, I’m now bored with sex. Is there a place on the web that panders to my lust for violence?\r\n\r\n", "Episode 03: Dating As a Digital Nomad"),
    new PodcastResponse(Guid.NewGuid(), DateTime.Now, "/assets/images/podcast/11.jpg", "Perhaps, but perhaps your civilization is merely the sewer of an even greater society above you! I feel like I was mauled by Jesus. Stop! Don’t shoot fire stick in space canoe! Cause explosive decompression!\r\n\r\nWell, let’s just dump it in the sewer and say we delivered it. Fry, you can’t just sit here in the dark listening to classical music. The alien mothership is in orbit here. If we can hit that bullseye, the rest of the dominoes will fall like a house of cards. Checkmate.\r\n\r\nFatal. Daddy Bender, we’re hungry. And so we say goodbye to our beloved pet, Nibbler, who’s gone to a place where I, too, hope one day to go. The toilet. Well, thanks to the Internet, I’m now bored with sex. Is there a place on the web that panders to my lust for violence?\r\n\r\n", "Episode 02: Getting Rid Of Time Consuming Habits"),
    new PodcastResponse(Guid.NewGuid(), DateTime.Now, "/assets/images/podcast/10.jpg", "Perhaps, but perhaps your civilization is merely the sewer of an even greater society above you! I feel like I was mauled by Jesus. Stop! Don’t shoot fire stick in space canoe! Cause explosive decompression!\r\n\r\nWell, let’s just dump it in the sewer and say we delivered it. Fry, you can’t just sit here in the dark listening to classical music. The alien mothership is in orbit here. If we can hit that bullseye, the rest of the dominoes will fall like a house of cards. Checkmate.\r\n\r\nFatal. Daddy Bender, we’re hungry. And so we say goodbye to our beloved pet, Nibbler, who’s gone to a place where I, too, hope one day to go. The toilet. Well, thanks to the Internet, I’m now bored with sex. Is there a place on the web that panders to my lust for violence?\r\n\r\n", "Episode 02: Getting Rid Of Time Consuming Habits"),
    new PodcastResponse(Guid.NewGuid(), DateTime.Now, "/assets/images/podcast/25.jpg", "Perhaps, but perhaps your civilization is merely the sewer of an even greater society above you! I feel like I was mauled by Jesus. Stop! Don’t shoot fire stick in space canoe! Cause explosive decompression!\r\n\r\nWell, let’s just dump it in the sewer and say we delivered it. Fry, you can’t just sit here in the dark listening to classical music. The alien mothership is in orbit here. If we can hit that bullseye, the rest of the dominoes will fall like a house of cards. Checkmate.\r\n\r\nFatal. Daddy Bender, we’re hungry. And so we say goodbye to our beloved pet, Nibbler, who’s gone to a place where I, too, hope one day to go. The toilet. Well, thanks to the Internet, I’m now bored with sex. Is there a place on the web that panders to my lust for violence?\r\n\r\n", "Episode 02: Getting Rid Of Time Consuming Habits"),
    new PodcastResponse(Guid.NewGuid(), DateTime.Now, "/assets/images/podcast/5.jpg", "Perhaps, but perhaps your civilization is merely the sewer of an even greater society above you! I feel like I was mauled by Jesus. Stop! Don’t shoot fire stick in space canoe! Cause explosive decompression!\r\n\r\nWell, let’s just dump it in the sewer and say we delivered it. Fry, you can’t just sit here in the dark listening to classical music. The alien mothership is in orbit here. If we can hit that bullseye, the rest of the dominoes will fall like a house of cards. Checkmate.\r\n\r\nFatal. Daddy Bender, we’re hungry. And so we say goodbye to our beloved pet, Nibbler, who’s gone to a place where I, too, hope one day to go. The toilet. Well, thanks to the Internet, I’m now bored with sex. Is there a place on the web that panders to my lust for violence?\r\n\r\n", "Episode 02: Getting Rid Of Time Consuming Habits"),
];

    }

    public async Task<ServiceResult<List<PodcastResponse>>> GetAllAsync(HttpContext context, CancellationToken cancellationToken, string? lang = "en-US")
    {
        var baseUrl = context.Request.Scheme + "://" + context.Request.Host.Value;

        foreach (var podcast in _podcastList)
        {
            if (!podcast.Image.StartsWith(baseUrl))
                podcast.Image = baseUrl + podcast.Image;
        }

        return ServiceResult<List<PodcastResponse>>.Success(_podcastList);
    }


    public async Task<ServiceResult<PodcastResponse>> GetByIdAsync(Guid id, HttpContext context, CancellationToken cancellationToken, string? lang = "en-US")
    {
        var baseUrl = context.Request.Scheme + "://" + context.Request.Host.Value;

        var podcast = _podcastList.FirstOrDefault(x => x.Id == id);
        if (podcast == null)
        {
            return ServiceResult<PodcastResponse>.Failure("Podcast not found", statusCode: 404);
        }
        if (!podcast.Image.StartsWith(baseUrl))
            podcast.Image = baseUrl + podcast.Image;

        return ServiceResult<PodcastResponse>.Success(podcast);
    }





}


