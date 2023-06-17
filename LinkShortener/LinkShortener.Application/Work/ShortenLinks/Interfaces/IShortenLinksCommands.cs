using LinkShortener.Application.Models.ShortenLinks.Dtos;

namespace LinkShortener.Application.Work.ShortenLinks.Interfaces;

public interface IShortenLinksCommands
{
    Task<long> CreateAsync(CreateShortenLinkDto dto);
    Task UpdateAsync(UpdateShortenLinkDto dto);
    Task DeleteAsync(long id);
}