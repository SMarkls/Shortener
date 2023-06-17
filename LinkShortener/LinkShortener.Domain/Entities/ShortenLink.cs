using LinkShortener.Domain.Common;
using LinkShortener.Domain.Indetity.Entities;

namespace LinkShortener.Domain.Entities;

/// <summary>
/// Сущность "Сокращенная ссылка".
/// </summary>
public class ShortenLink : BaseEntity
{
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
    
    /// <summary>
    /// Наивагационное свойство - создатель ссылки.
    /// </summary>
    public ApplicationUser Owner { get; set; }
}