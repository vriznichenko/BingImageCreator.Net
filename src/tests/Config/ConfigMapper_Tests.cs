using System.Text.Json;
using System.Text.RegularExpressions;
using BingImageCreatorDotnet.Lib.Config;

namespace BingImageCreatorDotnet.Tests.Config;

[TestClass]
public class ConfigMapper_Tests
{
    [TestMethod]
    public void Map_Null_Thtows()
    {
        ConfigMapper mapper = new();

        var actual = Assert.ThrowsException<JsonException>(() => mapper.Map(null));
        Assert.AreEqual("Empty config json file.", actual.Message);
    }

    [TestMethod]
    public void Map_Empty_Throws()
    {
        ConfigDto dto = new();
        ConfigMapper mapper = new();

        var actual = Assert.ThrowsException<JsonException>(() => mapper.Map(dto));
        Assert.AreEqual("Missing required field 'prompt' in config.", actual.Message);
    }

    [TestMethod]
    public void Map_PromptOnly_Ok()
    {
        ConfigDto dto = new()
        {
            Input = new()
            {
                Prompt = "sample_string",
            },
        };
        ConfigMapper mapper = new();
        ConfigModel expected = new()
        {
            Input = new()
            {
                Prompt = "sample_string",
                BaseUrl = "https://www.bing.com",
                PollingMaxRetries = 100,
                Regexp = new Regexp() { Pattern = "src=\"([^\"]+)\"" },
                HeaderConfigs = Array.Empty<HeaderConfig>(),
                CookieConfigs = Array.Empty<CookieConfig>(),
            },
            Output = new()
            {
                OutputDir = Directory.GetCurrentDirectory(),
                TempDir = Directory.GetCurrentDirectory(),
            },
        };

        var actual = mapper.Map(dto);
        Assert.AreEqual(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(actual));
    }

    [TestMethod]
    public void Map_FullCorrectInput_Ok()
    {
        ConfigDto dto = new()
        {
            Input = new()
            {
                Prompt = "sample_string",
                BaseUrl = "https://www.bing.com",
                PollingMaxRetries = 120,
                Regexp = new RegexpDto() { Pattern = "sample_string" },
                HeaderConfigs = new HeaderConfigDto[]
                {
                    new()
                    {
                        Name = "sample_string",
                        Value = "sample_string",
                    },
                },
                CookieConfigs = new CookieConfigDto[]
                {
                    new()
                    {
                        Name = "sample_string",
                        Value = "sample_string",
                    },
                },
            },
            Output = new()
            {
                OutputDir = "sample_string",
                TempDir = "sample_string",
            },
        };
        ConfigMapper mapper = new();
        ConfigModel expected = new()
        {
            Input = new()
            {
                Prompt = "sample_string",
                BaseUrl = "https://www.bing.com",
                PollingMaxRetries = 120,
                Regexp = new Regexp() { Pattern = "sample_string" },
                HeaderConfigs = new HeaderConfig[]
                {
                    new()
                    {
                        Name = "sample_string",
                        Value = "sample_string",
                    },
                },
                CookieConfigs = new CookieConfig[]
                {
                    new()
                    {
                        Name = "sample_string",
                        Value = "sample_string",
                    },
                },
            },
            Output = new()
            {
                OutputDir = "sample_string",
                TempDir = "sample_string",
            },
        };

        var actual = mapper.Map(dto);
        Assert.AreEqual(JsonSerializer.Serialize(expected), JsonSerializer.Serialize(actual));
    }
}
