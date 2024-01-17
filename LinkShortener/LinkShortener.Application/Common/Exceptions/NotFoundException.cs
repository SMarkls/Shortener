using System.Net;
using LinkShortener.Application.Common.Exceptions.Common;

namespace LinkShortener.Application.Common.Exceptions;

public class NotFoundException : ApiException
{
    public override int Code => (int)HttpStatusCode.NotFound;

    public NotFoundException(string name, object key)
        : this($"Объект \"{name}\" ({key}) не был найден.")
    {
    }

    public NotFoundException(string message) : base(message)
    {
    }
}