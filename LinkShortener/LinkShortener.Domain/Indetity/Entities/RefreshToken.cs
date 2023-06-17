using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinkShortener.Domain.Indetity.Entities;

public class RefreshToken
{
    [Key]
    public string Token { get; set; }
    
    public string JwtId { get; set; }
    
    public DateTime CreatedAt { get; set; }
    
    public DateTime ExpirationTime { get; set; }
    
    public bool Used { get; set; }
    
    public string UserId { get; set; }
    
    [ForeignKey(nameof(UserId))]
    public ApplicationUser User { get; set; }
}