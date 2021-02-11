using System;
using System.Configuration;
using System.Net;
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

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
				var message = new MimeMessage();
				message.From.Add(new MailboxAddress("Conger's Heating & Cooling, Inc", smtpFrom));
				message.To.Add(new MailboxAddress("", recipients[0]));
				message.Subject = subject;
				message.Body = new TextPart(isHtml ? "html" : "plain" ) { Text = messageBody };

				for (int i = 1; i <= recipients.GetUpperBound(0); i++)
				{
					message.To.Add(new MailboxAddress("", recipients[i]));
				}
				if (isHighPriority)
				{
					message.Priority = MessagePriority.Urgent;
				}

				using (var client = new SmtpClient())
				{
					client.Connect(smtpServer, smtpPort, SecureSocketOptions.StartTls);
					client.Authenticate(smtpUsername, smtpPassword);

					client.Send(message);
					client.Disconnect(true);
				}
			}

			catch (Exception ex)
			{
				//throw ex;
			}
		}
	}
}
