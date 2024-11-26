using System.Net.Mail;
using System.Net;

namespace Cultivatrade.Api.Services
{
    public class EmailSender
    {
        public void SendEmail(string toAddress, int verificationCode)
        {
            string smtpHost = "smtp.gmail.com";
            int smtpPort = 587;
            //string smtpUsername = "dkhrl2000@gmail.com";
            //string smtpPassword = "fwmgacelbhtbkvzx";
            string smtpUsername = "cultivatrade@gmail.com";
            string smtpPassword = "jhevmrvfbyqwwoku";
            bool enableSsl = true;

            using (var smtpClient = new SmtpClient(smtpHost, smtpPort))
            {
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(smtpUsername, smtpPassword);
                smtpClient.EnableSsl = enableSsl;

                var mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(smtpUsername);
                mailMessage.To.Add(toAddress);
                mailMessage.Subject = "Good day " + toAddress;
                mailMessage.Body = "Your verification is: " + verificationCode;
                mailMessage.IsBodyHtml = true;

                try
                {

                    smtpClient.Send(mailMessage);
                    Console.WriteLine("Email sent successfully!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to send email. Error: " + ex.Message);
                }
            }
        }
    }
}
