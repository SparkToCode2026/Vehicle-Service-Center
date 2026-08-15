using MailKit.Net.Smtp;
using MimeKit;

namespace VehicleServiceCenter.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        public EmailService(IConfiguration config) => _config = config;

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            string senderName = GetRequiredSetting(
                "EmailSettings:SenderName");
            string senderEmail = GetRequiredSetting(
                "EmailSettings:SenderEmail");
            string senderPassword = GetRequiredSetting(
                "EmailSettings:SenderPassword");
            string smtpServer = GetRequiredSetting(
                "EmailSettings:SmtpServer");

            if (!int.TryParse(
                    _config["EmailSettings:SmtpPort"],
                    out int smtpPort) ||
                smtpPort <= 0)
            {
                throw new InvalidOperationException(
                    "EmailSettings:SmtpPort must be a positive number.");
            }

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(
                senderName,
                senderEmail));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = subject;
            message.Body = new TextPart("plain") { Text = body };

            using var client = new SmtpClient();
            client.CheckCertificateRevocation = false;
            await client.ConnectAsync(
                smtpServer,
                smtpPort,
                MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(
                senderEmail,
                senderPassword);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }

        private string GetRequiredSetting(string key)
        {
            string? value = _config[key];
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new InvalidOperationException(
                    $"Configuration value '{key}' is missing.");
            }

            return value;
        }
    }
}
