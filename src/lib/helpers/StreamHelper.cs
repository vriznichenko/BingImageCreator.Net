namespace BingImageCreatorDotnet.Lib.Helpers;

public static class StreamHelper
{
    public static byte[] StreamToByteArray(Stream stream)
    {
        using MemoryStream ms = new();
        stream.CopyTo(ms);
        return ms.ToArray();
    }
}
