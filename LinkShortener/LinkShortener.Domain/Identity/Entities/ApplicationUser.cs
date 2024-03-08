using LinkShortener.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace LinkShortener.Domain.Identity.Entities;

public class ApplicationUser : IdentityUser
{
    /// <summary>
    /// Навигационное свойство - сокращенные ссылки.
    /// </summary>
    public List<ShortenLink> ShortenLinks { get; }
}