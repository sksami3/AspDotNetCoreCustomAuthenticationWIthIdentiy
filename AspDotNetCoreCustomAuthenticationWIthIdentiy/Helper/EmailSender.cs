using AspDotNetCoreCustomAuthenticationWIthIdentiy.Domain.AuthModel;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AspDotNetCoreCustomAuthenticationWIthIdentiy.Helper
{
    public static class EmailSender
    {
        #region Email
        public static bool SendEmail(ApplicationUser user,string token = "")
        {
            #region
            var file = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "EmailTemplate", "reset.txt");
            string body = System.IO.File.ReadAllText(file);
            if (!string.IsNullOrEmpty(body))
            {
                body = body.Replace("$websitelink$", token);
                body = body.Replace("$resetlink$", (token + "/confirmEmail" + user.Id).ToString());
            }
            #endregion
            try
            {
                // create email message
                var email = new MimeMessage();
                email.Sender = MailboxAddress.Parse("rakibchowdhury01942@gmail.com");
                email.To.Add(MailboxAddress.Parse(user.Email));
                email.Subject = "Test Email Subject";
                email.Body = new TextPart(TextFormat.Html) { Text = body };

                // send email
                using var smtp = new SmtpClient();
                smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
                smtp.Authenticate("rakibchowdhury01942@gmail.com", "shusil00");
                smtp.Send(email);
                smtp.Disconnect(true);

                return true;
            }
            catch (Exception e)
            {
                throw e;
            }

        }
        #endregion
    }
}
