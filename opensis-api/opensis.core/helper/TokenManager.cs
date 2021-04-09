using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IdentityModel.Tokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace opensis.core.helper
{
    public class TokenManager
    {
        private const string Secret = "ERMN05OPLoDvbTTa/QkqLNMI7cPLguaRyHzyg7n5qNBVjQmtBhz4SzYh4NBVCXi3KJHlSXKP+oi2+bXr6CUYTR==";

        public static string GenerateToken(string username)
        {
            //byte[] key = Convert.FromBase64String(Secret);
            //SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
            //SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            //{
            //    Subject = new ClaimsIdentity(new[] {
            //          new Claim(ClaimTypes.Name, username)}),
            //    Expires = DateTime.UtcNow.AddMinutes(30),
            //    SigningCredentials = new SigningCredentials(securityKey,
            //        SecurityAlgorithms.HmacSha256Signature)
            //};

            //JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            //JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);
            //return handler.WriteToken(token);

            return GenerateTokenWithExpiry(username).Token;
        }

        public static (string Token, DateTimeOffset Expiry) GenerateTokenWithExpiry(string username)
        {
            DateTimeOffset expiry = DateTimeOffset.UtcNow.AddMinutes(30);
            byte[] key = Convert.FromBase64String(Secret);
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(key);
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                      new Claim(ClaimTypes.Name, username)}),
                Expires = expiry.UtcDateTime,
                SigningCredentials = new SigningCredentials(securityKey,
                    SecurityAlgorithms.HmacSha256Signature)
            };

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken token = handler.CreateJwtSecurityToken(descriptor);
            var jwtToken = handler.WriteToken(token);
            return (jwtToken, expiry);
        }

        public static string RefreshToken(string token, string user_name)
        {
            string newToken = "";
            try
            {
                newToken= RefreshTokenWithExpiry(token, user_name).Token;

                //if (user_name != null && token != null)
                //{
                //    string tokenusername = TokenManager.ValidateToken(token);
                //    if (tokenusername != null)
                //    {
                //        if (tokenusername.Equals(user_name))
                //        {
                //            newToken = GenerateToken(user_name);
                //        }
                //    }
                //}
            }
            catch (Exception)
            {
                throw;
            }
            return newToken;
        }

        public static (string Token, DateTimeOffset Expiry) RefreshTokenWithExpiry(string token, string user_name)
        {
            try
            {
                if (CheckToken(user_name, token))
                    return GenerateTokenWithExpiry(user_name);
            }
            catch (Exception)
            {
                throw;
            }
            return default((string, DateTimeOffset));
        }

        public static ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
                if (jwtToken == null)
                    return null;
                byte[] key = Convert.FromBase64String(Secret);
                TokenValidationParameters parameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
                SecurityToken securityToken;
                ClaimsPrincipal principal = tokenHandler.ValidateToken(token,
                      parameters, out securityToken);
                return principal;
            }
            catch (Exception)
            {
                throw;
                //return null;
            }
        }

        public static string ValidateToken(string token)
        {
            string username = null;
            ClaimsPrincipal principal = GetPrincipal(token);
            if (principal == null)
                return null;
            ClaimsIdentity identity = null;
            try
            {
                identity = (ClaimsIdentity)principal.Identity;
            }
            catch (NullReferenceException)
            {
                return null;
            }
            Claim usernameClaim = identity.FindFirst(ClaimTypes.Name);
            username = usernameClaim.Value;
            return username;
        }

        public static bool CheckToken(string user_name, string token)
        {
            try
            {
                if (user_name != null && token != null)
                {
                    string tokenusername = TokenManager.ValidateToken(token);
                    if (tokenusername != null)
                    {
                        if (tokenusername.Equals(user_name))
                        {
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}