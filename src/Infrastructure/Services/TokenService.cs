using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Contracts.Services;
using Microsoft.AspNetCore.Identity;

namespace Team.Infrastructure.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _config;
    private readonly SymmetricSecurityKey _key;

    public TokenService(IConfiguration config)
    {
        _config = config;
        _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Token:Key"]));
    }

    public string CreateToken(IdentityUser user)
    {
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Email, user.Email)
        };

        var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(7),
            SigningCredentials = credentials,
            Issuer = _config["Token:Issuer"]
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
