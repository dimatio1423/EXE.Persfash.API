using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Org.BouncyCastle.Math.EC.ECCurve;
using MailKit.Net.Smtp;
using System.Security.Cryptography.X509Certificates;


namespace Services.EmailService
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendCoursePaymentSuccessEmail(string fullName, string courseName, string userEmail)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("SmtpSettings:Username").Value));
            email.To.Add(MailboxAddress.Parse(userEmail));
            email.Subject = "[PersFash Application] - Course Payment Confirmation";

            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = $@"
         <!DOCTYPE html>
         <html lang='en'>
         <head>
             <meta charset='UTF-8'>
             <meta name='viewport' content='width=device-width, initial-scale=1.0'>
             <title>Payment Confirmation</title>
         </head>
         <body style='font-family: Arial, sans-serif; background-color: #dcf343; color: #ffffff;'>
             <div style='max-width: 650px; margin: 0 auto; padding: 20px; background-color: #4949e9; '>
                 <h1 style='color: #ffffff;'>Payment Received!</h1>
                 <p style='color: #ffffff;'>Hi {fullName},</p>
                 <p style='color: #ffffff;'>We have successfully received your payment for the course: <strong>{courseName}</strong>.</p>
                 <p style='color: #ffffff;'>You are now enrolled in the course. Get ready to learn from the best in the fashion industry!</p>
                 <p style='color: #ffffff;'>Thank you,</p>
                 <p style='color: #ffffff;'>The PersFash Team</p>
             </div>
         </body>
         </html>"
            };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_config.GetSection("SmtpSettings:Host").Value, 587, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_config.GetSection("SmtpSettings:Username").Value, _config.GetSection("SmtpSettings:Password").Value);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }

        public async Task SendInfluencerRegistrationEmail(string fullName, string userEmail)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("SmtpSettings:Username").Value));
            email.To.Add(MailboxAddress.Parse(userEmail));
            email.Subject = "[PersFash Application] - Fashion Influencer Partner Registration Success";

            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = $@"
         <!DOCTYPE html>
         <html lang='en'>
         <head>
             <meta charset='UTF-8'>
             <meta name='viewport' content='width=device-width, initial-scale=1.0'>
             <title>Partner Registration Success</title>
         </head>
         <body style='font-family: Arial, sans-serif; background-color: #dcf343; color: #ffffff;'>
             <div style='max-width: 650px; margin: 0 auto; padding: 20px; background-color: #4949e9;'>
                 <h1 style='color: #ffffff;'>Welcome, {fullName}!</h1>
                 <p style='color: #ffffff;'>We are excited to have you join the PersFash family as a fashion influencer partner. You can now share your fashion insights, host courses, and connect with your audience!</p>
                 <p style='color: #ffffff;'>Start creating your profile and engaging with fashion enthusiasts.</p>
                 <p style='color: #ffffff; font-weight: bold;'>Please note that for every sale of your course, a fee of 5-10% will be charged to cover platform and service costs.</p>
                 <p style='color: #ffffff;'>Thank you,</p>
                 <p style='color: #ffffff;'>The PersFash Team</p>
             </div>
         </body>
         </html>"
            };


            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_config.GetSection("SmtpSettings:Host").Value, 587, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_config.GetSection("SmtpSettings:Username").Value, _config.GetSection("SmtpSettings:Password").Value);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }

        //public async Task SendPartnerRegistrationEmail(string partnerName, string partnerEmail)
        //{
        //    var email = new MimeMessage();
        //    email.From.Add(MailboxAddress.Parse(_config.GetSection("SmtpSettings:Username").Value));
        //    email.To.Add(MailboxAddress.Parse(partnerEmail));
        //    email.Subject = "[PersFash Application] - Store Partner Registration Success";

        //    email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        //    {
        //        Text = $@"
        // <!DOCTYPE html>
        // <html lang='en'>
        // <head>
        //     <meta charset='UTF-8'>
        //     <meta name='viewport' content='width=device-width, initial-scale=1.0'>
        //     <title>Partner Registration Success</title>
        // </head>
        // <body style='font-family: Arial, sans-serif; background-color: #dcf343; color: #ffffff;'>
        //     <div style='max-width: 650px; margin: 0 auto; padding: 20px; background-color: #4949e9;'>
        //         <h1 style='color: #ffffff;'>Welcome, {partnerName}!</h1>
        //         <p style='color: #ffffff;'>We are thrilled to have your store as a partner with PersFash. Now, you can showcase your products and reach a wide audience of fashion enthusiasts.</p>
        //         <p style='color: #ffffff;'>Start managing your store and connecting with customers!</p>
        //         <p style='color: #ffffff;'>Thank you,</p>
        //         <p style='color: #ffffff;'>The PersFash Team</p>
        //     </div>
        // </body>
        // </html>"
        //    };

        //    using var smtp = new SmtpClient();
        //    await smtp.ConnectAsync(_config.GetSection("SmtpSettings:Host").Value, 587, MailKit.Security.SecureSocketOptions.StartTls);
        //    await smtp.AuthenticateAsync(_config.GetSection("SmtpSettings:Username").Value, _config.GetSection("SmtpSettings:Password").Value);
        //    await smtp.SendAsync(email);
        //    await smtp.DisconnectAsync(true);
        //}

        // Gửi lúc đăng ký thành công
        public async Task SendRegistrationEmail(string fullName, string userEmail)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("SmtpSettings:Username").Value));
            email.To.Add(MailboxAddress.Parse($"{userEmail}"));
            email.Subject = "[PersFash Application] - Welcome to PersFash!";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = $@"
                     <!DOCTYPE html>
                     <html lang='en'>
                     <head>
                         <meta charset='UTF-8'>
                         <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                         <title>Welcome to PersFash</title>
                     </head>
                     <body style='font-family: Arial, sans-serif; background-color: #dcf343; color: #ffffff;'>
                         <div style='max-width: 650px; margin: 0 auto; padding: 20px; background-color: #4949e9; '>
                             <h1 style='color: #ffffff;'>Welcome to PersFash!</h1>
                             <p style='color: #ffffff;'>Hi {fullName},</p>
                             <p style='color: #ffffff;'>Thank you for registering with PersFash. We're excited to have you on board! Explore our platform for the latest in fashion, style advice, and personalized 

