using ParsLinks.Shared.ResultWrapper;


namespace ParsLinks.Shared.Extensions;
public static class ApiResultExensions
{

    public static async Task<IApiResult<T>> ToResultAsync<T>(this HttpResponseMessage response, IJsonSerializer jsonService, CancellationToken cancellationToken = default)
    {
        var responseAsString = await response.Content.ReadAsStringAsync(cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            return jsonService.Deserialize<ApiResult<T>>(responseAsString)!;
        }
        else
        {

            return await ApiResult<T>.FailAsync(responseAsString, response.StatusCode, cancellationToken);
        }
    }


    public static async Task<IApiResult> ToResultAsync(this HttpResponseMessage response, IJsonSerializer jsonService, CancellationToken cancellationToken = default)
    {
        var responseAsString = await response.Content.ReadAsStringAsync(cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            return jsonService.Deserialize<ApiResult<IApiResult>>(responseAsString)!;
        }
        else
        {
            return await ApiResult<IApiResult>.FailAsync(responseAsString, response.StatusCode, cancellationToken);
        }
    }



}
