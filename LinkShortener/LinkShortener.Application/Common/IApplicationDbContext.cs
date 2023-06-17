using LinkShortener.Domain.Entities;
using LinkShortener.Domain.Indetity.Entities;
using Microsoft.EntityFrameworkCore;

namespace LinkShortener.Application.Common;

public interface IApplicationDbContext
{
    // Identity
    DbSet<ApplicationUser> Users { get; set; }
    DbSet<RefreshToken> RefreshTokens { get; set; }
    
    // ShortenLinks
    DbSet<ShortenLink> Links { get; set; }
    
    Task<int> SaveChangesAsync();
}