using System.Net.Mail;
using System.Net;

namespace companyappbasic.Services.EmailServices
{
    public class EmailServi :IEmail
    {
        private readonly IConfiguration _configuration;

        public EmailServi(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string body)
        {
            try
            {
                var smtpClient = new SmtpClient(_configuration["Smtp:Host"])
                {
                    Port = int.Parse(_configuration["Smtp:Port"]!),
                    Credentials = new NetworkCredential(_configuration["Smtp:UserName"], _configuration["Smtp:Password"]),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_configuration["Smtp:FromEmail"]!),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(email);

                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"error: {ex.Message}");
                throw;
            }
        }
    }
}
