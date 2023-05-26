using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text.Json;

namespace WebClient.Datasource;

public class ApiClient : IApiClient
{
    protected readonly HttpClient Client;
    protected readonly string BaseUri;

    public ApiClient(IOptions<AppSettings> appSettings)
    {
        Client = new HttpClient();
        BaseUri = appSettings.Value.BaseUrl;
    }

    public async Task<T?> GetAsync<T>(string url = "")
    {
        HttpResponseMessage response = await Client.GetAsync($"{BaseUri}{url}");
        string strData = await response.Content.ReadAsStringAsync();
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };
        return System.Text.Json.JsonSerializer.Deserialize<T>(strData, options);
    }

    public async Task PostAsync<T>(T body, string url = "")
    {
        var content = JsonConvert.SerializeObject(body);
        var buffer = System.Text.Encoding.UTF8.GetBytes(content);
        var byteContent = new ByteArrayContent(buffer);
        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        await Client.PostAsync($"{BaseUri}{url}", byteContent);
    }

    public async Task PutAsync<T>(T body, string url = "")
    {
        var content = JsonConvert.SerializeObject(body);
        var buffer = System.Text.Encoding.UTF8.GetBytes(content);
        var byteContent = new ByteArrayContent(buffer);
        byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        await Client.PutAsync($"{BaseUri}{url}", byteContent);
    }

    public async Task DeleteAsync<T>(string url = "")
    {
        await Client.DeleteAsync($"{BaseUri}{url}");
    }
}
