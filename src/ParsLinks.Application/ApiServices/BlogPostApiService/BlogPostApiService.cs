using ParsLinks.Domain.Enums;
using ParsLinks.Shared.Constatns;
using ParsLinks.Shared.Dto;
using ParsLinks.Shared.Dto.Request;
using ParsLinks.Shared.Dto.Response;
using ParsLinks.Shared.Extensions;
using ParsLinks.Shared.Models;
using ParsLinks.Shared.PagedCollections;
using ParsLinks.Shared.ResultWrapper;
using ParsLinks.Shared.Services;
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

        public async Task<IApiResult<int>> AddPostAsync(FileDto image, BlogPostRequest request, CancellationToken cancellationToken = default)
        {
            var uri = AppEndPoints.BlogPosts.AddPost;

            var formData = new MultipartFormDataContent();

            image.StreamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(image.ContentType);
            formData.Add(image.StreamContent, nameof(image), image.Name);
            formData.Add(new StringContent(_jsonService.Serialize(request), Encoding.UTF8, "application/json"), nameof(request));

            var response = await _httpClient.PostAsync(uri, formData, cancellationToken);
            return await response.ToResultAsync<int>(_jsonService, cancellationToken);
        }
        public async Task<IApiResult> UpdatePostAsync(FileDto? image, BlogPostRequest request, CancellationToken cancellationToken = default)
        {
            var uri = AppEndPoints.BlogPosts.UpdateById;

            var formData = new MultipartFormDataContent();
            if (image != null)
            {
                image.StreamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(image.ContentType);
                formData.Add(image.StreamContent, nameof(image), image.Name);

            }
            formData.Add(new StringContent(_jsonService.Serialize(request), Encoding.UTF8, "application/json"), nameof(request));

            var response = await _httpClient.PatchAsync(uri, formData, cancellationToken);
            return await response.ToResultAsync(_jsonService, cancellationToken);
        }


        public async Task<IApiResult> DeletePostByIdAsync(int postId, CancellationToken cancellationToken = default)
        {
            var uri = string.Format(AppEndPoints.BlogPosts.DeleteById, postId);
            var response = await _httpClient.DeleteAsync(uri, cancellationToken);
            return await response.ToResultAsync(_jsonService, cancellationToken);
        }

        public async Task<IApiResult<List<BlogPostResponse>>> GetAllPostsAsync(BlogPostQueryParameters? parameters = null, CancellationToken cancellationToken = default)
        {
            if (parameters == null)
                parameters = new();
            parameters.Paged = false;

            var queryString = parameters.ToQueryString();
            var uri = $"{AppEndPoints.BlogPosts.Base}{queryString}";

            var response = await _httpClient.GetAsync(uri, cancellationToken);
            return await response.ToResultAsync<List<BlogPostResponse>>(_jsonService, cancellationToken);
        }

        public async Task<IApiResult<IPagedList<BlogPostResponse>>> GetPagedAllPostsAsync(BlogPostQueryParameters? parameters = null, CancellationToken cancellationToken = default)
        {
            if (parameters == null)
                parameters = new();

            parameters.Paged = true;
            var queryString = parameters.ToQueryString();
            var uri = $"{AppEndPoints.BlogPosts.Base}{queryString}";
            var response = await _httpClient.GetAsync(uri, cancellationToken);
            return await response.ToResultAsync<PagedList<BlogPostResponse>>(_jsonService, cancellationToken);
        }
        public async Task<IApiResult<IVirtualizedList<BlogPostResponse>>> GetVirtualizedAllPostsAsync(BlogPostQueryParameters? parameters = null, CancellationToken cancellationToken = default)
        {
            if (parameters == null)
                parameters = new();

            var queryString = parameters.ToQueryString();
            var uri = $"{AppEndPoints.BlogPosts.Base}{queryString}";
            var response = await _httpClient.GetAsync(uri, cancellationToken);
            return await response.ToResultAsync<VirtualizedList<BlogPostResponse>>(_jsonService, cancellationToken);
        }

        public async Task<IApiResult<BlogPostResponse>> GetPostByIdAsync(int postId, BlogPostQueryParameters? parameters = null, CancellationToken cancellationToken = default)
        {
            if (parameters == null)
                parameters = new();

            var queryString = parameters.ToQueryString();
            var uri = $"{string.Format(AppEndPoints.BlogPosts.GetById, postId)}{queryString}";


            var response = await _httpClient.GetAsync(uri, cancellationToken);
            return await response.ToResultAsync<BlogPostResponse>(_jsonService, cancellationToken);
        }

        public async Task<IApiResult<BlogPostRequest>> GetPostByDetailIdAsync(int postId, CancellationToken cancellationToken = default)
        {
            var uri = $"{string.Format(AppEndPoints.BlogPosts.GetDetailById, postId)}";


            var response = await _httpClient.GetAsync(uri, cancellationToken);
            return await response.ToResultAsync<BlogPostRequest>(_jsonService, cancellationToken);
        }
        public async Task<IApiResult> ChangePostStatusAsync(int postId, BlogPostStatusEnum status, CancellationToken cancellationToken = default)
        {
            var uri = $"{string.Format(AppEndPoints.BlogPosts.ChangeStatus, postId)}";
            var content = new StringContent(_jsonService.Serialize(status), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(uri, content, cancellationToken);
            return await response.ToResultAsync(_jsonService, cancellationToken);
        }
    }
}
