using System;
using System.Net;
using System.Net.Mail;

namespace EmailService
{
    public static class EmailSender
    {
        public static void SendEmail(string email)
        {
            MailAddress from = new MailAddress("pomodoro.kpi.team@gmail.com", "POMODODRO team");
            MailAddress to = new MailAddress(email);
            MailMessage message = new MailMessage(from, to);

            message.Subject = "Registration";
            message.Body = "Thank you for registration in POMODORO";

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;

            smtp.Credentials = new NetworkCredential("pomodoro.kpi.team@gmail.com", "PomidorEtoJagoda");
            
            smtp.Send(message);
            Console.WriteLine($"Message has been sent to {email}");
        }

    }
}
