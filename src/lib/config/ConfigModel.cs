namespace BingImageCreatorDotnet.Lib.Config;

public sealed record ConfigModel
{
    public Input Input { get; init; } = new();
    public Output Output { get; init; } = new();
}

public sealed record Input
{
    public string Prompt { get; init; } = string.Empty;
    public string BaseUrl { get; init; } = string.Empty;
    public Regexp Regexp { get; init; } = new();
    public IReadOnlyCollection<HeaderConfig> HeaderConfigs { get; init; }
        = Array.Empty<HeaderConfig>();
    public IReadOnlyCollection<CookieConfig> CookieConfigs { get; init; }
        = Array.Empty<CookieConfig>();
}

public sealed record Output
{
    public string OutputDir { get; init; } = string.Empty;
    public string TempDir { get; init; } = string.Empty;
}

public sealed record Regexp
{
    public string Pattern { get; init; } = string.Empty;
}

public sealed record HeaderConfig
{
    public string Name { get; init; } = string.Empty;
    public string Value { get; init; } = string.Empty;
}

public sealed record CookieConfig
{
    public string Name { get; init; } = string.Empty;
    public string Value { get; init; } = string.Empty;
}
