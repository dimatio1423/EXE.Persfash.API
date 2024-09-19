using BusinessObject.Entities;
using BusinessObject.Enums;
using BusinessObject.Models.PasswordModel;
using BusinessObject.Models.UserModels.Request;
using BusinessObject.Models.UserModels.Response;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repositories.FashionInfluencerRepos;
using Repositories.PartnerRepos;
using Repositories.RefreshTokenRepos;
using Repositories.SystemAdminRepos;
using Repositories.UserRepos;
using Services.EmailService;
using Services.Helper.CustomExceptions;
using Services.Helper.VerifyCode;
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
        private readonly IFashionInfluencerRepository _fashionInfluencerRepository;
        private readonly ISystemAdminRepository _adminRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IDecodeTokenHandler _decodeToken;
        private readonly IEmailService _emailService;
        private readonly IJWTService _jWTService;
        private readonly VerificationCodeCache verificationCodeCache;

        public AuthenticationService(ICustomerRepository customerRepository, 
            IFashionInfluencerRepository fashionInfluencerRepository,
            IRefreshTokenRepository refreshTokenRepository,
            ISystemAdminRepository adminRepository,
            IDecodeTokenHandler decodeToken,
            IEmailService emailService,
            IJWTService jWTService,
            VerificationCodeCache verificationCodeCache
            )
        {
             this.verificationCodeCache = verificationCodeCache;

            _customerRepository = customerRepository;
            _fashionInfluencerRepository = fashionInfluencerRepository;
            _adminRepository = adminRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _decodeToken = decodeToken;
            _emailService = emailService;
            _jWTService = jWTService;
        }

        public async Task ChangePassword(string token, ChangePasswordReqModel changePasswordReqModel)
        {
            var decode = _decodeToken.decode(token);

            var currCustomer = await _customerRepository.GetCustomerByUsername(decode.username);

            //var currPartner = await _partnerRepository.GetPartnerByUsername(decode.username);

            var currInfluencer = await _fashionInfluencerRepository.GetFashionInfluencerByUsername(decode.username);

            if (currCustomer != null)
            {

                if (!PasswordHasher.VerifyPassword(changePasswordReqModel.OldPassword, currCustomer.Password))
                {
                    throw new ApiException(HttpStatusCode.BadRequest, "The old password incorrect");
                }

                if (changePasswordReqModel.OldPassword.Equals(changePasswordReqModel.NewPassword))
                {
                    throw new ApiException(HttpStatusCode.BadRequest, "The new password is the same with old password");
                }

                currCustomer.Password = PasswordHasher.HashPassword(changePasswordReqModel.NewPassword);

                await _customerRepository.Update(currCustomer);
            }
            else if (currInfluencer != null)
            {
                if (!PasswordHasher.VerifyPassword(changePasswordReqModel.OldPassword, currInfluencer.Password))
                {
                    throw new ApiException(HttpStatusCode.BadRequest, "The old password incorrect");
                }

                if (changePasswordReqModel.OldPassword.Equals(changePasswordReqModel.NewPassword))
                {
                    throw new ApiException(HttpStatusCode.BadRequest, "The new password is the same with old password");
                }

                currInfluencer.Password = PasswordHasher.HashPassword(changePasswordReqModel.NewPassword);

                await _fashionInfluencerRepository.Update(currInfluencer);
            }
            else
            {
                throw new ApiException(HttpStatusCode.NotFound, "User does not exist");
            }
        }

        public async Task ForgotPassword(string email)
        {
            var currCustomer = await _customerRepository.GetCustomerByEmail(email);

            //var currPartner = await _partnerRepository.GetPartnerByEmail(email);

            var currInfluencer = await _fashionInfluencerRepository.GetFashionInfluencerByEmail(email);

            if (currCustomer != null)
            {

                var otp = GenerateOTP();

                verificationCodeCache.Put(currCustomer.Username, otp, 5);

                await _emailService.SendUserResetPassword(currCustomer.FullName, currCustomer.Email, otp);

            }
            else if (currInfluencer != null)
            {
                var otp = GenerateOTP();

                verificationCodeCache.Put(currInfluencer.Username, otp, 5);

                await _emailService.SendUserResetPassword(currInfluencer.FullName, currInfluencer.Email, otp);
            }
            else
            {
                throw new ApiException(HttpStatusCode.NotFound, "User does not exist");
            }
        }

        public async Task<UserInformationModel> GetUserInfor(string token)
        {
            var decode = _decodeToken.decode(token);

            var currCustomer = await _customerRepository.GetCustomerByUsername(decode.username);

            //var currPartner = await _partnerRepository.GetPartnerByUsername(decode.username);

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
                    ProfileURL = currCustomer.ProfilePicture,
                    Gender = currCustomer.Gender
                };

                return userInformation;
            }
            else if (currInfluencer != null)
            {
                UserInformationModel userInformation = new UserInformationModel
                {
                    UserId = currInfluencer.InfluencerId,
                    Username = currInfluencer.Username,
                    Email = currInfluencer.Email,
                    ProfileURL = currInfluencer.ProfilePicture,
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
            //var currentPartner = await _partnerRepository.GetPartnerByUsername(userLoginReqModel.Username);
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

        public async Task Logout(string refreshToken)
        {
            var currRefreshToken = await _refreshTokenRepository.GetByRefreshToken(refreshToken);

            if (currRefreshToken == null)
            {
                throw new ApiException(HttpStatusCode.NotFound, "Refresh token does not exist");
            }

            await _refreshTokenRepository.Remove(currRefreshToken);
        }

        public Task RegisterCustomer(UserLoginReqModel userLoginReqModel)
        {
            throw new NotImplementedException();
        }

        public async Task ResetPassword(ResetPasswordReqModel resetPasswordReqModel)
        {
            var currCustomer = await _customerRepository.GetCustomerByEmail(resetPasswordReqModel.Email);

            //var currPartner = await _partnerRepository.GetPartnerByEmail(resetPasswordReqModel.Email);

            var currInfluencer = await _fashionInfluencerRepository.GetFashionInfluencerByEmail(resetPasswordReqModel.Email);

            if (currCustomer != null)
            {

                var otp = verificationCodeCache.Get(currCustomer.Username);

                if (otp == null || !otp.Equals(resetPasswordReqModel.OTP))
                {
                    throw new ApiException(HttpStatusCode.BadRequest, "OTP has expired or invalid OTP");
                }

                currCustomer.Password = PasswordHasher.HashPassword(resetPasswordReqModel.NewPassword);

                await _customerRepository.Update(currCustomer);

            }
            else if (currInfluencer != null)
            {
                var otp = verificationCodeCache.Get(currInfluencer.Username);

                if (otp == null || !otp.Equals(resetPasswordReqModel.OTP))
                {
                    throw new ApiException(HttpStatusCode.BadRequest, "OTP has expired or invalid OTP");
                }

                currInfluencer.Password = PasswordHasher.HashPassword(resetPasswordReqModel.NewPassword);

                await _fashionInfluencerRepository.Update(currInfluencer);
            }
            else
            {
                throw new ApiException(HttpStatusCode.NotFound, "User does not exist");
            }
        }

        public string GenerateOTP()
        {
            var otp = new Random().Next(100000, 999999).ToString();

            return otp;
        }
    }
}
