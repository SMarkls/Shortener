using System.ComponentModel.DataAnnotations;

namespace LinkShortener.Domain.Identity.Entities;

public class RefreshToken
{
    [Key]
    public required string Token { get; init; }
    public required string JwtId { get; init; }
}