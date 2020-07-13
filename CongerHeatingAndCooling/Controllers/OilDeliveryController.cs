using System.Web.Mvc;
using System.Linq;
using CHC.Common.Repositories.Customers;
using CHC.Common.Repositories.OilDelivery;
using CongerHeatingAndCooling.Extensions;
using CongerHeatingAndCooling.Models.OilDelivery;
using System;
using CHC.Entities.Services.OilDelivery;
using CHC.Entities.Customers;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Configuration;
using CongerHeatingAndCooling.Utilities;
using System.Reflection;
using log4net;
using CHC.Common.Repositories.Office;
using System.Data.Entity;

namespace CongerHeatingAndCooling.Controllers
{
	public class OilDeliveryController : Controller
	{
		private static readonly ILog log = LogManager.GetLogger( typeof( OilDeliveryController ) );

		readonly IServiceAreaRepository serviceAreaRepo;
		readonly IServiceAreaTownRepository townRepo;
		readonly IPricingTierRepository pricingTierRepo;
		readonly IFillPipeLocationRepository oilTankLocationRepo;
		readonly ICustomerRepository customerRepo;
		readonly IDeliveryRequestRepository deliveryRequestRepo;
		readonly IOfficeRepository officeRepo;

		public OilDeliveryController(
			 IServiceAreaRepository serviceAreaRepo,
			 IServiceAreaTownRepository townRepo,
			 IPricingTierRepository pricingTierRepo,
			 IFillPipeLocationRepository oilTankLocationRepo,
			 ICustomerRepository customerRepo,
			 IDeliveryRequestRepository deliveryRequestRepo,
			 IOfficeRepository officeRepo )
		{
			this.serviceAreaRepo = serviceAreaRepo;
			this.pricingTierRepo = pricingTierRepo;
			this.oilTankLocationRepo = oilTankLocationRepo;
			this.townRepo = townRepo;
			this.customerRepo = customerRepo;
			this.deliveryRequestRepo = deliveryRequestRepo;
			this.officeRepo = officeRepo;
		}

		public ActionResult ServiceArea()
		{
			ServiceAreaModel model = new ServiceAreaModel
			{
				ServiceAreas = serviceAreaRepo.Query().OrderBy(a => a.Town.Name).Select(a => a.Town.Name).ToList()
			};
			return View(model);
		}

		[HttpGet]
		[OutputCacheAttribute( VaryByParam = "*", Duration = 0, NoStore = true )]
		public ActionResult Redirect()
		{
			return View( "Redirect" );
		}

		[HttpGet]
		[OutputCacheAttribute( VaryByParam = "*", Duration = 0, NoStore = true )]
		public ActionResult NewCustomer()
		{
			return View( "Redirect", ( object)"https://www.congersheatingandcoolingte.com/NCMM93YDL82/" );
		}

