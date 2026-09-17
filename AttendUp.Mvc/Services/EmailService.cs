using AttendUp.Mvc.Services.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace AttendUp.Mvc.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendAsync(string to, string subject, string body)
        {
            var emailSection = _configuration.GetSection("Email");

            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(emailSection["FromName"], emailSection["FromAddress"]));

            message.To.Add(MailboxAddress.Parse(to));

            message.Subject = subject;

            message.Body = new TextPart("html")
            {
                Text = body
            };

            using var smtp = new SmtpClient();

            await smtp.ConnectAsync(
                emailSection["Host"],
                int.Parse(emailSection["Port"]),
                SecureSocketOptions.StartTls);

            await smtp.AuthenticateAsync(
                emailSection["Username"],
                emailSection["Password"]);

            await smtp.SendAsync(message);

            await smtp.DisconnectAsync(true);
        }
    }
}