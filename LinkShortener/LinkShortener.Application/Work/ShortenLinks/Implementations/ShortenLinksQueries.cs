using AutoMapper;
using AutoMapper.QueryableExtensions;
using LinkShortener.Application.Common;
using LinkShortener.Application.Common.Exceptions;
using LinkShortener.Application.Models.ShortenLinks.ViewModels;
using LinkShortener.Application.Work.ShortenLinks.Interfaces;
using LinkShortener.Domain.Entities;
using LinkShortener.Domain.Indetity.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace LinkShortener.Application.Work.ShortenLinks.Implementations;

public class ShortenLinksQueries : IShortenLinksQueries
{
    private readonly IApplicationDbContext context;
    private readonly IMapper mapper;
    private readonly IHttpContextAccessor contextAccessor;

    public ShortenLinksQueries(IApplicationDbContext context, IMapper mapper, IHttpContextAccessor contextAccessor)
    {
        this.context = context;
        this.mapper = mapper;
        this.contextAccessor = contextAccessor;
    }
    
    public async Task<ShortenLinkVm> GetAsync(long id)
    {
        var owner = await GetOwnerByHttpContextAsync();
        
        var entity = await context.Links.FirstOrDefaultAsync(x => x.Id == id);

        if (entity is null)
            throw new NotFoundException(nameof(ShortenLink), id);

        if (owner != entity.Owner)
            throw new AccessDeniedException();

        return mapper.Map<ShortenLinkVm>(entity);
    }

    public async Task<List<ShortenLinkVm>> GetList()
    {
        var owner = await GetOwnerByHttpContextAsync();
        
        var list = await context.Links
            .Include(i => i.Owner)
            .Where(x => x.Owner == owner)
            .ProjectTo<ShortenLinkVm>(mapper.ConfigurationProvider)
            .ToListAsync();

        return list;
    }
    private async Task<ApplicationUser> GetOwnerByHttpContextAsync()
    {
        var claims = contextAccessor.HttpContext.User.Identities.First().Claims;
        var ownerIdClaim = claims.Skip(3).First();
        var ownerId = ownerIdClaim.Value;
        var owner = await context.Users.FirstAsync(x => x.Id == ownerId);

        return owner;
    }

    public async Task<string> GetFullLink(string token)
    {
        var link = await context.Links.FirstOrDefaultAsync(x => x.Token == token);

        if (link is null)
            throw new NotFoundException(nameof(ShortenLink), token);

        link.CountOfRedirections++;
        await context.SaveChangesAsync();
        
        return link.FullLink;
    }
}