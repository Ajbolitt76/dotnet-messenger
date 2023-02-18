using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;
using Messenger.Infrastructure.Services;

namespace Messenger.Core.Services;

public class JwtTokenGenerator : IJwtTokenGenerator
{
    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;
    private static readonly byte[] _key = "JWT_SECRET_KEY231321212321321321321321321weqdsadsa"u8.ToArray();

    private TokenValidationParameters _defaultValidationParameters = new()
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(_key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
    
    public JwtTokenGenerator()
    {
        _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        // JwtSettings = jwtSettings.Value;
    }


    public string GenerateFromClaims(IEnumerable<Claim> claims, DateTime expiresAt)
    {
        //TOOD Config + ECDSA
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims.ToArray()),
            Expires = expiresAt,
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = _jwtSecurityTokenHandler.CreateToken(tokenDescriptor);
        return _jwtSecurityTokenHandler.WriteToken(token);
    }

    public string GenerateToken<T>(T data, DateTime expiresAt)
    {
        //TOOD Config + ECDSA
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(typeof(T).Name, JsonSerializer.Serialize(data))
            }),
            Expires = expiresAt,
            SigningCredentials =
                new SigningCredentials(new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = _jwtSecurityTokenHandler.CreateToken(tokenDescriptor);
        return _jwtSecurityTokenHandler.WriteToken(token);
    }

    public T? ReadToken<T>(string token, bool shouldThrow = false, TokenValidationParameters? validationParameters = null)
    {
        try
        {
            var principal = ReadToken(token, validationParameters);
            var claim = principal.Claims.FirstOrDefault(c => c.Type == typeof(T).Name);
            return JsonSerializer.Deserialize<T>(claim.Value);    
        }catch (Exception e)
        {
            if (shouldThrow)
            {
                throw;
            }
            return default;
        }
    }
    
    public ClaimsPrincipal ReadToken(string token, TokenValidationParameters? validationParameters = null) 
        => _jwtSecurityTokenHandler.ValidateToken(
            token, 
            validationParameters ?? _defaultValidationParameters, 
            out _);

    public TokenValidationParameters CloneParameters()
        => _defaultValidationParameters.Clone();
}