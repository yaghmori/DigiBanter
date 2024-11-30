namespace DigiBanter.Api.Services
{
    public class ServiceResult<T>
    {
        public bool IsSuccess { get; private set; }
        public string? ErrorMessage { get; private set; }
        public T? Data { get; private set; }
        public int StatusCode { get; private set; }

        public static ServiceResult<T> Success(T? data, int statusCode = 200)
        {
            return new ServiceResult<T> { IsSuccess = true, Data = data, StatusCode = statusCode };
        }

        public static ServiceResult<T> Failure(string errorMessage, int statusCode = 400)
        {
            return new ServiceResult<T> { IsSuccess = false, ErrorMessage = errorMessage, StatusCode = statusCode };
        }
    }

    public class ServiceResult
    {
        public bool IsSuccess { get; private set; }
        public string? ErrorMessage { get; private set; }
        public int StatusCode { get; private set; }

        public static ServiceResult Success(int statusCode = 200)
        {
            return new ServiceResult { IsSuccess = true, StatusCode = statusCode };
        }

        public static ServiceResult Failure(string errorMessage, int statusCode = 400)
        {
            return new ServiceResult { IsSuccess = false, ErrorMessage = errorMessage, StatusCode = statusCode };
        }
    }

}
