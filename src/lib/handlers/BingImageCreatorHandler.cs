using System.Text.Json;
using System.Text.RegularExpressions;
using System.Web;
using BingImageCreatorDotnet.Lib.Config;
using BingImageCreatorDotnet.Lib.Exceptions;
using BingImageCreatorDotnet.Lib.Web;
using Microsoft.Extensions.Logging;

namespace BingImageCreatorDotnet.Lib.Handlers;

public sealed class BingImageCreatorHandler
{
    private readonly ILogger<BingImageCreatorHandler> logger;
    private readonly IConfigReader configReader;
    private readonly IConfigMapper configMapper;

    public BingImageCreatorHandler(
        ILogger<BingImageCreatorHandler> logger,
        IConfigReader configReader,
        IConfigMapper configMapper)
    {
        this.logger = logger;
        this.configReader = configReader;
        this.configMapper = configMapper;
    }

    public async Task RunTask()
    {
        var configDto = await configReader.ReadAsync("D:\\Projects\\BingImageCreator.Net\\config.json");
        var config = configMapper.Map(configDto);
        var downloadDirectoryName = $"{config.Output.OutputDir}/{config.Input.Prompt}_{DateTime.UtcNow:yyyy_MM_dd_HH_mm_ss}";
        Directory.CreateDirectory(downloadDirectoryName);

        try
        {
            BingClient bingClient = new();

            logger.LogInformation("Sending redirect request...");
            var redirectResponse = await bingClient.GetHttpResponse(
                config.Input.BaseUrl + "/images/create?q=" + HttpUtility.UrlEncode(config.Input.Prompt) + "&rt=4&FORM=GENCRE",
                HttpMethod.Post,
                new CancellationToken());
            logger.LogInformation($"Redirect response recieved. Status code: {redirectResponse.StatusCode}");

            var redirectUrl = redirectResponse.Headers.Location?.OriginalString.Replace("&nfy=1", "");
            var requestId = redirectUrl?.Split("id=").LastOrDefault();

            logger.LogInformation("Sending polling request...");
            logger.LogInformation($"Waiting for polling request response...");
            var pollingResponse = await bingClient.GetHttpResponsePolling(
                $"{config.Input.BaseUrl}/images/create/async/results/{requestId}?q={HttpUtility.UrlEncode(config.Input.Prompt)}",
                HttpMethod.Get,
                response => string.IsNullOrEmpty(response) is true || response.Contains("Pending"),
                new CancellationToken(),
                config.Input.PollingMaxRetries);
            logger.LogInformation($"Polling response recieved. Status Code: {pollingResponse.StatusCode}");

            var pollingResponseContent = await pollingResponse.Content.ReadAsStringAsync();

            Directory.CreateDirectory(config.Output.TempDir);
            File.WriteAllText($"{config.Output.TempDir}/temp.html", pollingResponseContent);

            var fileContent = File.ReadAllText($"{config.Output.TempDir}/temp.html");
            var regex = new Regex("src=\"([^\"]+)\"");
            var linksMatches = regex.Matches(fileContent);
            var links = linksMatches
                .OfType<Match>()
                .Select(m => m.Value.Split("=\"").Last().Split("?w").First())
                .ToArray();

            HttpDownloader httpDownloader = new();
            logger.LogInformation("Start files download...");

            await httpDownloader.DownloadAndSaveAsync(links, downloadDirectoryName, "jpg");
            logger.LogInformation($"Files downloaded. Output directory: {downloadDirectoryName}");
            File.Delete($"{config.Output.TempDir}/temp.html");
        }
        catch (JsonException jsonException)
        {
            HandleException(jsonException.ToString(), downloadDirectoryName);
        }
        catch (BingClientException bingClientException)
        {
            HandleException(bingClientException.ToString(), downloadDirectoryName);
        }
        catch (HttpDownloaderException httpDownloaderException)
        {
            HandleException(httpDownloaderException.ToString(), downloadDirectoryName);
        }
        catch (Exception exception)
        {
            HandleException(exception.ToString(), downloadDirectoryName);
        }
    }

    private async void HandleException(string exceptionTrace, string exceptionSavePath)
    {
        logger.LogError(exceptionTrace);
        using StreamWriter outputFile = new(Path.Combine(exceptionSavePath, "exception.log"));
        await outputFile.WriteAsync(exceptionTrace);
    }
}
