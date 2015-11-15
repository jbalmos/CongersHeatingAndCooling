using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;


namespace CongerHeatingAndCooling.Utilities
{
    public static class UtilitySmtp
    {
        public static void SendSmsMessage(string subject, string message, bool isHighPriority)
        {
            bool enableSmsMessaging = Boolean.Parse(ConfigurationManager.AppSettings["EnableSmsMessaging"]);
            if (enableSmsMessaging)
            {
                string[] recipientList = ConfigurationManager.AppSettings["SmsTo"].Split(',');
                SendSmtpMessage(recipientList, subject, message, false, isHighPriority);
            }
        }



        public static void SendSmtpMessage(string[] recipients, string subject, string messageBody, bool isHtml, bool isHighPriority)
        {

            string smtpServer = ConfigurationManager.AppSettings["smtpServer"];
            int smtpPort = Int32.Parse(ConfigurationManager.AppSettings["smtpPort"]);
            string smtpUsername = ConfigurationManager.AppSettings["smtpUsername"];
            string smtpPassword = ConfigurationManager.AppSettings["smtpPassword"];
            string smtpFrom = ConfigurationManager.AppSettings["SmtpFrom"];

            try
            {
                SmtpClient smtpClient = new SmtpClient(smtpServer, smtpPort);

                // set smtp-client with basicAuthentication
                smtpClient.UseDefaultCredentials = false;
                NetworkCredential basicAuthenticationInfo = new NetworkCredential(smtpUsername, smtpPassword);
                smtpClient.Credentials = basicAuthenticationInfo;

                // add from,to mailaddresses
                MailAddress from = new MailAddress(smtpFrom, "Conger's Heating & Cooling");
                MailAddress to = new MailAddress(recipients[0]);
                MailMessage message = new MailMessage(from, to);

                for (int i = 1; i < recipients.GetUpperBound(0); i++)
                {
                    message.To.Add(new MailAddress(recipients[i]));
                }

                // add ReplyTo
                //MailAddress replyto = new MailAddress("reply@example.com");
                //message.ReplyTo = replyto;

                // set subject and encoding
                message.Subject = subject;
                message.SubjectEncoding = System.Text.Encoding.UTF8;

                // set body-message and encoding
                message.Body = messageBody;
                message.BodyEncoding = System.Text.Encoding.UTF8;
                // text or html
                message.IsBodyHtml = isHtml;

                if (isHighPriority)
                {
                    message.Priority = MailPriority.High;
                }

                smtpClient.Send(message);
            }

            catch (SmtpException ex)
            {
                throw new ApplicationException
                  ("SmtpException has occured: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
