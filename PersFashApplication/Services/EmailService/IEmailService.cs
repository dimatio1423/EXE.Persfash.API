using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.EmailService
{
    public interface IEmailService
    {
        Task SendRegistrationEmail(string fullName, string userEmail, EmailSendingFormat sendingFormat);
        Task SendUserResetPassword(string fullName, string userEmail,string OTP);

    }
}
