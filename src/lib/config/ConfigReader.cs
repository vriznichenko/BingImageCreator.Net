using System.Text.Json;

namespace BingImageCreatorDotnet.Lib.Config;

public interface IConfigReader
{
    public Task<ConfigDto?> ReadAsync(string configPath);
}

public sealed class ConfigReader : IConfigReader
{
    public async Task<ConfigDto?> ReadAsync(string configPath)
    {
        using FileStream stream = File.OpenRead(configPath);
        return await JsonSerializer.DeserializeAsync<ConfigDto>(stream);
    }
}
