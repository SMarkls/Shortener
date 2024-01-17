namespace LinkShortener.Application.Models.Identity.Dtos;

public class RefreshTokenDto
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}