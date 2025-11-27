using Clinical6SDK.Helpers;
using System;
using System.Net.Http;
using Device = Clinical6SDK.Helpers.Device;

public sealed class ClientSingleton
{
    public string AuthToken { get; set; }
    public string BaseUrl { get; set; }
    public string VerificationCodeUrl { get; set; }
    public User User { get; set; }
    public Device Device { get; set; }

    public HttpClient HttpClient { get; set; }

    private HttpMessageHandler _httpMessageHandler;

    public HttpMessageHandler HttpMessageHandler
    {
        get => _httpMessageHandler;
        set
        {
            _httpMessageHandler?.Dispose();
            _httpMessageHandler = value;

            HttpClient?.Dispose();
            HttpClient = new HttpClient(_httpMessageHandler);
            HttpClient.Timeout = TimeSpan.FromMinutes(2);
        }
    }

    private ClientSingleton()
    {
        HttpMessageHandler = new HttpClientHandler();
    }

    public static ClientSingleton Instance { get; } = new ClientSingleton();

    public void Clear()
    {
        AuthToken = null;
        User = null;
    }
}