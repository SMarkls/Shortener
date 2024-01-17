using System.Net;
using LinkShortener.Application.Common.Exceptions.Common;

namespace LinkShortener.Application.Common.Exceptions;

public class InvalidLoginCredentialsException : ApiException
{
    public override int Code => (int)HttpStatusCode.Forbidden;

    public InvalidLoginCredentialsException() : base($"Неверные данные для входа")
    {
    }
}