		[HttpGet]
		[OutputCacheAttribute( VaryByParam = "*", Duration = 0, NoStore = true )]
		public ActionResult ExistingCustomer()
		{
			return View( "Redirect", (object)"https://www.congersheatingandcoolingte.com/CU8CPM629/Login.aspx?ReturnUrl=%2fCU8CPM629%2f" );
		}
		/*
		[HttpGet]
		public ActionResult Order()
		{
			var office = officeRepo.Query().Include( x => x.OfficeHours ).First();
			OrderFormModel orderForm = new OrderFormModel {
				State = "MA",
				Office = office,
				IsEmergencyRequest = false,
				FillerPipeLocations = oilTankLocationRepo.Query().ToList().Select(l => new SelectListItem
				{
					Value = l.ID.ToString(),
					Text = l.Description
				})
			};
			return View(orderForm);
		}

		[HttpPost]
		public ActionResult Order(OrderFormModel model)
		{
			var office = officeRepo.Query().Include( x => x.OfficeHours ).First();
			model.Office = office;

			log.Info( $"Oil Delivery Order: {model.FirstName} { model.LastName}: {model.Mobile}, {model.Email}: {model.EstimatedGallons} gal" );
			Customer customer = new Customer
			{
				FirstName = model.FirstName,
				LastName = model.LastName,
				Email = model.Email,
				Mobile = model.Mobile,
				IsFuelAssistanceCustomer = model.IsFuelAssistanceCustomer,
				IsSeniorCitizen = model.IsSeniorCitizen,
				IsUSMilitaryCustomer = model.IsUSMilitaryCustomer,
				IsEmergencyPersonnel = model.IsEmergencyPersonnel,
            Addresses = new List<Address> {
						  new Address {
								Address1 = model.Address1,
								Address2 = model.Address2,
								City = model.City,
								State = model.State,
								Zip = model.Zip,
								Phone = model.Phone,
								OilTanks = new List<OilTank> {
									 new OilTank {
										  FillPipeLocation = model.FillerPipeLocation,
										  Size = model.TankSize
									 }
								}
						  }
					 }
			};

			customer.Addresses.First().Customer = customer;
			customer.Addresses.First().OilTanks.First().Address = customer.Addresses.First();

			PricingTier pricingTier = pricingTierRepo.Query().Where(t => t.PriceLevels.Any(l => l.ID == model.OilDeliveryPriceLevelID)).FirstOrDefault() ??
				 pricingTierRepo.GetDefault();
			PriceLevel price = null;


			var customerAddress = customer.Addresses.First();
			DeliveryRequest request = new DeliveryRequest
			{
				CustomerAddress = customerAddress,
				DateRequested = DateTime.Now,
				EstimatedGallons = model.EstimatedGallons,
				RequiresBurnerPriming = model.RequiresBurnerPriming,
				isFillUp = model.isFillUp
			};
			if (model.RequiresBurnerPriming)
			{
				request.DeliveryRequestFees.Add(new DeliveryRequestFee
				{
					Fee = pricingTier.BurnerPrimingFee,
					Description = "Burner Priming",
					DeliveryRequest = request
				});
			}
			if (model.OilDeliveryPriceLevelID.HasValue && model.OilDeliveryPriceLevelID > 0)
			{
				price = pricingTier.PriceLevels.Where(l => l.ID == model.OilDeliveryPriceLevelID).First();
				request.PricePerGallon = price.PricePerGallon;
				model.QuotedPricePerGallon = price.PricePerGallon;
				if (price.Fees.Count() > 0)
				{
					price.Fees.ToList().ForEach((fee) =>
					{
						request.DeliveryRequestFees.Add(new DeliveryRequestFee
						{
							Fee = fee.Fee,
							Description = fee.Description,
							DeliveryRequest = request
						});
					});
				}
			}

			SendRequestNotification(model, pricingTier);

			if (deliveryRequestRepo.Add(request))
			{
				return Json(new { Success = true });
			}

			return Json(new { Success = false });
		}
		*/
		private void SendRequestNotification(OrderFormModel model, PricingTier pricingTier)
		{
			var emailTemplate = Path.Combine(HttpContext.Request.PhysicalApplicationPath, @"App_Data\template-oil-delivery-submission-email.txt");
			var customerTemplate = Path.Combine(HttpContext.Request.PhysicalApplicationPath, @"App_Data\template-oil-delivery-submission-customer-email.txt");
			var smsTemplate = Path.Combine(HttpContext.Request.PhysicalApplicationPath, @"App_Data\template-oil-delivery-submission-sms.txt");

			using (StreamReader streamReader = new StreamReader(emailTemplate, Encoding.ASCII))
			{
				emailTemplate = streamReader.ReadToEnd();
			}

			using (StreamReader streamReader = new StreamReader(smsTemplate, Encoding.ASCII))
			{
				smsTemplate = streamReader.ReadToEnd();
			}

			using (StreamReader streamReader = new StreamReader(customerTemplate, Encoding.ASCII))
			{
				customerTemplate = streamReader.ReadToEnd();
			}

			var formCollection = Request.Form;

			foreach (PropertyInfo prop in model.GetType().GetProperties())
			{
				if (prop.PropertyType.IsPrimitive || prop.PropertyType.Equals(typeof(string)) || prop.PropertyType.IsValueType)
				{
					var value = prop.GetValue(model, null) ?? string.Empty;
					emailTemplate = emailTemplate.Replace(string.Format(@"{{{0}}}", prop.Name), value.ToString());
					customerTemplate = customerTemplate.Replace(string.Format(@"{{{0}}}", prop.Name), value.ToString());
					smsTemplate = smsTemplate.Replace(string.Format(@"{{{0}}}", prop.Name), value.ToString());
				}
			}

			customerTemplate = customerTemplate.Replace(@"{BurnerPrimingFee}", GetBurnerPrimingFee( model, pricingTier ));
			customerTemplate = customerTemplate.Replace("{EstimatedPrice}", GetEstimateTotal(model, pricingTier));
			customerTemplate = GetDisclaimers(customerTemplate, model);
			customerTemplate = customerTemplate.Replace( "{EmailBody}", GetEmailBody( model ) );
			customerTemplate = customerTemplate.Replace( "{EmergencyRequest}",model.IsEmergencyRequest ? "<h2>EMERGENCY REQUEST</h2>" : "" );
			emailTemplate = GetDisclaimers( emailTemplate, model );
			emailTemplate = emailTemplate.Replace( "{EmergencyRequest}", model.IsEmergencyRequest ? "<h2>EMERGENCY PRIORITY</h2>" : "" );

			string[] recipients = ConfigurationManager.AppSettings["SmtpTo"].Split(',');

			UtilitySmtp.SendSmsMessage("Oil Delivery Request", smsTemplate, true);
			UtilitySmtp.SendSmtpMessage(recipients, "Oil Delivery Request", emailTemplate, true, true);
			UtilitySmtp.SendSmtpMessage(new[] { model.Email }, "Conger's Heating & Cooling, Inc: Your Oil Delivery Estimate Confirmation", customerTemplate, true, false);
		}

