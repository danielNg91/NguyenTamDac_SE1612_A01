namespace WebClient.Datasource;

public interface IApiClient
{
    Task<T?> GetAsync<T>(string url = "");
    Task PostAsync<T>(T body, string url = "");
    Task PutAsync<T>(T body, string url = "");
    Task DeleteAsync<T>(string url = "");
}
