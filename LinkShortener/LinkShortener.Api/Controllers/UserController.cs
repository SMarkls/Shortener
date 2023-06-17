using System.Net;
using LinkShortener.Application.Common.Services;
using Microsoft.AspNetCore.Mvc;
using LinkShortener.Application.Models.Identity.Dtos;
using LinkShortener.Application.Models.Identity.ViewModels;
using Microsoft.Extensions.Primitives;

namespace LinkShortener.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController
{
    private readonly IAuthService authService;
    private readonly IIdentityTokenService tokenService;
    private readonly IHttpContextAccessor contextAccessor;

    public UserController(IAuthService authService, IIdentityTokenService tokenService, IHttpContextAccessor contextAccessor)
    {
        this.authService = authService;
        this.tokenService = tokenService;
        this.contextAccessor = contextAccessor;
    }
    
    [HttpPost]
    [Route("[action]")]
    public async Task<AuthVm> Register(RegisterDto dto)
    {
        var authVm = await authService.Register(dto);
        
        contextAccessor.HttpContext.Response.Cookies.Append("Authorization", authVm.AccessToken);
        contextAccessor.HttpContext.Response.Cookies.Append("RefreshToken", authVm.RefreshToken);

        return authVm;
    }
    
    [HttpPost]
    [Route("[action]")]
    public async Task<AuthVm> Login(LoginDto dto)
    {
        var authVm = await authService.Login(dto);
        
        contextAccessor.HttpContext.Response.Cookies.Append("Authorization", authVm.AccessToken);
        contextAccessor.HttpContext.Response.Cookies.Append("RefreshToken", authVm.RefreshToken);

        return authVm;
    }

    [HttpPost]
    [Route("[action]")]
    public async Task<AuthVm> RefreshToken(RefreshTokenDto dto)
    {
        var authVm = await tokenService.RefreshTokenAsync(dto);

        contextAccessor.HttpContext.Response.Cookies.Append("Authorization", authVm.AccessToken);
        contextAccessor.HttpContext.Response.Cookies.Append("RefreshToken", authVm.RefreshToken);

        return authVm;
    }

    [HttpGet]
    [Route("[action]")]
    public async Task<AuthVm> RefreshToken()
    {
        string refreshToken;
        if (!contextAccessor.HttpContext.Request.Cookies.TryGetValue("RefreshToken", out refreshToken))
        {
            contextAccessor.HttpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            await contextAccessor.HttpContext.Response.WriteAsync("Пользователь не авторизован.");
            return null;
        }

        string accessToken;
        if (!contextAccessor.HttpContext.Request.Cookies.TryGetValue("Authorization", out accessToken))
        {
            contextAccessor.HttpContext.Response.Redirect("/User/Login");
            return new AuthVm { AccessToken = "", RefreshToken = "" };
        }

        
        var authVm = await RefreshToken(new RefreshTokenDto { RefreshToken = refreshToken, AccessToken = accessToken });

        contextAccessor.HttpContext.Response.Cookies.Delete("Authorization");
        contextAccessor.HttpContext.Response.Cookies.Delete("RefreshToken");
        
        contextAccessor.HttpContext.Response.Cookies.Append("Authorization", authVm.AccessToken);
        contextAccessor.HttpContext.Response.Cookies.Append("RefreshToken", authVm.RefreshToken);

        return authVm;
    }
}