namespace LinkShortener.Application.Common.Exceptions.Common;

public abstract class ApiException : Exception
{
    public abstract int Code { get; }

    protected ApiException(string message) : base(message)
    {
    }
}