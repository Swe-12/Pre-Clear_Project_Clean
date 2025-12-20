using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace PreClear.Api.Helpers
{
    public static class JwtTokenGenerator
    {
        // Generate a JWT using a symmetric secret key
        public static string GenerateToken(IEnumerable<KeyValuePair<string, string>> claims, string secretKey, int expiresMinutes = 60, string? issuer = null, string? audience = null)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtClaims = new List<Claim>();
            foreach (var kv in claims)
            {
                // Map common claim names to proper ClaimTypes
                var claimType = kv.Key switch
                {
                    "sub" => ClaimTypes.NameIdentifier,
                    "email" => ClaimTypes.Email,
                    "role" => ClaimTypes.Role,
                    _ => kv.Key
                };
                jwtClaims.Add(new Claim(claimType, kv.Value));
            }

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: jwtClaims,
                expires: DateTime.UtcNow.AddMinutes(expiresMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
