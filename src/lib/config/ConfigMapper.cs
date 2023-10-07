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
                    Prompt = configDto?.Input?.Prompt ?? string.Empty,
                    BaseUrl = configDto?.Input?.BaseUrl ?? string.Empty,
                    Regexp = ParseRegex(configDto?.Input?.Regexp ?? new()),
                    HeaderConfigs = ParseHeaders(configDto?.Input?.HeaderConfigs ?? Array.Empty<HeaderConfigDto>())
                        .ToList().AsReadOnly(),
                    CookieConfigs = ParseCookies(configDto?.Input?.CookieConfigs ?? Array.Empty<CookieConfigDto>())
                        .ToList().AsReadOnly(),
                },

                Output = new()
                {
                    OutputDir = configDto?.Output?.OutputDir ?? string.Empty,
                    TempDir = configDto?.Output?.TempDir ?? string.Empty,
                },
            }
        : new();

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