.</p>
                             <p style='color: #ffffff;'>We hope you enjoy the experience!</p>
                             <p style='color: #ffffff;'>Thank you,</p>
                             <p style='color: #ffffff;'>The PersFash Team</p>
                         </div>
                     </body>
                     </html>"
            };


            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_config.GetSection("SmtpSettings:Host").Value, 587, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_config.GetSection("SmtpSettings:Username").Value, _config.GetSection("SmtpSettings:Password").Value);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
            throw new NotImplementedException();
        }

        public async Task SendUpgradeToPremiumEmail(string fullName, string userEmail, string subscriptionName)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("SmtpSettings:Username").Value));
            email.To.Add(MailboxAddress.Parse(userEmail));
            email.Subject = $"[PersFash Application] - Congratulations on Your {subscriptionName} Upgrade!";

            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = $@"
         <!DOCTYPE html>
         <html lang='en'>
         <head>
             <meta charset='UTF-8'>
             <meta name='viewport' content='width=device-width, initial-scale=1.0'>
             <title>{subscriptionName} Upgrade Success</title>
         </head>
         <body style='font-family: Arial, sans-serif; background-color: #dcf343; color: #ffffff;'>
             <div style='max-width: 650px; margin: 0 auto; padding: 20px; background-color: #4949e9; '>
                 <h1 style='color: #ffffff;'>You are now a {subscriptionName} Member!</h1>
                 <p style='color: #ffffff;'>Hi {fullName},</p>
                 <p style='color: #ffffff;'>Congratulations on upgrading to {subscriptionName}! You now have access to exclusive content, features, and perks available only to {subscriptionName} members.</p>
                 <p style='color: #ffffff;'>Enjoy the enhanced experience!</p>
                 <p style='color: #ffffff;'>Thank you,</p>
                 <p style='color: #ffffff;'>The PersFash Team</p>
             </div>
         </body>
         </html>"
            };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_config.GetSection("SmtpSettings:Host").Value, 587, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_config.GetSection("SmtpSettings:Username").Value, _config.GetSection("SmtpSettings:Password").Value);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);

        }

        public async Task SendUserResetPassword(string fullName, string userEmail, string OTP)
        {
            // Create the email message
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("SmtpSettings:Username").Value));
            email.To.Add(MailboxAddress.Parse(userEmail));
            email.Subject = "[PersFash Application] - Password Reset Request";

            // Construct the email body
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = $@"
        <!DOCTYPE html>
        <html lang='en'>
        <head>
            <meta charset='UTF-8'>
            <meta name='viewport' content='width=device-width, initial-scale=1.0'>
            <title>Password Reset</title>
        </head>
        <body style='font-family: Arial, sans-serif; background-color: #dcf343; color: #ffffff;'>
            <div style='max-width: 650px; margin: 0 auto; padding: 20px; background-color: #4949e9; '>
                <h1 style='color: #ffffff;'>Password Reset Request</h1>
                <p style='color: #ffffff;'>Hi {fullName},</p>
                <p style='color: #ffffff;'>You have requested to reset your password. Please use the following OTP (One-Time Password) to reset your password:</p>
                <p style='font-size: 24px; font-weight: bold; color: #fffff;'>{OTP}</p>
                <p style='color: #ffffff;'>This OTP is valid for a limited time. Please use it as soon as possible.</p>
                <p style='color: #ffffff;'>If you did not request a password reset, please ignore this email.</p>
                <p style='color: #ffffff;'>Thank you,</p>
                <p style='color: #ffffff;'>The PersFash Team</p>
            </div>
        </body>
        </html>"
            };

            // Send the email using SMTP
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_config.GetSection("SmtpSettings:Host").Value, 587, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_config.GetSection("SmtpSettings:Username").Value, _config.GetSection("SmtpSettings:Password").Value);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
