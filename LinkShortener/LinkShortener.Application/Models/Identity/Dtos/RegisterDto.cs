namespace LinkShortener.Application.Models.Identity.Dtos;

/// <summary>
/// Объект передачи данных регистрации пользователя.
/// </summary>
public class RegisterDto
{
    /// <summary>
    /// Ник.
    /// </summary>
    public string NickName { get; set; }
    
    /// <summary>
    /// Пароль.
    /// </summary>
    public string Password { get; set; }
    
    /// <summary>
    /// Подтверждение пароля.
    /// </summary>
    public string AccessPassword { get; set; }
}