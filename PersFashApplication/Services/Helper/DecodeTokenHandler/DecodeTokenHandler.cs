using BusinessObject.Models.TokenModels;
using Services.AuthenticationServices;
using Services.JWTService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services.Helpers.Handler.DecodeTokenHandler
{
    public class DecodeTokenHandler : IDecodeTokenHandler
    {
        private readonly IJWTService _jWTService;

        public DecodeTokenHandler(IJWTService jWTService)
        {
            _jWTService = jWTService;
        }
        public TokenModel decode(string token)
        {
            var roleName = _jWTService.decodeToken(token, ClaimsIdentity.DefaultRoleClaimType);
            var userId = _jWTService.decodeToken(token, "userid");
            var email = _jWTService.decodeToken(token, "email");
            var username = _jWTService.decodeToken(token, "username");

            return new TokenModel(userId, roleName, email, username);
        }
    }
}
