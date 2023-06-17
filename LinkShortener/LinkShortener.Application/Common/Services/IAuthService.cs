using LinkShortener.Application.Models.Identity.Dtos;
using LinkShortener.Application.Models.Identity.ViewModels;

namespace LinkShortener.Application.Common.Services;

public interface IAuthService
{
    Task<AuthVm> Register(RegisterDto dto);
    Task<AuthVm> Login(LoginDto dto);
}