//using DataAnnotationsExtensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace CongerHeatingAndCooling.Models
{
	public class ContactForm
	{
		public string Subject
		{
			get
			{
				return string.Format( "{0} for {1} {2}", this.ContactPurpose, this.FirstName, this.LastName );
			}
		}

		[Required( ErrorMessage = "This field is required" )]
		[Display( Name = "First Name" )]
		public string FirstName { get; set; }

		[Required( ErrorMessage = "This field is required" )]
		[Display( Name = "Last Name" )]
		public string LastName { get; set; }

		[Required( ErrorMessage = "This field is required" )]
		//[Email]
		[DataType( DataType.EmailAddress )]
		[Display( Name = "Email Address" )]
		public string Email { get; set; }

		[DataType( DataType.PhoneNumber )]
		[Display( Name = "Phone Number" )]
		public string Phone { get; set; }

		[Required( ErrorMessage = "This field is required" )]
		[Display( Name = "Contact Purpose" )]
		public string ContactPurpose { get; set; }

		[Required( ErrorMessage = "This field is required" )]
		[Display( Name = "Additional Information" )]
		public string Comments { get; set; }

		public string RecaptchaTokenResponse { get; set; }

		public bool RecaptchaFailed { get; set; }

		public bool IsHighPriority { get; set; }
		public string EmailTemplatePath( string applicationPath )
		{
			return Path.Combine( applicationPath, @"App_Data\template-contact-us-email.txt" );
		}
		public string SmsTemplatePath( string applicationPath )
		{
			return Path.Combine( applicationPath, @"App_Data\template-contact-us-sms.txt" );
		}
		public string EmergencySmsTemplatePath( string applicationPath )
		{
			return Path.Combine( applicationPath, @"App_Data\template-contact-us-emergency-sms.txt" );
		}
	}
}