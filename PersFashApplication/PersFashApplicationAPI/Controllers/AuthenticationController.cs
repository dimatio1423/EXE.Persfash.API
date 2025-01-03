﻿using BusinessObject.Models.PasswordModel;
using BusinessObject.Models.ResultModel;
using BusinessObject.Models.UserModels.Request;
using BusinessObjects.Models.RefreshTokenModel.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.AuthenticationServices;
using System.Net;

namespace PersFashApplicationAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginReqModel userLoginReqModel)
        {
            var result = await _authenticationService.Login(userLoginReqModel);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Login successfully",
                Data = result,
            };

            return StatusCode(response.Code, response);
        }

        [HttpPost]
        [Route("login-google")]
        public async Task<IActionResult> LoginGoogle([FromBody] UserLoginGoogleReqModel userLoginGoogleReqModel)
        {
            var result = await _authenticationService.LoginGoogle(userLoginGoogleReqModel);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Login google successfully",
                Data = result,
            };

            return StatusCode(response.Code, response);
        }

        [HttpGet]
        [Authorize]
        [Route("user-infor")]
        public async Task<IActionResult> GetUserInfor()
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            var result = await _authenticationService.GetUserInfor(token);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Get user information successfully",
                Data = result,
            };

            return StatusCode(response.Code, response);
        }

        [HttpPost]
        [Route("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody]ForgotPasswordReqModel forgotPasswordReqModel)
        {
            await _authenticationService.ForgotPassword(forgotPasswordReqModel.email);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = $"Send OTP code to {forgotPasswordReqModel.email} successfully",
            };

            return StatusCode(response.Code, response);
        }

        [HttpPost]
        [Authorize]
        [Route("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordReqModel changePasswordReqModel)
        {
            var token = Request.Headers["Authorization"].ToString().Split(" ")[1];

            await _authenticationService.ChangePassword(token, changePasswordReqModel);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Change password successfully",
            };

            return StatusCode(response.Code, response);
        }

        [HttpPost]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordReqModel resetPasswordReqModel)
        {
            await _authenticationService.ResetPassword(resetPasswordReqModel);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Reset password successfully",
            };

            return StatusCode(response.Code, response);
        }


        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> Logout(RefreshTokenReqModel refreshTokenReqModel)
        {
            await _authenticationService.Logout(refreshTokenReqModel.RefreshToken);

            ResultModel response = new ResultModel
            {
                IsSuccess = true,
                Code = (int)HttpStatusCode.OK,
                Message = "Logout successfully",
            };

            return StatusCode(response.Code, response);
        }
    }
}
