using ParsLinks.Shared.Constatns;
using ParsLinks.Shared.Dto.Response;
using ParsLinks.Shared.Extensions;
using ParsLinks.Shared.ResultWrapper;
using ParsLinks.Shared.Services;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace ParsLinks.Application.ApiServices
{

    public class BlogPostApiService : BaseHttpClient, IBlogPostApiService
    {
        private readonly ITimeZoneHelper _timeZoneService;

        public BlogPostApiService(IHttpClientFactory httpClient, ITimeZoneHelper timeZoneService, IJsonSerializer jsonService)
            : base(httpClient, timeZoneService, jsonService)
        {
            _timeZoneService = timeZoneService;
        }

        public async Task<IApiResult<List<BlogPostResponse>>> GetAllPostsAsync(string? lang="en-US",CancellationToken cancellationToken = default)
        {
            var uri = AppEndPoints.BlogPosts.Base;

            if (!string.IsNullOrWhiteSpace(lang))
                uri = QueryHelpers.AddQueryString(uri, nameof(lang), lang);

            var response = await _httpClient.GetAsync(uri, cancellationToken);
            return await response.ToResultAsync<List<BlogPostResponse>>(_jsonService, cancellationToken);
        }

        public async Task<IApiResult<BlogPostResponse>> GetPostByIdAsync(int postId, string? lang = "en-US", CancellationToken cancellationToken = default)
        {
            var uri = string.Format(AppEndPoints.BlogPosts.GetById, postId, cancellationToken);

            if (!string.IsNullOrWhiteSpace(lang))
                uri = QueryHelpers.AddQueryString(uri, nameof(lang), lang);

            var response = await _httpClient.GetAsync(uri, cancellationToken);
            return await response.ToResultAsync<BlogPostResponse>(_jsonService, cancellationToken);
        }
    }
}
