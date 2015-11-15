using CongerHeatingAndCooling.Models;
using CongerHeatingAndCooling.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace CongerHeatingAndCooling.Controllers
{
    public class SupportController : Controller
    {
        public SupportController()
        {
        }
        
        public ActionResult ContactUs()
        {
            return View(new ContactForm());
        }
        
        [HttpPost]
        public ActionResult ContactUs(ContactForm contactForm)
        {
            if (ModelState.IsValid)
            {
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

                    var formCollection = Request.Form;

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
            else
            {
                return View();
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
