using System.Net;

namespace LinkShortener.Application.Common.Exceptions;

public class InvalidLoginCredentialsException : Exception
{
    public static int Code => (int)HttpStatusCode.Forbidden;

    public InvalidLoginCredentialsException() : base($"Неверные данные для входа. {Code}")
    {
    }
}