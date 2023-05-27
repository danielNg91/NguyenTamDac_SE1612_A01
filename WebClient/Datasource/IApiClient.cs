namespace WebClient.Datasource;

public interface IApiClient
{
    Task<T?> GetAsync<T>(string url);
    Task<T?> PostAsync<T>(string url, T body);
    Task<T?> PutAsync<T>(string url, T body);
    Task<T?> DeleteAsync<T>(string url);
}
