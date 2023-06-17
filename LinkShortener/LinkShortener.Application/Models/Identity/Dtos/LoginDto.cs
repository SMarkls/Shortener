namespace LinkShortener.Application.Models.Identity.Dtos;

public class LoginDto
{
    /// <summary>
    /// Имя пользователя.
    /// </summary>
    public string NickName { get; set; }
    
    /// <summary>
    /// Пароль пользователя.
    /// </summary>
    public string Password { get; set; }
}