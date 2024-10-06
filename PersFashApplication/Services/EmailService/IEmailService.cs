using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.EmailService
{
    public interface IEmailService
    {
        Task SendRegistrationEmail(string fullName, string userEmail);
        Task SendUserResetPassword(string fullName, string userEmail,string OTP);
        //Task SendPartnerRegistrationEmail(string parteName, string partnerEmail);
        Task SendUpgradeToPremiumEmail(string fullName, string userEmail, string subscriptionName);
        Task SendInfluencerRegistrationEmail(string fullName, string userEmail);
        Task SendCoursePaymentSuccessEmail(string fullName, string courseName, string userEmail);
        Task SendEmailForExpireSubscription(string fullName, string userEmail);
    }
}
