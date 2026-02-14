using Azure;
using GravyFoodsApi.MasjidRepository;
using GravyFoodsApi.Models;
using GravyFoodsApi.Data;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace GravyFoodsApi.MasjidServices
{
    public class AuthService : IAuthService
    {
        private readonly ILoginService _loginService;
        private readonly string _jwtKey;
        private readonly MasjidDBContext _db;

        private readonly IConfiguration _config;

        public AuthService(ILoginService userRepository, IConfiguration config)
        {
            _loginService = userRepository;
            _jwtKey = config["Jwt:Key"] ?? "Subana11ahi#Wabihamdihi$Subahana11ahiul@Azim!";
            _config = config;
        }

        public async Task<string?> LoginAsync(LoginRequest request)
        {
            try
            {
                var user = await _loginService.GetUser(request.Username, request.Password, request.BranchCode, request.CompanyCode);
                if (user == null) return null;

                return GenerateTokenAsync(request).Result;
                //return user;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<string?> GenerateTokenAsync(LoginRequest request)
        {
            try
            {

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtKey));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                {
                    //new Claim(ClaimTypes.Name, user.Username)
                    new Claim(JwtRegisteredClaimNames.Sub, request.Username.ToString()),
                    new Claim("companyId", request.CompanyCode.ToString()),
                    new Claim("branchId", request.BranchCode.ToString()),
                    

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
            catch (Exception ex)
            {
                return null;
            }
        }

        private async Task GenerateRefreshTocken()
        {
            var refreshToken = RefreshTokenGenerator.Generate();
            var refreshTokenHash = RefreshTokenGenerator.Hash(refreshToken);

            //_db.RefreshTokens.Add(new RefreshTokens
            //{
            //    UserId = User.UserId,
            //    TokenHash = refreshTokenHash,
            //    ExpiresAt = DateTime.UtcNow.AddDays(7)
            //});

            //await _db.SaveChangesAsync();

            //// Send as HttpOnly cookie
            //Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
            //{
            //    HttpOnly = true,
            //    Secure = true,
            //    SameSite = SameSiteMode.None,
            //    Expires = DateTime.UtcNow.AddDays(7)
            //});

        }

        //public Task<string?> ValidateRefreshToken(string refreshToken)
        //{

        //    return null;
        //}

        public async Task<UserInfoDTO?> ValidateRefreshTokenAsync(string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
                return null;

            var tokenHash = RefreshTokenGenerator.Hash(refreshToken);

            var storedToken = await _db.RefreshTokens
                .Include(rt => rt.User)
                .FirstOrDefaultAsync(rt =>
                    rt.TokenHash == tokenHash &&
                    !rt.IsRevoked &&
                    rt.ExpiresAt > DateTime.UtcNow);

            //return storedToken.User;
            return null;
        }

        public Task<string?> ValidateRefreshToken(string refreshToken)
        {
            throw new NotImplementedException();
        }
    }


}
