using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Requiem.PetProject.Interfaces;
using Requiem.PetProject.Models;

namespace Requiem.PetProject.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly SymmetricSecurityKey _securityKey;
    
    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
        _securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SigningKey"]));
    }
    
    public string CreateToken(AppUser appUser)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Email, appUser.Email),
            new Claim(JwtRegisteredClaimNames.GivenName, appUser.UserName)
        };

        var credentials = new SigningCredentials(_securityKey, SecurityAlgorithms.HmacSha512Signature);
        
        var tokenDescriptor = new SecurityTokenDescriptor
        { 
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(7),
            SigningCredentials = credentials,
            Issuer = _configuration["JWT:Issuer"],
            Audience = _configuration["JWT:Audience"]
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}