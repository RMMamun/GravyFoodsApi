using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GravyFoodsApi.MasjidServices
{
    public class AuthService : IAuthService
    {
        private readonly ILoginService _loginService;
        private readonly string _jwtKey;

        public AuthService(ILoginService userRepository, IConfiguration config)
        {
            _loginService = userRepository;
            _jwtKey = config["Jwt:Key"] ?? "Subana11ahi#Wabihamdihi$Subahana11ahiul@Azim!";
        }

        public string? Authenticate(LoginRequest request)
        {
            var user = _loginService.GetUser(request.Username, request.Password);
            if (user == null) return null;

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username)
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }


}
