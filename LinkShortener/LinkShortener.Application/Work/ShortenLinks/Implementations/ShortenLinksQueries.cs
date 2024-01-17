using AutoMapper;
using AutoMapper.QueryableExtensions;
using LinkShortener.Application.Common;
using LinkShortener.Application.Common.Exceptions;
using LinkShortener.Application.Models.ShortenLinks.ViewModels;
using LinkShortener.Application.Work.ShortenLinks.Interfaces;
using LinkShortener.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LinkShortener.Application.Work.ShortenLinks.Implementations;

public class ShortenLinksQueries : IShortenLinksQueries
{
    private readonly IApplicationDbContext context;
    private readonly IMapper mapper;

    public ShortenLinksQueries(IApplicationDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<ShortenLinkVm> GetAsync(long id, string userId)
    {
        var entity = await context.Links.Include(i => i.Owner).FirstOrDefaultAsync(x => x.Id == id);

        if (entity is null)
            throw new NotFoundException(nameof(ShortenLink), id);

        if (entity.Owner == null || userId != entity.Owner.Id)
            throw new AccessDeniedException();

        return mapper.Map<ShortenLinkVm>(entity);
    }

    public async Task<List<ShortenLinkVm>> GetList(string userId)
    {
        var list = await context.Links
            .Include(i => i.Owner)
            .Where(x => x.Owner != null && x.Owner.Id == userId)
            .ProjectTo<ShortenLinkVm>(mapper.ConfigurationProvider)
            .ToListAsync();

        return list;
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