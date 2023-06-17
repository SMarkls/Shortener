using System.Net;

namespace LinkShortener.Application.Common.Exceptions;

public class NotFoundException : Exception
{
    public static int Code => (int)HttpStatusCode.NotFound;
    
    public NotFoundException(string name, object key)
        : this($"Объект \"{name}\" ({key}) не был найден.")
    {
    }
    
    public NotFoundException(string message) : base(message + $" {Code}")
    {
    }
}