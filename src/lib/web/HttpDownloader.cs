using BingImageCreatorDotnet.Lib.Exceptions;
using BingImageCreatorDotnet.Lib.Helpers;

namespace BingImageCreatorDotnet.Lib.Web;

public sealed class HttpDownloader
{
    internal async Task DownloadAndSaveAsync(IEnumerable<string> links, string dir, string extension)
    {
        var counter = 1;

        foreach(var link in links)
        {
            var stream = await DownloadResourceByLink(link);

            if (stream is not null)
            {
                var bytes = StreamHelper.StreamToByteArray(stream);
                await File.WriteAllBytesAsync(dir + "/" + counter.ToString() + "." + extension, bytes);
                counter += 1;
            }
        }
    }

    private async Task<Stream> DownloadResourceByLink(string link)
    {
        using HttpClient httpClient = new();
        httpClient.BaseAddress = new(link);
        httpClient.DefaultRequestHeaders.Accept.Clear();
        HttpResponseMessage response = await httpClient.GetAsync(link);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStreamAsync();
            return content;
        }

        throw new HttpDownloaderException("Empty download content.");
    }
}
