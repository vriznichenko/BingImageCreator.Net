namespace BingImageCreatorDotnet.Lib.Exceptions;

public class BingClientException : TaskCanceledException
{
    public BingClientException() {  }
    public BingClientException(string message) : base(message) { }
}
