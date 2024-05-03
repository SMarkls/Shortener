using LinkShortener.Domain.Common;
using LinkShortener.Domain.Identity.Entities;

namespace LinkShortener.Domain.Entities;

/// <summary>
/// Сущность "Сокращенная ссылка".
/// </summary>
public class ShortenLink : BaseEntity
{
    /// <summary>
    /// Короткий токен ссылки.
    /// </summary>
    public required string Token { get; init; }

    /// <summary>
    /// Полная ссылка.
    /// </summary>
    public required string FullLink { get; set; }

    /// <summary>
    /// Количество переходов по ссылке.
    /// </summary>
    public required int CountOfRedirections { get; set; }

    /// <summary>
    /// Наивагационное свойство - создатель ссылки.
    /// </summary>
    public ApplicationUser? Owner { get; init; }

    /// <summary>
    /// Навигационное свойство - статистики переходв по ссылке.
    /// </summary>
    public IList<Statistic> Statistics { get; init; } = new List<Statistic>();
}