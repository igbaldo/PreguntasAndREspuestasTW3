
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace TPWebIII.Utils.EmailsManager
{
    public class EmailsManager
    {
        public string body { get; set; }
        public string recipients { get; set; }
        public string recipientsCc { get; set; }
        public string recipientsBcc { get; set; }
        public string subject { get; set; }

        public void SendEmail()
        {
            var mail = new MailMessage();
            mail.To.Add(this.recipients);

            if (!string.IsNullOrEmpty(this.recipientsCc))
                mail.CC.Add(this.recipientsCc);

            if (!string.IsNullOrEmpty(this.recipientsBcc))
                mail.Bcc.Add(this.recipientsBcc);
            
            mail.From = new MailAddress("diegotpsunlam@gmail.com");
            mail.Subject = this.subject;
            mail.Body = this.body;
            mail.IsBodyHtml = true;

            var smtp = this.GetSmtpCredentials();

            //this.SetCertificateValidation();

            smtp.Send(mail);
        }

        #region HelperMethods

        private SmtpClient GetSmtpCredentials()
        {
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential("diegotpsunlam@gmail.com", "PrograWeb2019"),
                EnableSsl = true
            };

            return smtp;
        }

        private void SetCertificateValidation()
        {
            ServicePointManager.ServerCertificateValidationCallback =
                delegate(object s
                    , X509Certificate certificate
                    , X509Chain chain
                    , SslPolicyErrors sslPolicyErrors)
                { return true; };
        }

        #endregion
    }
}