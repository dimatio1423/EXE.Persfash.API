using BusinessObject.Entities;
using BusinessObject.Enums;
using BusinessObject.Models.UserModels.Request;
using BusinessObject.Models.UserModels.Response;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repositories.FashionInfluencerRepos;
using Repositories.PartnerRepos;
using Repositories.RefreshTokenRepos;
using Repositories.UserRepos;
using Services.Helper.CustomExceptions;
using Services.JWTService;
using Services.Security;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace Services.AuthenticationServices
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IPartnerRepository _partnerRepository;
        private readonly IFashionInfluencerRepository _fashionInfluencerRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IJWTService _jWTService;

        public AuthenticationService(ICustomerRepository customerRepository, 
            IPartnerRepository partnerRepository, 
            IFashionInfluencerRepository fashionInfluencerRepository,
            IRefreshTokenRepository refreshTokenRepository,
            IJWTService jWTService)
        {
            _customerRepository = customerRepository;
            _partnerRepository = partnerRepository;
            _fashionInfluencerRepository = fashionInfluencerRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _jWTService = jWTService;
        }

        public async Task<UserLoginResModel> Login(UserLoginReqModel userLoginReqModel)
        {
            var currentCustomer = await _customerRepository.GetCustomerByEmail(userLoginReqModel.Email);
            var currentPartner = await _partnerRepository.GetPartnerByEmail(userLoginReqModel.Email);
            var currentInfluencer = await _fashionInfluencerRepository.GetFashionInfluencerByEmail(userLoginReqModel.Email);

            if (currentCustomer != null)
            {
                if (PasswordHasher.VerifyPassword(userLoginReqModel.Password, currentCustomer.Password))
                {
                    var token = _jWTService.GenerateJWT(currentCustomer);

                    var refreshToken = _jWTService.GenerateRefreshToken();

                    var newRefreshToken = new RefreshToken
                    {
                        Token = refreshToken,
                        ExpiredAt = DateTime.Now.AddDays(1),
                        CustomerId = currentCustomer.CustomerId
                    };

                    await _refreshTokenRepository.Add(newRefreshToken);

                    var userLoginRes = new UserLoginResModel
                    {
                        UserId = currentCustomer.CustomerId,
                        Username = currentCustomer.Username,
                        Email = currentCustomer.Email,
                        Role = RoleEnums.Customer.ToString(),
                        Token = token,
                        RefreshToken = refreshToken
                    };

                    return userLoginRes;
                }
                else
                {
                    throw new ApiException(HttpStatusCode.BadRequest, "Incorrect password");

                }
            } else if (currentPartner != null)
            {
                if (PasswordHasher.VerifyPassword(userLoginReqModel.Password, currentPartner.Password))
                {
                    var token = _jWTService.GenerateJWT(currentPartner);

                    var refreshToken = _jWTService.GenerateRefreshToken();

                    var newRefreshToken = new RefreshToken
                    {
                        Token = refreshToken,
                        ExpiredAt = DateTime.Now.AddDays(1),
                        PartnerId = currentPartner.PartnerId
                    };

                    await _refreshTokenRepository.Add(newRefreshToken);

                    var userLoginRes = new UserLoginResModel
                    {
                        UserId = currentPartner.PartnerId,
                        Username = currentPartner.Email,
                        Email = currentPartner.Email,
                        Role = RoleEnums.Partner.ToString(),
                        Token = token,
                        RefreshToken = refreshToken
                    };

                    return userLoginRes;
                }
                else
                {
                    throw new ApiException(HttpStatusCode.BadRequest, "Incorrect password");

                }
            }else if (currentInfluencer != null)
            {
                if (PasswordHasher.VerifyPassword(userLoginReqModel.Password, currentInfluencer.Password))
                {
                    var token = _jWTService.GenerateJWT(currentInfluencer);

                    var refreshToken = _jWTService.GenerateRefreshToken();

                    var newRefreshToken = new RefreshToken
                    {
                        Token = refreshToken,
                        ExpiredAt = DateTime.Now.AddDays(1),
                        InfluencerId = currentInfluencer.InfluencerId
                    };

                    await _refreshTokenRepository.Add(newRefreshToken);

                    var userLoginRes = new UserLoginResModel
                    {
                        UserId = currentInfluencer.InfluencerId,
                        Username = currentInfluencer.Username,
                        Email = currentInfluencer.Email,
                        Role = RoleEnums.FashionInfluencer.ToString(),
                        Token = token,
                        RefreshToken = refreshToken
                    };

                    return userLoginRes;
                }
                else
                {
                    throw new ApiException(HttpStatusCode.BadRequest, "Incorrect password");

                }
            }
            else
            {
                throw new ApiException(HttpStatusCode.NotFound, "User does not exist");
            }
        }
    }
}
