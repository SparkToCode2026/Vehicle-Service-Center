using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VehicleServiceCenter.DTOs;
using VehicleServiceCenter.Models;

namespace VehicleServiceCenter.Services
{
    public class JwtTokenService
    {
        private readonly IConfiguration _configuration;

        public JwtTokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public LoginResponse CreateToken(UserModel user)
        {
            string? key = _configuration["Jwt:Key"];
            string? issuer = _configuration["Jwt:Issuer"];
            string? audience = _configuration["Jwt:Audience"];
            string? expireMinutesValue =
                _configuration["Jwt:ExpireMinutes"];

            if (string.IsNullOrWhiteSpace(key))
            {
                throw new InvalidOperationException(
                    "JWT signing key is missing."
                );
            }

            if (string.IsNullOrWhiteSpace(issuer))
            {
                throw new InvalidOperationException(
                    "JWT issuer is missing."
                );
            }

            if (string.IsNullOrWhiteSpace(audience))
            {
                throw new InvalidOperationException(
                    "JWT audience is missing."
                );
            }

            if (!int.TryParse(
                    expireMinutesValue,
                    out int expireMinutes) ||
                expireMinutes <= 0)
            {
                throw new InvalidOperationException(
                    "JWT expiration must be a positive number."
                );
            }

            DateTime issuedAtUtc = DateTime.UtcNow;
            DateTime expiresAtUtc =
                issuedAtUtc.AddMinutes(expireMinutes);

            List<Claim> claims = new()
            {
                new Claim(
                    JwtRegisteredClaimNames.Sub,
                    user.UserId.ToString()
                ),

                new Claim(
                    ClaimTypes.NameIdentifier,
                    user.UserId.ToString()
                ),

                new Claim(
                    JwtRegisteredClaimNames.UniqueName,
                    user.UserName
                ),

                new Claim(
                    JwtRegisteredClaimNames.Email,
                    user.Email
                ),

                new Claim(
                    ClaimTypes.Role,
                    user.Role
                ),

                new Claim(
                    JwtRegisteredClaimNames.Jti,
                    Guid.NewGuid().ToString()
                )
            };

            SymmetricSecurityKey securityKey = new(
                Encoding.UTF8.GetBytes(key)
            );

            SigningCredentials signingCredentials = new(
                securityKey,
                SecurityAlgorithms.HmacSha256
            );

            JwtSecurityToken token = new(
                issuer: issuer,
                audience: audience,
                claims: claims,
                notBefore: issuedAtUtc,
                expires: expiresAtUtc,
                signingCredentials: signingCredentials
            );

            JwtSecurityTokenHandler tokenHandler = new();

            return new LoginResponse
            {
                AccessToken = tokenHandler.WriteToken(token),
                ExpiresAtUtc = expiresAtUtc,
                UserId = user.UserId,
                UserName = user.UserName,
                Role = user.Role
            };
        }
    }
}
