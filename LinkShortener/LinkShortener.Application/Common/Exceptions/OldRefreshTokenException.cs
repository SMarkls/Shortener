using System.Net;

namespace LinkShortener.Application.Common.Exceptions;

public class OldRefreshTokenException : Exception
{
    public static int Code => (int)HttpStatusCode.Forbidden;
    
    public OldRefreshTokenException()
        : base($"Авторизуйтесь заново. {Code}")
    {
    }
}