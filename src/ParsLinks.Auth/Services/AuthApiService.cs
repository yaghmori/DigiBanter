using ParsLinks.Shared.Constatns;
using ParsLinks.Shared.Dto.Request;
using ParsLinks.Shared.Dto.Response;
using ParsLinks.Shared.Extensions;
using ParsLinks.Shared.ResultWrapper;
using System.Text;

namespace ParsLinks.Auth.Services;

public class AuthApiService
{
    public readonly HttpClient _httpClient;
    public readonly IJsonSerializer _jsonService;

    public AuthApiService(IHttpClientFactory httpClientFactory, IJsonSerializer jsonService)
    {
        _httpClient = httpClientFactory.CreateClient(AppClientTypes.Master);
        _jsonService = jsonService;
    }

    public async Task<IApiResult<TokenResponse>> LoginAsync(LoginByEmailRequest request, CancellationToken cancellationToken = default)
    {
        var uri = AppEndPoints.Auth.Login;
        var content = new StringContent(_jsonService.Serialize(request), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(uri, content, cancellationToken);
        return await response.ToResultAsync<TokenResponse>(_jsonService, cancellationToken);

    }
    public async Task<IApiResult<string>> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var uri = AppEndPoints.Auth.Register;
        var content = new StringContent(_jsonService.Serialize(request), Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(uri, content, cancellationToken);
        return await response.ToResultAsync<string>(_jsonService, cancellationToken);

    }

    public async Task<IApiResult<List<UserResponse>>> GetUserAsync(CancellationToken cancellationToken = default)
    {
        var uri = AppEndPoints.User.GetUsers;
        var response = await _httpClient.GetAsync(uri, cancellationToken);
        return await response.ToResultAsync<List<UserResponse>>(_jsonService, cancellationToken);

    }
    public async Task<IApiResult<UserResponse>> GetUserByIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        var uri = string.Format(AppEndPoints.User.GetUserById, userId);
        var response = await _httpClient.GetAsync(uri, cancellationToken);
        return await response.ToResultAsync<UserResponse>(_jsonService, cancellationToken);

    }
}
