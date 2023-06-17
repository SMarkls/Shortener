using System.Net;

namespace LinkShortener.Application.Common.Exceptions;

public class AccessDeniedException : Exception
{
    public static int Code => (int)HttpStatusCode.NotAcceptable;

    public AccessDeniedException() : base("У вас нет доступа к данному ресурсу.")
    {
    }
}