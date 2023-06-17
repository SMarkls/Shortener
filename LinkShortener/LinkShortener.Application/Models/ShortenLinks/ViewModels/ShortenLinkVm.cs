using LinkShortener.Application.Common.Mappings;
using LinkShortener.Domain.Entities;

namespace LinkShortener.Application.Models.ShortenLinks.ViewModels;

/// <summary>
/// Модель представления запроса получения сущности "Сокращенная ссылка".
/// </summary>
public class ShortenLinkVm : IMapFrom<ShortenLink>
{
    /// <summary>
    /// Идентификатор ссылки.
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// Короткий токен ссылки.
    /// </summary>
    public string Token { get; set; }
    
    /// <summary>
    /// Полная ссылка.
    /// </summary>
    public string FullLink { get; set; }
    
    /// <summary>
    /// Количество переходов по ссылке.
    /// </summary>
    public int CountOfRedirections { get; set; }
}