namespace WebClient.Datasource;

public class WrapResponse<T>
{
    public string? Message { get; set; }
    public bool IsError { get; set; }
    public T? Result { get; set; }
    public ResponseException? ResponseException { get; set; }
}

public class ResponseException
{
    public string ExceptionMessage { get; set; }
}