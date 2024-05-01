using System;
using System.Text.Json.Serialization;

namespace TechStore.Tests.Authentication.Models;

public class TestAuthenticationResult
{
    [JsonPropertyName("username")]
    public string Username { get; set; }

    [JsonPropertyName("token")]
    public string Token { get; set; }

    [JsonPropertyName("expiresUtc")]
    public DateTime ExpiresUtc { get; set; }
}