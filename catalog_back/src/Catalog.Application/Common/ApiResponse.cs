namespace Catalog.Application.Common;

public class ApiResponse<T>
{
    public T? Data { get; set; }
    public bool Success { get; set; }
    public string? Message { get; set; }

    public static ApiResponse<T> SuccessResponse(T data, string? message = null)
    {
        return new(){Data = data, Success = true, Message = message};
    }

    public static ApiResponse<T> ErrorResponse(string message)
    {
        return new(){Data = default, Success = false, Message = message};
    }
}