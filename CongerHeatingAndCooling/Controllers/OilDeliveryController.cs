using System.Web.Mvc;
using System.Linq;
using CHC.Common.Repositories.Customers;
using CHC.Common.Repositories.OilDelivery;
using CongerHeatingAndCooling.Extensions;
using CongerHeatingAndCooling.Models;
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

namespace CongerHeatingAndCooling.Controllers
{
	public class OilDeliveryController : Controller
	{
		readonly IServiceAreaRepository serviceAreaRepo;
		readonly IServiceAreaTownRepository townRepo;
		readonly IPricingTierRepository pricingTierRepo;
		readonly IFillPipeLocationRepository oilTankLocationRepo;
		readonly ICustomerRepository customerRepo;
		readonly IDeliveryRequestRepository deliveryRequestRepo;

		public OilDeliveryController(
			 IServiceAreaRepository serviceAreaRepo,
			 IServiceAreaTownRepository townRepo,
			 IPricingTierRepository pricingTierRepo,
			 IFillPipeLocationRepository oilTankLocationRepo,
			 ICustomerRepository customerRepo,
			 IDeliveryRequestRepository deliveryRequestRepo)
		{
			this.serviceAreaRepo = serviceAreaRepo;
			this.pricingTierRepo = pricingTierRepo;
			this.oilTankLocationRepo = oilTankLocationRepo;
			this.townRepo = townRepo;
			this.customerRepo = customerRepo;
			this.deliveryRequestRepo = deliveryRequestRepo;
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
		public ActionResult Order()
		{
			OrderFormModel orderForm = new OrderFormModel
			{
				State = "MA",
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
			Customer customer = new Customer
			{
				FirstName = model.FirstName,
				LastName = model.LastName,
				Email = model.Email,
				Mobile = model.Mobile,
				IsFuelAssistanceCustomer = model.IsFuelAssistanceCustomer,
				IsSeniorCitizen = model.IsSeniorCitizen,
				IsUSMilitaryCustomer = model.IsUSMilitaryCustomer,
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
				RequiresBurnerPriming = model.RequiresBurnerPriming
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
			if (model.OilDeliveryPriceLevelID.HasValue)
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

			SendRequestNotification(model);

			if (deliveryRequestRepo.Add(request))
			{
				return Json(new { Success = true });
			}

			return Json(new { Success = false });
		}

		private void SendRequestNotification(OrderFormModel model)
		{
			var emailTemplate = Path.Combine(HttpContext.Request.PhysicalApplicationPath, @"App_Data\template-oil-delivery-submission-email.txt");
			var smsTemplate = Path.Combine(HttpContext.Request.PhysicalApplicationPath, @"App_Data\template-oil-delivery-submission-sms.txt");

			using (StreamReader streamReader = new StreamReader(emailTemplate, Encoding.ASCII))
			{
				emailTemplate = streamReader.ReadToEnd();
			}

			using (StreamReader streamReader = new StreamReader(smsTemplate, Encoding.ASCII))
			{
				smsTemplate = streamReader.ReadToEnd();
			}

			var formCollection = Request.Form;

			foreach (PropertyInfo prop in model.GetType().GetProperties())
			{
				if (prop.PropertyType.IsPrimitive || prop.PropertyType.Equals(typeof(string)) || prop.PropertyType.IsValueType)
				{
					var value = prop.GetValue(model, null) ?? string.Empty;
					emailTemplate = emailTemplate.Replace(string.Format(@"{{{0}}}", prop.Name), value.ToString());
					smsTemplate = smsTemplate.Replace(string.Format(@"{{{0}}}", prop.Name), value.ToString());
				}
			}

			string[] recipients = ConfigurationManager.AppSettings["SmtpTo"].Split(',');

			UtilitySmtp.SendSmsMessage("Oil Delivery Request", smsTemplate, true);
			UtilitySmtp.SendSmtpMessage(recipients, "Oil Delivery Request", emailTemplate, true, true);
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
