using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using LinkShortener.Application.Common;
using LinkShortener.Application.Common.Exceptions;
using LinkShortener.Application.Common.Services;
using LinkShortener.Application.Models.Identity.Dtos;
using LinkShortener.Application.Models.Identity.ViewModels;
using LinkShortener.Domain.Identity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace LinkShortener.Infrastructure.Services;

public class IdentityTokenService : IIdentityTokenService
{
    private static TokenValidationParameters? validationParameters;
    private static string? propName;
    private static string? issuer;
    private static string? audience;
    private static int? expirationMinutes;

    private readonly IApplicationDbContext context;
    private readonly IDistributedCache cache;

    public IdentityTokenService(IConfiguration configuration, IApplicationDbContext context, IDistributedCache cache)
    {
        this.context = context;
        this.cache = cache;
        validationParameters ??= new TokenValidationParameters
        {
            ClockSkew = TimeSpan.Zero,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = false,
            ValidateIssuerSigningKey = true,
            ValidIssuer = configuration["Jwt:Issuer"],
            ValidAudience = configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
        };

        propName ??= configuration["Jwt:UserIdPropName"];
        issuer ??= configuration["Jwt:Issuer"];
        audience ??= configuration["Jwt:Audience"];
        expirationMinutes ??= int.Parse(configuration["Jwt:MinutesLifeTime"]);
    }

    public async Task<AuthVm> GenerateTokenAsync(ApplicationUser user)
    {
        var symmetricKey = validationParameters!.IssuerSigningKey;
        var signingCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);
        var claims = CreateClaims(user);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expirationMinutes!.Value),
            signingCredentials: signingCredentials
        );

        var refreshToken = new RefreshToken
        {
            Token = GenerateRefreshToken(),
            JwtId = token.Id,
        };

        await cache.SetStringAsync(user.Id, JsonSerializer.Serialize(refreshToken), new DistributedCacheEntryOptions { AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(10) });
        return new AuthVm
        {
            AccessToken = new JwtSecurityTokenHandler().WriteToken(token),
            RefreshToken = refreshToken.Token
        };
    }

    private IEnumerable<Claim> CreateClaims(ApplicationUser user)
    {
        var claims = new List<Claim>
        {
            new (JwtRegisteredClaimNames.Sub, user.UserName),
            new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new (JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
            new (propName!, user.Id)
        };
        return claims;
    }

    private static string GenerateRefreshToken()
    {
        var data = Guid.NewGuid().ToString();
        var bytes = Encoding.UTF8.GetBytes(data);
        return Convert.ToBase64String(SHA256.HashData(bytes));
    }

    public async Task<AuthVm> RefreshTokenAsync(RefreshTokenDto dto)
    {
        var claims = GetClaimsFromToken(dto.AccessToken).ToList();
        var jwtId = claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti);
        if (jwtId is null)
        {
            throw new InvalidLoginCredentialsException();
        }

        var userId = claims.FirstOrDefault(x => x.Type == propName)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            throw new InvalidLoginCredentialsException();
        }

        var user = await context.Users.FirstOrDefaultAsync(x => x.Id == userId);
        if (user is null)
        {
            throw new InvalidLoginCredentialsException();
        }

        var refreshJson = await cache.GetStringAsync(userId);
        if (string.IsNullOrEmpty(refreshJson))
        {
            throw new InvalidLoginCredentialsException();
        }

        var refreshToken = JsonSerializer.Deserialize<RefreshToken>(refreshJson);
        if (refreshToken == default)
        {
            throw new InvalidLoginCredentialsException();
        }

        if (jwtId.Value != refreshToken.JwtId)
        {
            throw new InvalidLoginCredentialsException();
        }

        return await GenerateTokenAsync(user);
    }

    private IEnumerable<Claim> GetClaimsFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        try
        {
            tokenHandler.ValidateToken(token.Replace("Bearer ", ""), validationParameters, out var validatedToken);
            if (validatedToken is JwtSecurityToken jwtToken &&
                jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                return jwtToken.Claims;
            }

            return Enumerable.Empty<Claim>();
        }
        catch
        {
            return Enumerable.Empty<Claim>();
        }
    }
}