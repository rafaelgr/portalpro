using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace PortalProWebApi
{
    public static partial class PortalProMailController
    {
        public static EmailConfig GetEmailConfig()
        {
            return new EmailConfig();
        }
        public static Email SendEmail(string addressTo, string subject, string body)
        {
            Email e = new Email();
            //
            e.Message = new MailMessage();
            EmailConfig eC = new EmailConfig();
            e.Message.From = new MailAddress(eC.Address);
            e.Message.To.Add(addressTo);
            e.Message.Bcc.Add(eC.AddressCc);
            e.Message.Subject = subject;
            e.Message.Body = body;
            e.Message.IsBodyHtml = true;
            e.Message.Priority = MailPriority.Normal;
            //
            e.ClientSmtp = new SmtpClient(eC.Server);
            e.ClientSmtp.Credentials = new NetworkCredential(eC.Usr, eC.Password);
            e.ClientSmtp.Port = eC.Port;
            e.ClientSmtp.EnableSsl = eC.UseSsl;
            e.ClientSmtp.Send(e.Message);
            return e;
        }
    }
}