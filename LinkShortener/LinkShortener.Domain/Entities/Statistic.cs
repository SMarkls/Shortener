namespace LinkShortener.Domain.Entities;

public class Statistic
{
    /// <summary>
    /// Браузер, с которого осуществлен переход
    /// </summary>
    public required string? Browser { get; init; }

    /// <summary>
    /// Ip адрес, откуда осуществлен переход
    /// </summary>
    public required string IpAddress { get; init; }

    /// <summary>
    /// Время перехода
    /// </summary>
    public required DateTime Time { get; init; }

    /// <summary>
    /// Навигационное свойство - сокращенная ссылка, по которой был совершен переход.
    /// </summary>
    public required string Token { get; init; }
}