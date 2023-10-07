using System.Net;
using BingImageCreatorDotnet.Lib.Exceptions;

namespace BingImageCreatorDotnet.Lib.Web;

public interface IBingClient
{
    public Task<HttpResponseMessage> GetHttpResponse(
        string url,
        HttpMethod httpMethod,
        CancellationToken cancellationToken);

    public Task<HttpResponseMessage> GetHttpResponsePolling(
        string url,
        HttpMethod httpMethod,
        Func<string, bool> predicate,
        CancellationToken cancellationToken,
        int maxRetries);
}

public sealed class BingClient : IBingClient
{
    public async Task<HttpResponseMessage> GetHttpResponse(
        string url,
        HttpMethod httpMethod,
        CancellationToken cancellationToken)
    {
        HttpResponseMessage responseMessage = new();

        CookieContainer cookieContainer = new();
        using HttpClientHandler handler = new()
        {
            CookieContainer = cookieContainer,
            AllowAutoRedirect = false,
        };
        using HttpClient httpClient = new(handler)
        {
            BaseAddress = new(url),
        };
        httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
        httpClient.DefaultRequestHeaders.Add("X-Forwarded-For", "13.104.0.0/14");
        httpClient.DefaultRequestHeaders.Add("Origin", "https://www.bing.com");
        httpClient.DefaultRequestHeaders.Add("Referrer", "https://www.bing.com/images/create/");
        httpClient.DefaultRequestHeaders.Add("Cache-Control", "max-age=0");
        httpClient.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9");

        cookieContainer.Add(httpClient.BaseAddress, new Cookie("_U", "1-LA7EZsXt35SZU2zdrKzUIOV4ZI9KSsQot2ZGLcJ6iufwLL-iVmMjTdCEv_ojSHvbhUEU-csufcM3q1YGn0vx2Qw11ivosI1ueCfzKfpFqyBvGfVUzNygb4C8sh6bv7oVHJnM8k-kWYM4WJh6lph4H_xYf7p3H_-cRb3MK1HAD32xFY9KNbMbYiocHccxdrYHBDIlhc5Gadr1DiP-le9Q0bn3QUihiaIlSrBe5WpNccjjqiyCcdZom5oymgSMZcG"));
        cookieContainer.Add(httpClient.BaseAddress, new Cookie("SRCHHPGUSR", "NRSLT=50"));

        using HttpRequestMessage request = new() { Method = httpMethod };

        try
        {
            responseMessage = await httpClient.SendAsync(request, cancellationToken);
        }
        catch (TaskCanceledException taskCanceledException)
        {
            if(taskCanceledException.CancellationToken != cancellationToken)
            {
                throw new BingClientException("Request error.");
            }
        }

        return responseMessage;
    }

    public async Task<HttpResponseMessage> GetHttpResponsePolling(
        string url,
        HttpMethod httpMethod,
        Func<string, bool> predicate,
        CancellationToken cancellationToken,
        int maxRetries)
    {
        HttpResponseMessage responseBuffer;
        string responseString;
        int retries = 0;

        do {
            var response = await GetHttpResponse(url, httpMethod, cancellationToken);
            responseBuffer = response;
            responseString = await response.Content.ReadAsStringAsync();
            retries += 1;

            if (retries > maxRetries)
                throw new BingClientException("Too many retries on polling request.");

            Thread.Sleep(1000);
        } while (predicate(responseString));

        return responseBuffer;
    }
}
