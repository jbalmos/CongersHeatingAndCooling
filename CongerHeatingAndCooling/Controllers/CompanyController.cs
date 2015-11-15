using CHC.Common.Repositories.OilDelivery;
using CongerHeatingAndCooling.Models;
using CongerHeatingAndCooling.Utilities;
using CHC.Entities.Services.OilDelivery;
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
    public class CompanyController : Controller
    {
        readonly IServiceAreaRepository serviceAreaRepo;
        readonly IPricingTierRepository pricingTierRepo;

        public CompanyController( IServiceAreaRepository serviceAreaRepo,
            IPricingTierRepository pricingTierRepo )
        {
            this.serviceAreaRepo = serviceAreaRepo;
            this.pricingTierRepo = pricingTierRepo;

        }
        //
        // GET: /Home/

        public ActionResult Home()
        {
            HomeModel model = new HomeModel
            {
                ServiceAreas = serviceAreaRepo.Query().OrderBy( a => a.Town.Name).Select(a => a.Town.Name).ToList()
            };
            return View(model);
        }

        public PartialViewResult Navigation()
        {
            var price = pricingTierRepo.Query().SelectMany(t => t.PriceLevels).OrderBy(l => l.PricePerGallon).First();
            
            NavigationModel model = new NavigationModel
            {
                PricePerGallon = price.FractionalHtmlFormattedPrice(),
                MimimumGallons = price.GallonRangeStart.ToString()
            };
            return PartialView("_Navigation", model);
        }

        //
        // GET: /Home/

        public ActionResult About()
        {
            return View();
        }

        //
        // GET: /Home/

        public ActionResult Products()
        {
            return View();
        }

        //
        // GET: /Home/

        public ActionResult Services()
        {
            ContactForm contactForm = (TempData["ContactForm"] == null) ? new ContactForm() : TempData["ContactForm"] as ContactForm;
            
            return View();
        }

        [HttpPost]
        public ActionResult Services(FormCollection formCollection)
        {
            string requestType = formCollection["rblRequestType"];
            string subject = string.Format("{0} for {1} {2}", requestType, formCollection["FirstName"], formCollection["LastName"]);
            string emailTemplate;
            string smsTemplate;
            bool isHighPriority = false;

            string filePath = Path.Combine(HttpContext.Request.PhysicalApplicationPath, @"App_Data\template-service-request-submission-email.txt");
            
            using (StreamReader streamReader = new StreamReader(filePath, Encoding.ASCII))
            {
                emailTemplate = streamReader.ReadToEnd();
            }

            if (requestType.Equals("Emergency Service Request"))
            {
                filePath = Path.Combine(HttpContext.Request.PhysicalApplicationPath, @"App_Data\template-emergency-service-request-submission-sms.txt");
                isHighPriority = true;
            }
            else
            {
                filePath = Path.Combine(HttpContext.Request.PhysicalApplicationPath, @"App_Data\template-service-request-submission-sms.txt");
            }

            using (StreamReader streamReader = new StreamReader(filePath, Encoding.ASCII))
            {
                smsTemplate = streamReader.ReadToEnd();
            }

            // Make constants
            string requestedServiceItemList = string.Empty;
            string prefixServiceItem = "rblServiceItem";

            foreach (string key in formCollection.AllKeys)
            {
                if (key.StartsWith(prefixServiceItem))
                {
                    requestedServiceItemList += string.Format("{0}\r\n", formCollection[key]);
                }
                else
                {
                    emailTemplate = emailTemplate.Replace(string.Format(@"{{{0}}}", key), formCollection[key]);
                    smsTemplate = smsTemplate.Replace(string.Format(@"{{{0}}}", key), formCollection[key]);
                }
            }

            emailTemplate = emailTemplate.Replace(string.Format(@"{{{0}}}", prefixServiceItem), requestedServiceItemList);

            string[] recipients = ConfigurationManager.AppSettings["SmtpTo"].Split(',');

            UtilitySmtp.SendSmsMessage(requestType, smsTemplate, isHighPriority);
            UtilitySmtp.SendSmtpMessage(recipients, subject, emailTemplate, false, isHighPriority);

            return RedirectToAction("Success");
        }

        //
        // GET: /Testimonials/

        public ActionResult Testimonials()
        {
            return View();
        }

        //
        // GET: /Success/

        public ActionResult Success()
        {
            return View();
        }

    }
}
