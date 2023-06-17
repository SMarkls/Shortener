using LinkShortener.Application.Models.Identity.Dtos;
using LinkShortener.Application.Models.Identity.ViewModels;
using LinkShortener.Domain.Indetity.Entities;

namespace LinkShortener.Application.Common.Services;

public interface IIdentityTokenService
{
    Task<AuthVm> GenerateTokenAsync(ApplicationUser user);
    Task<AuthVm> RefreshTokenAsync(RefreshTokenDto dto);

    bool IsExpired(string token);
}