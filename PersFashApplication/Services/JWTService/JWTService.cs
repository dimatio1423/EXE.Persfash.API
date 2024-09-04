using BusinessObject.Entities;
using BusinessObject.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repositories.RefreshTokenRepos;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace Services.JWTService
{
    public class JWTService : IJWTService
    {
        private readonly IConfiguration _config;
        private readonly JwtSecurityTokenHandler _tokenHandler;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        public JWTService(IConfiguration config, IRefreshTokenRepository refreshTokenRepository)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _tokenHandler = new JwtSecurityTokenHandler();
            _refreshTokenRepository = refreshTokenRepository;
        }
        public string decodeToken(string jwtToken, string nameClaim)
        {
            Claim? claim = _tokenHandler.ReadJwtToken(jwtToken).Claims.FirstOrDefault(selector => selector.Type.ToString().Equals(nameClaim));

            return claim != null ? claim.Value : "Error!!!";
        }

        public string GenerateJWT<T>(T entity) where T : class
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:JwtKey"]));
            var credential = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            List<Claim> claims = new List<Claim>();

            if (entity is Customer customer)
            {
                claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, RoleEnums.Customer.ToString()));
                claims.Add(new Claim("userid", customer.CustomerId.ToString()));
                claims.Add(new Claim("email", customer.Email));
            }
            else if (entity is Partner partner)
            {
                claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, RoleEnums.Partner.ToString()));
                claims.Add(new Claim("userid", partner.PartnerId.ToString()));
                claims.Add(new Claim("email", partner.Email));
            }
            else if (entity is FashionInfluencer influencer)
            {
                claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, RoleEnums.FashionInfluencer.ToString()));
                claims.Add(new Claim("userid", influencer.InfluencerId.ToString()));
                claims.Add(new Claim("email", influencer.Email));
            }
            else
            {
                throw new ArgumentException("Unsupported entity type");
            }

            var token = new JwtSecurityToken(
               issuer: _config["JwtSettings:Issuer"],
               audience: _config["JwtSettings:Audience"],
               claims: claims,
               expires: DateTime.Now.AddMinutes(15),
               signingCredentials: credential
               );


            var encodetoken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodetoken;
        }

        public string GenerateRefreshToken()
        {
            var newRefreshToken = Guid.NewGuid().ToString();
            return newRefreshToken;
        }
    }
}
