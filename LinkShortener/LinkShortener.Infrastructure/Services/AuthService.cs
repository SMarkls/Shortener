using LinkShortener.Application.Common.Exceptions;
using LinkShortener.Application.Common.Services;
using LinkShortener.Application.Models.Identity.Dtos;
using LinkShortener.Application.Models.Identity.ViewModels;
using LinkShortener.Domain.Identity.Entities;
using Microsoft.AspNetCore.Identity;

namespace LinkShortener.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> userManager;
    private readonly IIdentityTokenService tokenService;

    public AuthService(UserManager<ApplicationUser> userManager, IIdentityTokenService tokenService)
    {
        this.userManager = userManager;
        this.tokenService = tokenService;
    }

    public async Task<AuthVm> Register(RegisterDto dto)
    {
        var result = await userManager.CreateAsync(new ApplicationUser { UserName = dto.NickName }, dto.Password);

        if (result.Succeeded)
            return await tokenService.GenerateTokenAsync(userManager.Users.First(x => x.UserName == dto.NickName));

        throw new Exception(string.Join("; ", result.Errors.Select(x => x.Description)));
    }

    public async Task<AuthVm> Login(LoginDto dto)
    {
        var user = await userManager.FindByNameAsync(dto.NickName);
        if (user is null)
            throw new NotFoundException(nameof(ApplicationUser), dto.NickName);

        var isValidPassword = await userManager.CheckPasswordAsync(user, dto.Password);

        if (isValidPassword)
            return await tokenService.GenerateTokenAsync(user);

        throw new InvalidLoginCredentialsException();
    }
}