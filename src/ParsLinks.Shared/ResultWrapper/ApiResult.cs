using System.Net;

namespace ParsLinks.Shared.ResultWrapper
{
    public class ApiResult : IApiResult
    {
        public ApiResult()
        {
        }

        public List<string> Messages { get; set; } = new List<string>();

        public bool Succeeded { get; set; }
        public HttpStatusCode StatusCode { get; set; }

        public static IApiResult Fail()
        {
            return new ApiResult { Succeeded = false, StatusCode = HttpStatusCode.InternalServerError };
        }
        public static IApiResult Fail(List<string>? messages)
        {
            return new ApiResult { Succeeded = false, Messages = messages };
        }
        public static IApiResult Fail(string message)
        {
            return new ApiResult { Succeeded = false, Messages = new List<string> { message } };
        }
        public static IApiResult Fail(string message, HttpStatusCode statusCode)
        {
            return new ApiResult { Succeeded = false, Messages = new List<string> { message }, StatusCode = statusCode };
        }

        public static IApiResult Fail(List<string> messages, HttpStatusCode statusCode)
        {
            return new ApiResult { Succeeded = false, Messages = messages, StatusCode = statusCode };
        }

        public static Task<IApiResult> FailAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Fail());
        }

        public static Task<IApiResult> FailAsync(string message, HttpStatusCode statusCode, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Fail(message, statusCode));
        }

        public static Task<IApiResult> FailAsync(List<string> messages, HttpStatusCode statusCode, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Fail(messages, statusCode));
        }

        public static IApiResult Success()
        {
            return new ApiResult { Succeeded = true, StatusCode = HttpStatusCode.OK };
        }


        public static IApiResult Success(string message)
        {
            return new ApiResult { Succeeded = true, Messages = new List<string> { message }, StatusCode = HttpStatusCode.OK };
        }


        public static IApiResult Success(string message, HttpStatusCode statusCode)
        {
            return new ApiResult { Succeeded = true, Messages = new List<string> { message }, StatusCode = statusCode };
        }



        public static Task<IApiResult> SuccessAsync(string message, HttpStatusCode statusCode, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Success(message, statusCode));
        }
        public static Task<IApiResult> SuccessAsync(string message, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Success(message, HttpStatusCode.OK));
        }
    }

    public class ApiResult<T> : ApiResult, IApiResult<T>
    {
        public ApiResult()
        {
        }

        public T Data { get; set; }

        public static new ApiResult<T> Fail()
        {
            return new ApiResult<T> { Succeeded = false, StatusCode = HttpStatusCode.InternalServerError };
        }

        public static new ApiResult<T> Fail(string message)
        {
            return new ApiResult<T> { Succeeded = false, Messages = new List<string> { message } };
        }


        public static new ApiResult<T> Fail(string message, HttpStatusCode statusCode)
        {
            return new ApiResult<T> { Succeeded = false, Messages = new List<string> { message }, StatusCode = statusCode };
        }



        public static new ApiResult<T> Fail(List<string>? messages)
        {
            return new ApiResult<T> { Succeeded = false, Messages = messages };
        }



        public static new ApiResult<T> Fail(List<string> messages, HttpStatusCode statusCode)
        {
            return new ApiResult<T> { Succeeded = false, Messages = messages, StatusCode = statusCode };
        }

        public static new Task<ApiResult<T>> FailAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Fail());
        }

        public static new Task<ApiResult<T>> FailAsync(string message, HttpStatusCode statusCode, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Fail(message, statusCode));
        }

        public static new Task<ApiResult<T>> FailAsync(List<string> messages, HttpStatusCode statusCode, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Fail(messages, statusCode));
        }



        public static new ApiResult<T> Success()
        {
            return new ApiResult<T> { Succeeded = true, StatusCode = HttpStatusCode.OK };
        }

        public static new ApiResult<T> Success(string message, HttpStatusCode statusCode)
        {
            return new ApiResult<T> { Succeeded = true, Messages = new List<string> { message }, StatusCode = statusCode };
        }

        public static ApiResult<T> Success(T data, HttpStatusCode statusCode)
        {
            return new ApiResult<T> { Succeeded = true, Data = data, StatusCode = statusCode };
        }
        public static ApiResult<T> Success(T data)
        {
            return new ApiResult<T> { Succeeded = true, Data = data, StatusCode = HttpStatusCode.OK };
        }

        public static ApiResult<T> Success(T data, string message)
        {
            return new ApiResult<T> { Succeeded = true, Data = data, Messages = new List<string> { message }, StatusCode = HttpStatusCode.OK };
        }

        public static ApiResult<T> Success(T data, string message, HttpStatusCode statusCode)
        {
            return new ApiResult<T> { Succeeded = true, Data = data, Messages = new List<string> { message }, StatusCode = statusCode };
        }

        public static ApiResult<T> Success(T data, List<string> messages, HttpStatusCode statusCode)
        {
            return new ApiResult<T> { Succeeded = true, Data = data, Messages = messages, StatusCode = statusCode };
        }

        public static new Task<ApiResult<T>> SuccessAsync(string message, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Success(message, HttpStatusCode.OK));
        }

        public static new Task<ApiResult<T>> SuccessAsync(string message, HttpStatusCode statusCode, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Success(message, statusCode));
        }

        public static Task<ApiResult<T>> SuccessAsync(T data, HttpStatusCode statusCode, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Success(data, statusCode));
        }
        public static Task<ApiResult<T>> SuccessAsync(T data, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Success(data));
        }
        public static Task<ApiResult<T>> SuccessAsync(T data, string message, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Success(data, message, HttpStatusCode.OK));
        }

        public static Task<ApiResult<T>> SuccessAsync(T data, string message, HttpStatusCode statusCode, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Success(data, message, statusCode));
        }
        public static Task<ApiResult<T>> SuccessAsync(T data, List<string> messages, HttpStatusCode statusCode, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(Success(data, messages, statusCode));
        }

    }
}