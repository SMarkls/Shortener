using LinkShortener.Application.Common.Services;
using Microsoft.AspNetCore.Mvc;
using LinkShortener.Application.Models.Identity.Dtos;
using LinkShortener.Application.Models.Identity.ViewModels;

namespace LinkShortener.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class UserController
{
    private readonly IAuthService authService;
    private readonly IIdentityTokenService tokenService;

    public UserController(IAuthService authService, IIdentityTokenService tokenService)
    {
        this.authService = authService;
        this.tokenService = tokenService;
    }

    [HttpPost]
    public async Task<AuthVm> Register([FromBody] RegisterDto dto)
    {
        var authVm = await authService.Register(dto);
        return authVm;
    }

    [HttpPost]
    public async Task<AuthVm> Login([FromBody] LoginDto dto)
    {
        var authVm = await authService.Login(dto);
        return authVm;
    }

    [HttpPost]
    public async Task<AuthVm> RefreshToken([FromBody] RefreshTokenDto dto)
    {
        var authVm = await tokenService.RefreshTokenAsync(dto);
        return authVm;
    }
}