
namespace StatisticService.Models;

public class Statistic
{
    /// <summary>
    /// Идентификатор сущности
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Идентификатор сокращенной ссылки
    /// </summary>
    public required string Token { get; init; }

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
}
