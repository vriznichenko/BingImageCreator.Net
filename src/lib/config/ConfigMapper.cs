using System.Text.Json;

namespace BingImageCreatorDotnet.Lib.Config;

public interface IConfigMapper
{
    public ConfigModel Map(ConfigDto? configDto);
}

public sealed class ConfigMapper : IConfigMapper
{
    public ConfigModel Map(ConfigDto? configDto)
        => configDto is not null
        ? new()
            {
                Input = new()
                {
                    Prompt = configDto?.Input?.Prompt ?? throw new JsonException("Missing required field 'prompt' in config."),
                    BaseUrl = configDto?.Input?.BaseUrl ?? "https://www.bing.com",
                    PollingMaxRetries = configDto?.Input?.PollingMaxRetries ?? 100,
                    Regexp = ParseRegex(configDto?.Input?.Regexp ?? new() { Pattern = "src=\"([^\"]+)\"" }),
                    HeaderConfigs = ParseHeaders(configDto?.Input?.HeaderConfigs ?? Array.Empty<HeaderConfigDto>())
                        .ToList().AsReadOnly(),
                    CookieConfigs = ParseCookies(configDto?.Input?.CookieConfigs ?? Array.Empty<CookieConfigDto>())
                        .ToList().AsReadOnly(),
                },

                Output = new()
                {
                    OutputDir = configDto?.Output?.OutputDir ?? Directory.GetCurrentDirectory(),
                    TempDir = configDto?.Output?.TempDir ?? Directory.GetCurrentDirectory(),
                },
            }
        : throw new JsonException("Empty config json file.");

    private static Regexp ParseRegex(RegexpDto regexDto)
        => new()
        {
            Pattern = regexDto.Pattern ?? string.Empty,
        };

    private static IEnumerable<HeaderConfig> ParseHeaders(IReadOnlyCollection<HeaderConfigDto?> headerConfigDtos)
        => headerConfigDtos
            .Select(x => new HeaderConfig
                {
                    Name = x?.Name ?? string.Empty,
                    Value = x?.Value ?? string.Empty,
                }) ?? Array.Empty<HeaderConfig>();

    private static IEnumerable<CookieConfig> ParseCookies(IReadOnlyCollection<CookieConfigDto?> cookieConfigDtos)
        => cookieConfigDtos
            .Select(x => new CookieConfig
                {
                    Name = x?.Name ?? string.Empty,
                    Value = x?.Value ?? string.Empty,
                }) ?? Array.Empty<CookieConfig>();
}
