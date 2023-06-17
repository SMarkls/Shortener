namespace LinkShortener.Application.Models.Identity.ViewModels;

/// <summary>
/// ViewModel с данными о пользователе и токенами
/// </summary>
public class AuthVm
{
    /// <summary>
    /// Токен доступа
    /// </summary>
    public string AccessToken { get; set; }

    /// <summary>
    /// Токен обновления.
    /// </summary>
    public string RefreshToken { get; set; }
}