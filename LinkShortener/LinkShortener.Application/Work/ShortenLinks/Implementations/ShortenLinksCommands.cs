using System.Text;
using AutoMapper;
using LinkShortener.Application.Common;
using LinkShortener.Application.Common.Exceptions;
using LinkShortener.Application.Models.ShortenLinks.Dtos;
using LinkShortener.Application.Work.ShortenLinks.Interfaces;
using LinkShortener.Domain.Entities;
using LinkShortener.Domain.Indetity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace LinkShortener.Application.Work.ShortenLinks.Implementations;

public class ShortenLinksCommands : IShortenLinksCommands
{
    private readonly IApplicationDbContext context;
    private readonly IMapper mapper;
    private readonly IHttpContextAccessor contextAccessor;

    public ShortenLinksCommands(IApplicationDbContext context, IMapper mapper, IHttpContextAccessor contextAccessor)
    {
        this.context = context;
        this.mapper = mapper;
        this.contextAccessor = contextAccessor;
    }
    
    public async Task<long> CreateAsync(CreateShortenLinkDto dto)
    {
        var owner = await GetOwnerByHttpContextAsync();
        
        string token;
        do
        {
            token = Convert.ToBase64String(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString())).Remove(6).ToLower();
        } while (context.Links.Any(x => x.Token == token));
        
        var entity = new ShortenLink { Token = token, FullLink = dto.FullLink, Owner = owner, CountOfRedirections = 0 };
        await context.Links.AddAsync(entity);
        await context.SaveChangesAsync();
        
        return entity.Id;
    }
    
    public async Task UpdateAsync(UpdateShortenLinkDto dto)
    {
        var owner = await GetOwnerByHttpContextAsync();
        
        var entity = await context.Links
            .Include(i => i.Owner)
            .FirstOrDefaultAsync(x => x.Id == dto.Id);
        
        if (entity is null)
            throw new NotFoundException(nameof(ShortenLink), dto.Id);

        if (owner != entity.Owner)
            throw new AccessDeniedException();

        entity.FullLink = dto.FullLink;
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(long id)
    {
        var owner = await GetOwnerByHttpContextAsync();

        var entity = await context.Links
            .Include(i => i.Owner)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (entity is null)
            throw new NotFoundException(nameof(ShortenLink), id);

        if (owner != entity.Owner)
            throw new AccessDeniedException();

        context.Links.Remove(entity);
        await context.SaveChangesAsync();
    }
    
    private async Task<ApplicationUser> GetOwnerByHttpContextAsync()
    {
        var claims = contextAccessor.HttpContext.User.Identities.First().Claims;
        var ownerIdClaim = claims.Skip(3).First();
        var ownerId = ownerIdClaim.Value;
        var owner = await context.Users.FirstAsync(x => x.Id == ownerId);

        return owner;
    }
}