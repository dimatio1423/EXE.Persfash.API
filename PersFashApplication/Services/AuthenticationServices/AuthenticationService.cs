using BusinessObject.Entities;
using BusinessObject.Enums;
using BusinessObject.Models.UserModels.Request;
using BusinessObject.Models.UserModels.Response;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repositories.FashionInfluencerRepos;
using Repositories.PartnerRepos;
using Repositories.RefreshTokenRepos;
using Repositories.SystemAdminRepos;
using Repositories.UserRepos;
using Services.Helper.CustomExceptions;
using Services.Helpers.Handler.DecodeTokenHandler;
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
        private readonly ISystemAdminRepository _adminRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IDecodeTokenHandler _decodeToken;
        private readonly IJWTService _jWTService;

        public AuthenticationService(ICustomerRepository customerRepository, 
            IPartnerRepository partnerRepository, 
            IFashionInfluencerRepository fashionInfluencerRepository,
            IRefreshTokenRepository refreshTokenRepository,
            ISystemAdminRepository adminRepository,
            IDecodeTokenHandler decodeToken,
            IJWTService jWTService)
        {
            _customerRepository = customerRepository;
            _partnerRepository = partnerRepository;
            _fashionInfluencerRepository = fashionInfluencerRepository;
            _adminRepository = adminRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _decodeToken = decodeToken;
            _jWTService = jWTService;
        }

        public async Task<UserInformationModel> GetUserInfor(string token)
        {
            var decode = _decodeToken.decode(token);

            var currCustomer = await _customerRepository.GetCustomerByUsername(decode.username);

            var currPartner = await _partnerRepository.GetPartnerByUsername(decode.username);

            var currInfluencer = await _fashionInfluencerRepository.GetFashionInfluencerByUsername(decode.username);

            var currAdmin = await _adminRepository.GetAdminByUsername(decode.username);

            if (currCustomer != null)
            {
                UserInformationModel userInformation = new UserInformationModel
                {
                    UserId = currCustomer.CustomerId,
                    Username = currCustomer.Username,
                    Email = currCustomer.Email,
                    Role = RoleEnums.Customer.ToString(),
                };

                return userInformation;
            }else if (currPartner != null)
            {
                UserInformationModel userInformation = new UserInformationModel
                {
                    UserId = currPartner.PartnerId,
                    Username = currPartner.Username,
                    Email = currPartner.Email,
                    Role = RoleEnums.Partner.ToString(),
                };

                return userInformation;
            }else if (currInfluencer != null)
            {
                UserInformationModel userInformation = new UserInformationModel
                {
                    UserId = currInfluencer.InfluencerId,
                    Username = currInfluencer.Username,
                    Email = currInfluencer.Email,
                    Role = RoleEnums.FashionInfluencer.ToString(),
                };

                return userInformation;
            }else if (currAdmin != null)
            {
                UserInformationModel userInformation = new UserInformationModel
                {
                    UserId = currAdmin.AdminId,
                    Username = currAdmin.Username,
                    Role = RoleEnums.Admin.ToString(),
                };

                return userInformation;
            }
            else
            {
                throw new ApiException(HttpStatusCode.NotFound, "User not found");
            }
        }

        public async Task<UserLoginResModel> Login(UserLoginReqModel userLoginReqModel)
        {
            var currentCustomer = await _customerRepository.GetCustomerByUsername(userLoginReqModel.Username);
            var currentPartner = await _partnerRepository.GetPartnerByUsername(userLoginReqModel.Username);
            var currentInfluencer = await _fashionInfluencerRepository.GetFashionInfluencerByUsername(userLoginReqModel.Username);
            var currAdmin = await _adminRepository.GetAdminByUsername(userLoginReqModel.Username);

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
            }else if (currAdmin != null)
            {
                if (PasswordHasher.VerifyPassword(userLoginReqModel.Password, currAdmin.Password))
                {
                    var token = _jWTService.GenerateJWT(currAdmin);

                    var refreshToken = _jWTService.GenerateRefreshToken();

                    var newRefreshToken = new RefreshToken
                    {
                        Token = refreshToken,
                        ExpiredAt = DateTime.Now.AddDays(1),
                        AdminId = currAdmin.AdminId
                    };

                    await _refreshTokenRepository.Add(newRefreshToken);

                    var userLoginRes = new UserLoginResModel
                    {
                        UserId = currAdmin.AdminId,
                        Username = currAdmin.Username,
                        Role = RoleEnums.Admin.ToString(),
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

        public Task RegisterCustomer(UserLoginReqModel userLoginReqModel)
        {
            throw new NotImplementedException();
        }
    }
}
