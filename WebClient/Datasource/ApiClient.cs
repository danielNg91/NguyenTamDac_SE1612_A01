using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text.Json;

namespace WebClient.Datasource;

public class ApiClient : IApiClient
{
    protected readonly HttpClient Client;

    public ApiClient(IOptions<AppSettings> appSettings)
    {
        Client = new HttpClient();
    }

    public async Task<T?> GetAsync<T>(string url)
    {
        HttpResponseMessage response = await Client.GetAsync(url);
        return await Deserialize<T>(response);
    }

    public async Task<T?> PostAsync<T>(string url, T body)
    {
        var byteContent = ConvertToByteContent(body);
        HttpResponseMessage response = await Client.PostAsync(url, byteContent);
        return await Deserialize<T>(response);
    }

    public async Task<T?> PutAsync<T>(string url, T body)
    {
        var byteContent = ConvertToByteContent(body);
        HttpResponseMessage response = await Client.PutAsync(url, byteContent);
        return await Deserialize<T>(response);
    }

    public async Task<T?> DeleteAsync<T>(string url)
    {
        HttpResponseMessage response = await Client.DeleteAsync(url);
        return await Deserialize<T>(response);
    }

    private async Task<T?> Deserialize<T>(HttpResponseMessage response)
    {
        string strData = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };
        return System.Text.Json.JsonSerializer.Deserialize<T>(strData, options);
    }

    private ByteArrayContent ConvertToByteContent<T>(T body)
    {
        var content = JsonConvert.SerializeObject(body);
        var buffer = System.Text.Encoding.UTF8.GetBytes(content);
        var byteContent = new ByteArrayContent(buffer);
        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        return byteContent;
    }
}
