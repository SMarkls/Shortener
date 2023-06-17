using LinkShortener.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace LinkShortener.Domain.Indetity.Entities;

public class ApplicationUser : IdentityUser
{
    /// <summary>
    /// Навигационное свойство - сокращенные ссылки.
    /// </summary>
    public List<ShortenLink> ShortenLinks { get; }
    
    public RefreshToken RefreshToken { get; set; }
}