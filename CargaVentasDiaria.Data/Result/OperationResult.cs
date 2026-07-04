namespace CargaVentasDiaria.Data.Result
{
    public class OperationResult
    {
        public OperationResult() { Success = true; }

        public bool Success { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }

        public static OperationResult Ok() => new() { Success = true };
        public static OperationResult Ok(object? data) => new() { Success = true, Data = data };
        public static OperationResult Fail(string message) => new() { Success = false, Message = message };
    }

    public class OperationResult<T> : OperationResult
    {
        public new T? Data { get; set; }

        public static OperationResult<T> Ok(T data) => new() { Success = true, Data = data };
        public static new OperationResult<T> Fail(string message) => new() { Success = false, Message = message };
    }
}
