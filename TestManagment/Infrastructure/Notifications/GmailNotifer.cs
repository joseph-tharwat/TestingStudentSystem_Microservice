using MimeKit;
using MailKit.Net.Smtp;
using TestManagment.ApplicationLayer.Interfaces.TestReminder;
using Microsoft.Extensions.Options;

namespace TestManagment.Infrastructure.Notifications
{
    public class GmailNotifer : INotifyService
    {
        private readonly IOptions<GmailSettings> gmailSettings;

        public GmailNotifer(IOptions<GmailSettings> gmailSettings)
        {
            this.gmailSettings = gmailSettings;
        }
        public async Task Notify(List<string> usersEmails)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(gmailSettings.Value.Email));
            foreach (var userEmail in usersEmails)
            {
                email.To.Add(MailboxAddress.Parse(userEmail));
            }
            email.Subject = "Ready for the test";
            email.Body = new TextPart("plain") { Text = "Be ready for the test" };
            await sendEmail(email);
        }

        private async Task sendEmail(MimeMessage email)
        {
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(gmailSettings.Value.Email, gmailSettings.Value.AppPassword);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
