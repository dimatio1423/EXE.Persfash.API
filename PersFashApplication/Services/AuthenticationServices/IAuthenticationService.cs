using BusinessObject.Entities;
using BusinessObject.Models.PasswordModel;
using BusinessObject.Models.UserModels.Request;
using BusinessObject.Models.UserModels.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.AuthenticationServices
{
    public interface IAuthenticationService
    {
        Task<UserLoginResModel> Login(UserLoginReqModel userLoginReqModel);
        Task Logout(string refreshToken);
        Task ChangePassword(string token, ChangePasswordReqModel changePasswordReqModel);
        Task ForgotPassword(string email);
        Task ResetPassword(ResetPasswordReqModel resetPasswordReqModel);
        Task<UserInformationModel> GetUserInfor(string token);
        Task RegisterCustomer(UserLoginReqModel userLoginReqModel);
    }
}
