using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace TechStore.Tests.Extensions;

public static class HttpClientExtensions
{
    public static async Task<(HttpStatusCode StatusCode, string Response)> DoGet(this HttpClient client, string token, string endpoint)
    {
        client.SetupAuthorizationHeader(token);

        var response = await client.GetAsync(new Uri(endpoint, UriKind.Relative));
        var content = await response.Content.ReadAsStringAsync();

        return (response.StatusCode, content);
    }
    
    public static async Task<(HttpStatusCode StatusCode, TResponse Response)> DoGet<TResponse>(this HttpClient client, string token, string endpoint)
    {
        client.SetupAuthorizationHeader(token);
        
        var responseMessage = await client.GetAsync(new Uri(endpoint, UriKind.Relative));
        var responseJson = await responseMessage.Content.ReadAsStringAsync();

        var response = JsonSerializer.Deserialize<TResponse>(responseJson);
        return (responseMessage.StatusCode, response);
    }

    public static async Task<(HttpStatusCode StatusCode, string Response)> DoPost<TPayload>(this HttpClient client, string token, string endpoint, TPayload payload)
    {
        client.SetupAuthorizationHeader(token);
        
        using var content = JsonContent.Create(payload, typeof(TPayload));
        var response = await client.PostAsync(new Uri(endpoint, UriKind.Relative), content);

        var json = await response.Content.ReadAsStringAsync();
        return (response.StatusCode, json);
    }
    
    public static async Task<(HttpStatusCode StatusCode, TResponse Response)> DoPost<TPayload, TResponse>(this HttpClient client, string token, string endpoint, TPayload payload)
    {
        client.SetupAuthorizationHeader(token);
        
        using var content = JsonContent.Create(payload, typeof(TPayload));
        var responseMessage = await client.PostAsync(new Uri(endpoint, UriKind.Relative), content);

        var responseJson = await responseMessage.Content.ReadAsStringAsync();
        
        var response = JsonSerializer.Deserialize<TResponse>(responseJson);
        return (responseMessage.StatusCode, response);
    }

    public static async Task<(HttpStatusCode StatusCode, string Response)> DoPut<TContent>(this HttpClient client, string token, string endpoint, TContent payload)
    {
        client.SetupAuthorizationHeader(token);
        
        using var content = JsonContent.Create(payload, typeof(TContent));
        var response = await client.PutAsync(new Uri(endpoint, UriKind.Relative), content);

        var json = await response.Content.ReadAsStringAsync();
        return (response.StatusCode, json);
    }
    
    public static async Task<(HttpStatusCode StatusCode, string Response)> DoDelete(this HttpClient client, string token, string endpoint)
    {
        client.SetupAuthorizationHeader(token);
        
        var response = await client.DeleteAsync(new Uri(endpoint, UriKind.Relative));
        var content = await response.Content.ReadAsStringAsync();

        return (response.StatusCode, content);
    }

    private static void SetupAuthorizationHeader(this HttpClient client, string token)
    {
        if (string.IsNullOrEmpty(token)) return;

        client.DefaultRequestHeaders.Remove("Authorization");
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
    }
}