		private string GetEmailBody( OrderFormModel model )
		{
			if(model.IsOfficeClosed()) {
				if( model.IsEmergencyRequest) {
					return "Our office is currently closed. Please contact us via our emergency number 978.870.4945 as soon as possible and leave a voice message if this is an emergency.";
				}
				return "Our office is currently closed at this time so your order will be processed on the next business day. If this is an emergency please contact us via our emergency number as soon as possible.";
			}
			return "We have received your order and will begin processing it immediately. We will contact you to confirm the appointment delivery time and location.";
		}

		private string GetDisclaimers( string customerTemplate, OrderFormModel model)
		{
			customerTemplate = customerTemplate.Replace("{Disclaimer}", (model.IsUSMilitaryCustomer || model.IsSeniorCitizen || model.IsEmergencyPersonnel ? "**" : string.Empty));

			string disclaimerText = string.Empty;

			if(model.isFillUp ) {
				customerTemplate = customerTemplate.Replace( "{DeliveryFillType}", @"<tr><td colspan=""2""><h3>Fill Tank To Full*</h3></td></tr>" );
            disclaimerText = "<p>*You have requested to have your tank filled to full, which means we may deliver more than the estimated amount if the estimate does not reflect the actual ammount remaining in your oil tank.</p>";
         } else {
				customerTemplate = customerTemplate.Replace( "{DeliveryFillType}", @"<tr><td colspan=""2""><h3>Deliver Exact Amount From Estimate Below</h3></td></tr>" );
			}

			disclaimerText = disclaimerText + ( model.IsUSMilitaryCustomer || model.IsSeniorCitizen ? @"<p>** Additional discounts have been indicated but not applied to this estimate. These discounts are subject to verification to qualify and will be applied to the final bill at time of delivery.</p>" : string.Empty);

			customerTemplate = customerTemplate.Replace("{DisclaimerText}", disclaimerText );

			return customerTemplate;
      }

		private string GetBurnerPrimingFee(OrderFormModel model, PricingTier pricingTier)
		{
			return model.RequiresBurnerPriming && pricingTier != null ?
				string.Format(@"
				<tr>
					<td><b>Burner Priming Fee:</b></td>
					<td>{0:C}</td>
				</tr>", pricingTier.BurnerPrimingFee)
				: "";
      }

		private string GetEstimateTotal(OrderFormModel model, PricingTier pricingTier )
		{
			decimal price = model.EstimatedGallons * model.QuotedPricePerGallon;
			if (model.RequiresBurnerPriming && pricingTier != null)
			{
				price += pricingTier.BurnerPrimingFee;
         }
			return string.Format("{0:C}", price);
		}

		public ActionResult GetPricing(string zipCode)
		{
			var pricingTier = pricingTierRepo.Find(zipCode);
			PricingModel model;

			if (pricingTier == null)
			{
				pricingTier = pricingTierRepo.GetDefault();
				var town = townRepo.Find(zipCode) ?? new ServiceAreaTown();

				model = new PricingModel
				{
					PricingLevelJson = pricingTier.PriceLevels.ToJson(true),
					BurnerPrimingFee = pricingTier.BurnerPrimingFee,
					ZipCode = zipCode,
					Town = town.Name,
					MinGallons = 50,
					MaxGallons = 750,
					ShowPricing = false
				};
			}
			else
			{
				var town = pricingTier.ServiceAreas.Where(s => s.Zip == zipCode).Select(a => a.Town.Name).FirstOrDefault();

				model = new PricingModel
				{
					PricingLevelJson = pricingTier.PriceLevels.ToJson(true),
					BurnerPrimingFee = pricingTier.BurnerPrimingFee,
					ZipCode = zipCode,
					Town = town,
					MinGallons = pricingTier.PriceLevels.OrderBy(l => l.GallonRangeStart).First().GallonRangeStart,
					MaxGallons = pricingTier.PriceLevels.OrderByDescending(l => l.GallonRangeEnd).First().GallonRangeEnd,
					ShowPricing = true
				};
			}

			return View("_PricingPanel", model);
		}
	}
}
