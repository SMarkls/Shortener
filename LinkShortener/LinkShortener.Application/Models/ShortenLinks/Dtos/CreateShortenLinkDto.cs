namespace LinkShortener.Application.Models.ShortenLinks.Dtos;

/// <summary>
/// Объект передачи данных команды создания сокращенной ссылки.
/// </summary>
public class CreateShortenLinkDto
{
    /// <summary>
    /// Полная ссылка.
    /// </summary>
    public string FullLink { get; set; }
}