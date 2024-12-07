using System.Net;

namespace ParsLinks.Shared.ResultWrapper
{
    public interface IApiResult
    {
        List<string> Messages { get; set; }

        bool Succeeded { get; set; }
        HttpStatusCode StatusCode { get; set; }
    }

    public interface IApiResult<out T> : IApiResult
    {
        T Data { get; }
    }
}