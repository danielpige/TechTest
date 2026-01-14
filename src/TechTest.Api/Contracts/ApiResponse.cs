namespace TechTest.Api.Contracts
{
    public sealed record ApiResponse<T>( bool Success, string Message, T? Data)
    {
        public static ApiResponse<T> Ok(T data, string message = "Operación exitosa.")
            => new(true, message, data);

        public static ApiResponse<T> Ok(string message = "Operación exitosa.")
            => new(true, message, default);

        public static ApiResponse<T> Created(T data, string message = "Recurso creado exitosamente.")
            => new(true, message, data);
    }

    public sealed record ApiResponse( bool Success, string Message)
    {
        public static ApiResponse Ok(string message = "Operación exitosa.")
            => new(true, message);
    }
}
