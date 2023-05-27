namespace WebClient.Datasource;

public interface IApiClient
{
    Task<M?> GetAsync<M>(string url);
    Task<M?> PostAsync<M, T>(string url, T body);
    Task<M?> PutAsync<M, T>(string url, T body);
    Task<M?> DeleteAsync<M, T>(string url);
}
