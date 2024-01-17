using LinkShortener.Application.Models.ShortenLinks.Dtos;

namespace LinkShortener.Application.Work.ShortenLinks.Interfaces;

public interface IShortenLinksCommands
{
    Task<string> CreateAsync(CreateShortenLinkDto dto, string userId);
    Task UpdateAsync(UpdateShortenLinkDto dto, string userId);
    Task DeleteAsync(long id, string userId);
}