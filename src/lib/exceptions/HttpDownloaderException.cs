using System.Runtime.Serialization;

namespace BingImageCreatorDotnet.Lib.Exceptions;

public class HttpDownloaderException : SerializationException
{
    public HttpDownloaderException() {  }
    public HttpDownloaderException(string message) : base(message) { }
}
