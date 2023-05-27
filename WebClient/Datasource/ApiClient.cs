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
        Client.DefaultRequestHeaders.ConnectionClose = true;
    }

    public async Task<M?> GetAsync<M>(string url)
    {
        HttpResponseMessage response = await Client.GetAsync(url);
        return await Deserialize<M>(response);
    }

    public async Task<M?> PostAsync<M, T>(string url, T body)
    {
        var byteContent = ConvertToByteContent<T>(body);
        HttpResponseMessage response = await Client.PostAsync(url, byteContent);
        return await Deserialize<M>(response);
    }

    public async Task<M?> PutAsync<M, T>(string url, T body)
    {
        var byteContent = ConvertToByteContent<T>(body);
        HttpResponseMessage response = await Client.PutAsync(url, byteContent);
        return await Deserialize<M>(response);
    }

    public async Task<M?> DeleteAsync<M, T>(string url)
    {
        HttpResponseMessage response = await Client.DeleteAsync(url);
        return await Deserialize<M>(response);
    }

    private async Task<M?> Deserialize<M>(HttpResponseMessage response)
    {
        response.EnsureSuccessStatusCode();
        string strData = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrEmpty(strData))
        {
            return default(M);
        }
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };
        var wrapResponse = System.Text.Json.JsonSerializer.Deserialize<WrapResponse<M>>(strData, options);
        return wrapResponse.Result;
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
