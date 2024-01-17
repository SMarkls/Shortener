using System.Net;
using LinkShortener.Application.Common.Exceptions.Common;

namespace LinkShortener.Application.Common.Exceptions;

public class AccessDeniedException : ApiException
{
    public override int Code => (int)HttpStatusCode.NotAcceptable;

    public AccessDeniedException() : base("У вас нет доступа к данному ресурсу.")
    {
    }
}