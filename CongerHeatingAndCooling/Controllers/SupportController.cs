using CongerHeatingAndCooling.Models;
using CongerHeatingAndCooling.Utilities;
using System.Configuration;
using System.IO;
using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Net.Http;

namespace CongerHeatingAndCooling.Controllers
{
	public class SupportController : Controller
	{
		private readonly string _googleVerifyAddress = "https://www.google.com/recaptcha/api/siteverify";
		private readonly string _googleRecaptchaSecret = "6LcOzaEUAAAAAOzMnU4aC0SMyRs-ZCP8dSvI_sih";

		public SupportController()
		{
		}

		public ActionResult ContactUs()
		{
			return View( new ContactForm() );
		}

		private bool RecaptchaV3Passed(string token)
		{
			TokenResponse tokenResponse = new TokenResponse() {
				Success = false
			};

			using ( var client = HttpClientFactory.Create() ) {
				var response = client.GetStringAsync( $"{_googleVerifyAddress}?secret={_googleRecaptchaSecret}&response={token}" );
				tokenResponse = JsonConvert.DeserializeObject<TokenResponse>( response.Result );
				return tokenResponse.Success && tokenResponse.Score > 0.4m;
			}
		}

		
		[HttpPost]
		public ActionResult ContactUs( ContactForm contactForm )
		{
			contactForm.RecaptchaFailed = !RecaptchaV3Passed( contactForm.RecaptchaTokenResponse );
			if ( ModelState.IsValid && !contactForm.RecaptchaFailed ) {
				string emailTemplate;
				string smsTemplate;
				// check model state
				if ( contactForm.ContactPurpose.Equals( "Service Request" ) ) {
					TempData["ContactForm"] = contactForm;
					return RedirectToAction( "Services", "Company" );
				}
				else {
					using ( StreamReader streamReader = new StreamReader( contactForm.EmailTemplatePath( HttpContext.Request.PhysicalApplicationPath ), Encoding.ASCII ) ) {
						emailTemplate = streamReader.ReadToEnd();
					}

					if ( contactForm.ContactPurpose.Equals( "Emergency Service Request" ) ) {
						contactForm.IsHighPriority = true;
					}

					string filePath = (contactForm.IsHighPriority) ? contactForm.EmergencySmsTemplatePath( HttpContext.Request.PhysicalApplicationPath ) : contactForm.SmsTemplatePath( HttpContext.Request.PhysicalApplicationPath );
					using ( StreamReader streamReader = new StreamReader( filePath, Encoding.ASCII ) ) {
						smsTemplate = streamReader.ReadToEnd();
					}

					var formCollection = Request.Form;

					foreach ( string key in formCollection.AllKeys ) {
						emailTemplate = emailTemplate.Replace( string.Format( @"{{{0}}}", key ), formCollection[key] );
						smsTemplate = smsTemplate.Replace( string.Format( @"{{{0}}}", key ), formCollection[key] );
					}

					string[] recipients = ConfigurationManager.AppSettings["SmtpTo"].Split( ',' );

					UtilitySmtp.SendSmsMessage( contactForm.ContactPurpose, smsTemplate, contactForm.IsHighPriority );
					UtilitySmtp.SendSmtpMessage( recipients, contactForm.Subject, emailTemplate, false, contactForm.IsHighPriority );

					return RedirectToAction( "Success" );
				}
			}
			else {
				return View( contactForm );
			}
		}

		/*
		[HttpPost]
		public ActionResult ContactUs(FormCollection formCollection)
		{
			 ContactForm contactForm = _contactFormRepository.GetContactForm();

			 contactForm.ContactPurpose = formCollection["selectContactPurpose"];
			 contactForm.FirstName = formCollection["txtFirstName"];
			 contactForm.LastName =  formCollection["txtLastName"];
			 contactForm.Phone = formCollection["txtPhoneNumber"];
			 contactForm.Email = formCollection["txtEmail"];
			 contactForm.Comments = formCollection["txtAreaComments"];
			 contactForm.IsHighPriority = false;

			 string emailTemplate;
			 string smsTemplate;
			 // check model state
			 if (contactForm.ContactPurpose.Equals("Service Request"))
			 {
				  TempData["ContactForm"] = contactForm;
				  return RedirectToAction("Services", "Company");
			 }
			 else
			 {
				  using (StreamReader streamReader = new StreamReader(contactForm.EmailTemplatePath(HttpContext.Request.PhysicalApplicationPath), Encoding.ASCII))
				  {
						emailTemplate = streamReader.ReadToEnd();
				  }

				  if (contactForm.ContactPurpose.Equals("Emergency Service Request"))
				  {
						contactForm.IsHighPriority = true;
				  }

				  string filePath = (contactForm.IsHighPriority) ? contactForm.EmergencySmsTemplatePath(HttpContext.Request.PhysicalApplicationPath) : contactForm.SmsTemplatePath(HttpContext.Request.PhysicalApplicationPath);
				  using (StreamReader streamReader = new StreamReader(filePath, Encoding.ASCII))
				  {
						smsTemplate = streamReader.ReadToEnd();
				  }

				  foreach (string key in formCollection.AllKeys)
				  {
						emailTemplate = emailTemplate.Replace(string.Format(@"{{{0}}}", key), formCollection[key]);
						smsTemplate = smsTemplate.Replace(string.Format(@"{{{0}}}", key), formCollection[key]);
				  }

				  string[] recipients = ConfigurationManager.AppSettings["SmtpTo"].Split(',');

				  UtilitySmtp.SendSmsMessage(contactForm.ContactPurpose, smsTemplate, contactForm.IsHighPriority);
				  UtilitySmtp.SendSmtpMessage(recipients, contactForm.Subject, emailTemplate, false, contactForm.IsHighPriority);

				  return RedirectToAction("Success");
			 }
		}
		*/
		public ActionResult Success()
		{
			return View();
		}

	}
}
