using System.Text;
using AutoMapper;
using LinkShortener.Application.Common;
using LinkShortener.Application.Common.Exceptions;
using LinkShortener.Application.Models.ShortenLinks.Dtos;
using LinkShortener.Application.Work.ShortenLinks.Interfaces;
using LinkShortener.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LinkShortener.Application.Work.ShortenLinks.Implementations;

public class ShortenLinksCommands : IShortenLinksCommands
{
    private readonly IApplicationDbContext context;
    private readonly IMapper mapper;

    public ShortenLinksCommands(IApplicationDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<string> CreateAsync(CreateShortenLinkDto dto, string userId)
    {
        var owner = await context.Users.FirstOrDefaultAsync(x => x.Id == userId);

        string token;
        do
        {
            token = Convert.ToBase64String(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString())).Remove(6).ToLower();
        } while (context.Links.Any(x => x.Token == token));

        var entity = new ShortenLink { Token = token, FullLink = dto.FullLink, Owner = owner, CountOfRedirections = 0 };
        await context.Links.AddAsync(entity);
        await context.SaveChangesAsync();

        return entity.Token;
    }

    public async Task UpdateAsync(UpdateShortenLinkDto dto, string userId)
    {
        var entity = await context.Links
            .Include(i => i.Owner)
            .FirstOrDefaultAsync(x => x.Id == dto.Id);

        if (entity is null)
            throw new NotFoundException(nameof(ShortenLink), dto.Id);

        if (userId != entity.Owner?.Id)
            throw new AccessDeniedException();

        entity.FullLink = dto.FullLink;
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(long id, string userId)
    {
        var entity = await context.Links
            .Include(i => i.Owner)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (entity is null)
            throw new NotFoundException(nameof(ShortenLink), id);

        if (userId != entity.Owner?.Id)
            throw new AccessDeniedException();

        context.Links.Remove(entity);
        await context.SaveChangesAsync();
    }
}