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
using LinkShortener.Domain.Identity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace LinkShortener.Infrastructure.Services;

public class IdentityTokenService : IIdentityTokenService
{
    private readonly IConfiguration configuration;
    private readonly TokenValidationParameters validationParameters;
    private readonly IApplicationDbContext context;
    private const int expirationMinutes = 30;

    public IdentityTokenService(IConfiguration configuration, IApplicationDbContext context)
    {
        this.configuration = configuration;
        this.context = context;
        validationParameters = new TokenValidationParameters
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
    }

    public async Task<AuthVm> GenerateTokenAsync(ApplicationUser user)
    {
        var symmetricKey = validationParameters.IssuerSigningKey;
        var signingCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);
        var claims = CreateClaims(user);

        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expirationMinutes),
            signingCredentials: signingCredentials
        );

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
        }

        await context.RefreshTokens.AddAsync(refreshToken);
        await context.SaveChangesAsync();
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
            new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
            new Claim(this.configuration["Jwt:UserIdPropName"], user.Id)
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
        var claims = GetClaimsFromToken(dto.AccessToken);
        var refreshToken = await context.RefreshTokens.Include(i => i.User)
            .FirstOrDefaultAsync(x => x.Token == dto.RefreshToken);

        if (refreshToken is null)
        {
            throw new InvalidLoginCredentialsException();
        }

        var jwtId = claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti);
        if (jwtId is null)
        {
            throw new InvalidLoginCredentialsException();
        }

        if (jwtId.Value == refreshToken.JwtId)
        {
            return await GenerateTokenAsync(refreshToken.User);
        }

        throw new InvalidLoginCredentialsException();
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