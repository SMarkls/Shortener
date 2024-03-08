using LinkShortener.Domain.Entities;
using LinkShortener.Domain.Identity.Entities;
using Microsoft.EntityFrameworkCore;

namespace LinkShortener.Application.Common;

public interface IApplicationDbContext
{
    DbSet<ApplicationUser> Users { get; set; }
    DbSet<ShortenLink> Links { get; set; }
    Task<int> SaveChangesAsync();
}