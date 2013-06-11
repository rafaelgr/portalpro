using System;
using System.Net.Mail;

namespace PortalProWebApi
{
    public class Email
    {
        public DateTime When { get; set; }
        public MailMessage Message { get; set; }
        public SmtpClient ClientSmtp { get; set; }
    }
}
