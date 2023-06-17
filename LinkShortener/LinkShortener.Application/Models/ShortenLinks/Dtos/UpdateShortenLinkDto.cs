using LinkShortener.Application.Common.Mappings;
using LinkShortener.Domain.Entities;

namespace LinkShortener.Application.Models.ShortenLinks.Dtos;

public class UpdateShortenLinkDto : IMapTo<ShortenLink>
{
    /// <summary>
    /// Идентификатор сущности.
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// Полная ссылка.
    /// </summary>
    public string FullLink { get; set; }
}