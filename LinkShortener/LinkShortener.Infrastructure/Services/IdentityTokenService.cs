using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using LinkShortener.Application.Common;
using LinkShortener.Application.Common.Exceptions;
using LinkShortener.Application.Common.Services;
using LinkShortener.Application.Models.Identity.Dtos;
using LinkShortener.Application.Models.Identity.ViewModels;
using LinkShortener.Domain.Indetity.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace LinkShortener.Infrastructure.Services;

public class IdentityTokenService : IIdentityTokenService
{
    private readonly IConfiguration configuration;
    private readonly TokenValidationParameters validationParameters;
    private readonly IApplicationDbContext context;
    private readonly UserManager<ApplicationUser> userManager;
    private readonly IHttpContextAccessor contextAccessor;
    private const int expirationMinutes = 30;

    public IdentityTokenService(IConfiguration configuration, TokenValidationParameters validationParameters,
        IApplicationDbContext context, UserManager<ApplicationUser> userManager, IHttpContextAccessor contextAccessor)
    {
        this.configuration = configuration;
        this.validationParameters = validationParameters;
        this.context = context;
        this.userManager = userManager;
        this.contextAccessor = contextAccessor;

        this.validationParameters.ValidateLifetime = false;
    }

    public async Task<AuthVm> GenerateTokenAsync(ApplicationUser user)
    {
        var expiration = DateTime.UtcNow.AddMinutes(expirationMinutes);
        var token = CreateJwtToken(
            CreateClaims(user),
            CreateSigningCredentials(),
            expiration
        );
        var tokenHandler = new JwtSecurityTokenHandler();
        var accessToken = tokenHandler.WriteToken(token);
        var refreshToken = new RefreshToken
        {
            Token = GenerateRefreshToken(),
            JwtId = token.Id,
            User = user,
            CreatedAt = DateTime.Now.ToUniversalTime(),
            Used = false,
            ExpirationTime = DateTime.Now.AddDays(10).ToUniversalTime()
        };

        var oldRefreshToken = await context.RefreshTokens.FirstOrDefaultAsync(x => x.UserId == user.Id);

        if (oldRefreshToken is not null)
        {
            context.RefreshTokens.Remove(oldRefreshToken);
            await context.SaveChangesAsync();
        }

        await context.RefreshTokens.AddAsync(refreshToken);
        await context.SaveChangesAsync();

        return new AuthVm { AccessToken = accessToken, RefreshToken = refreshToken.Token };
    }
    
    
    private JwtSecurityToken CreateJwtToken(List<Claim> claims, SigningCredentials credentials, DateTime expiration)
    {
        return new JwtSecurityToken(
            configuration["Jwt:Issuer"],
            configuration["Jwt:Audience"],
            claims,
            expires: expiration,
            signingCredentials: credentials
        );
    }
    
    private List<Claim> CreateClaims(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
            new Claim(JwtRegisteredClaimNames.NameId, user.Id),
            new Claim(JwtRegisteredClaimNames.Name, user.UserName)
        };
        return claims;
    }

    private SigningCredentials CreateSigningCredentials()
    {
        return new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"])), SecurityAlgorithms.HmacSha256);
    }

    public async Task<AuthVm> RefreshTokenAsync(RefreshTokenDto dto)
    {
        var claimsPrincipal = GetPrincipalFromToken(dto.AccessToken);
        
        if (claimsPrincipal is null)
            throw new OldRefreshTokenException();
        
        var claims = claimsPrincipal.Claims;

        var refreshToken = await context.RefreshTokens
            .Include(i => i.User)
            .FirstOrDefaultAsync(x => x.Token == dto.RefreshToken);

        if (refreshToken is null)
            throw new OldRefreshTokenException();

        var user = refreshToken.User;
        
        
        if (claims is null)
            throw new OldRefreshTokenException();

        var tokenId = claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)?.Value;
        if (tokenId is null)
            throw new OldRefreshTokenException();
        
        if (refreshToken.JwtId == tokenId)
            return await GenerateTokenAsync(user);

        throw new OldRefreshTokenException();
    }

    private ClaimsPrincipal GetPrincipalFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
            if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
                return null;
            return principal;
        }
        catch
        {
            return null;
        }
    }

    private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken) =>
        validatedToken is JwtSecurityToken jwtSecurityToken &&
        jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
            StringComparison.InvariantCultureIgnoreCase);

    private string GenerateRefreshToken()
    {
        var data = Guid.NewGuid().ToString();
        using var hashAlgorithm = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(data);
        return Convert.ToBase64String(hashAlgorithm.ComputeHash(bytes));
    }
    
    public bool IsExpired(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
        if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
            return true;
        if (DateTime.Now.Subtract(validatedToken.ValidFrom) > TimeSpan.FromMinutes(expirationMinutes))
            return true;
        return false;
    }
}