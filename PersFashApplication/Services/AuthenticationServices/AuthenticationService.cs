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
using Repositories.SubscriptionRepos;
using Repositories.SystemAdminRepos;
using Repositories.UserProfilesRepos;
using Repositories.UserRepos;
using Repositories.UserSubscriptionRepos;
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
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
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
        private readonly ICustomerSubscriptionRepository _customerSubscriptionRepository;
private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly ICustomerProfileRepository _customerProfileRepository;
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
            ICustomerSubscriptionRepository customerSubscriptionRepository,
            ISubscriptionRepository subscriptionRepository,
            ICustomerProfileRepository customerProfileRepository,
            VerificationCodeCache verificationCodeCache
            )
        {
             this.verificationCodeCache = verificationCodeCache;

            _customerRepository = customerRepository;
            _fashionInfluencerRepository = fashionInfluencerRepository;
            _adminRepository = adminRepository;
            _refreshTokenRepository = refreshTokenRepository;
            _customerSubscriptionRepository = customerSubscriptionRepository;
            _subscriptionRepository = subscriptionRepository;
            _customerProfileRepository = customerProfileRepository;
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

                var currCustomerSubscriptions = await _customerSubscriptionRepository.GetCustomerSubscriptionByCustomerId(currCustomer.CustomerId);

                UserInformationModel userInformation = new UserInformationModel
                {
                    UserId = currCustomer.CustomerId,
                    Username = currCustomer.Username,
                    Email = currCustomer.Email,
                    Role = RoleEnums.Customer.ToString(),
                    ProfileURL = currCustomer.ProfilePicture,
                    Gender = currCustomer.Gender,
                    Subscription = currCustomerSubscriptions.Where(x => x.IsActive == true).Select(x => x.Subscription.SubscriptionTitle).ToList(),
                    IsDoneProfileSetup = await _customerProfileRepository.GetCustomerProfileByCustomerId(currCustomer.CustomerId) != null ? true : false,
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
                if (currentCustomer.Status.Equals(StatusEnums.Inactive.ToString())) throw new ApiException(HttpStatusCode.BadRequest, "This account has been deactivated");

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

                if (currAdmin.Status.Equals(StatusEnums.Inactive.ToString())) throw new ApiException(HttpStatusCode.BadRequest, "This account has been deactivated");

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

        public async Task<UserLoginResModel> LoginGoogle(UserLoginGoogleReqModel userLoginGoogleReqModel)
        {
            string token = userLoginGoogleReqModel.Token;
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    HttpResponseMessage response = client.GetAsync("https://www.googleapis.com/oauth2/v3/userinfo").Result;

                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = response.Content.ReadAsStringAsync().Result;

                        var jsonData = JsonSerializer.Deserialize<Dictionary<string, object>>(responseContent);
                        string email = jsonData.ContainsKey("email") ? jsonData["email"].ToString() : string.Empty;
                        string givenName = jsonData.ContainsKey("given_name") ? jsonData["given_name"].ToString() : string.Empty;
                        string picture = jsonData.ContainsKey("picture") ? jsonData["picture"].ToString() : string.Empty;

                        var currCustomer = await _customerRepository.GetCustomerByEmail(email);
                        Customer customer;
                        if (currCustomer != null)
                        {
                            if (currCustomer.Status.Equals(StatusEnums.Inactive.ToString())) throw new ApiException(HttpStatusCode.BadRequest, "This account has been deactivated");

                            customer = currCustomer;
                        }
                        else
                        {
                            customer = new Customer
                            {
                                Email = email,
                                Username = email,
                                FullName = givenName,
                                ProfilePicture = picture,
                                Gender = GenderEnums.Male.ToString(), // Default value, this can be dynamic
                                Password = PasswordHasher.HashPassword(Guid.NewGuid().ToString()), // Random password
                                Status = StatusEnums.Active.ToString(),
                                DateJoined = DateTime.Now
                            };

                            var customerId = await _customerRepository.AddCustomer(customer);

                            var subscription = await _subscriptionRepository.GetSubscriptionsByName(SubscriptionTypeEnums.Free.ToString());

                            if (subscription == null)
                            {
                                throw new ApiException(HttpStatusCode.NotFound, "Subscription does not exist");
                            }

                            CustomerSubscription customerSubscription = new CustomerSubscription
                            {
                                SubscriptionId = subscription.SubscriptionId,
                                CustomerId = customerId,
                                StartDate = null,
                                EndDate = null,
                                IsActive = true,
                            };

                            await _customerSubscriptionRepository.Add(customerSubscription);

                            await _emailService.SendRegistrationEmail(customer.FullName, customer.Email);
                        }

                        // Authenticate the user and generate tokens
                        var newToken = _jWTService.GenerateJWT(customer);

                        var refreshToken = _jWTService.GenerateRefreshToken();

                        var newRefreshToken = new RefreshToken
                        {
                            Token = refreshToken,
                            ExpiredAt = DateTime.Now.AddDays(1),
                            CustomerId = customer.CustomerId
                        };

                        await _refreshTokenRepository.Add(newRefreshToken);

                        var userLoginRes = new UserLoginResModel
                        {
                            UserId = customer.CustomerId,
                            Username = customer.Username,
                            Email = customer.Email,
                            Role = RoleEnums.Customer.ToString(),
                            Token = newToken,
                            RefreshToken = refreshToken
                        };

                        return userLoginRes;
                    }
                    else
                    {
                        throw new ApiException(HttpStatusCode.Unauthorized, "Invalid Google token");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
