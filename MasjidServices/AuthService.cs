using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text;

namespace GravyFoodsApi.MasjidServices
{
    public class AuthService : IAuthService
    {
        private readonly ILoginService _loginService;
        private readonly string _jwtKey;

        private readonly IConfiguration _config;

        public AuthService(ILoginService userRepository, IConfiguration config)
        {
            _loginService = userRepository;
            _jwtKey = config["Jwt:Key"] ?? "Subana11ahi#Wabihamdihi$Subahana11ahiul@Azim!";
            _config = config;
        }

        public async Task<string?> Authenticate(LoginRequest request)
        {
            var user = _loginService.GetUser(request.Username, request.Password,request.CompanyCode);
            if (user == null) return null;

            return GenerateTokenAsync(request).Result;
        }

        public async Task<string?> GenerateTokenAsync(LoginRequest request)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                //new Claim(ClaimTypes.Name, user.Username)
                new Claim(JwtRegisteredClaimNames.Sub, request.Username.ToString()),
                new Claim("companyId", request.CompanyCode.ToString()),
                new Claim("branchId", request.BranchCode.ToString())

            };

            var token = new JwtSecurityToken(
                claims: claims,
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }



        public Task<string?> ValidateRefreshToken(string refreshToken)
        {

            return null;
        }
    }


}
