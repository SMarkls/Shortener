using System.ComponentModel.DataAnnotations;

namespace LinkShortener.Domain.Identity.Entities;

public class RefreshToken
{
    [Key]
    public string Token { get; set; }

    public string JwtId { get; set; }
}