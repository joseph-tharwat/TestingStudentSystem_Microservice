using MimeKit;
using MailKit.Net.Smtp;
using TestManagment.ApplicationLayer.Interfaces.TestReminder;

namespace TestManagment.Infrastructure.TestReminder
{
    //not completed...
    public class EmailNotifer : INotifyService
    {
        private readonly string SenderEmail = "jo.tharwat.jt@gmail.com";
        private readonly string SenderPassword = "Abc_12345";

        public async Task Notify(List<string> usersEmails)
        {
            throw new NotImplementedException("Need App Password configuration, some refactoring");

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(SenderEmail));
            foreach (var userEmail in usersEmails) 
            {
                email.To.Add(MailboxAddress.Parse(userEmail));
            }
            email.Subject = "Ready for the test";
            email.Body = new TextPart("plain") { Text = "Be ready for the test" };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(SenderEmail, SenderPassword);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}
