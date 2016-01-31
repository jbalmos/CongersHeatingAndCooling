﻿using AutoMapper;
using CongerHeatingAndCooling.Models.Manage;
using CHC.Common.Extensions;
using System;
using System.Linq;
using System.Web.Mvc;
using CHC.Entities.Customers;
using CHC.Entities.Services.OilDelivery;
using CHC.Common.Repositories.Customers;
using CHC.Common.Repositories.OilDelivery;
using System.Data.Entity;
using System.Collections.Generic;

namespace CongerHeatingAndCooling.Controllers
{
	public class ManageController : Controller
	{
		public const int DefaultPricingTierID = 1;
		readonly IServiceAreaTownRepository serviceAreaTownRepo;
		readonly IServiceAreaRepository serviceAreaRepo;
		readonly IPricingTierRepository pricingTierRepo;
		readonly IAccountRepository accountRepo;

		public ManageController(IServiceAreaTownRepository serviceAreaTownRepo,
			IServiceAreaRepository serviceAreaRepo,
			IPricingTierRepository pricingTierRepo,
			IAccountRepository accountRepo)
		{
			this.serviceAreaTownRepo = serviceAreaTownRepo;
			this.serviceAreaRepo = serviceAreaRepo;
			this.pricingTierRepo = pricingTierRepo;
			this.accountRepo = accountRepo;
		}

		[HttpPost]
		public ActionResult Login(LoginModel model)
		{
			var account = accountRepo.Login(model.Username, model.Password, this.Request.UserHostAddress);

			if (account == null || account.Type != AccountType.SysAdmin)
			{
				Session["IsLoggedIn"] = null;
				Session["Account"] = null;
			}
			else
			{
				Session["IsLoggedIn"] = "True";
				Session["Account"] = account;
				return RedirectToAction("Pricing");
			}
			return View("Pricing");
		}

		public ActionResult Logout()
		{
			Session["IsLoggedIn"] = null;
			Session["Account"] = null;
			return View("Pricing");
		}

		[HttpPost]
		public JsonResult ChangePassword(int accountID, string oldPassword, string newPassword)
		{
			bool success = accountRepo.ResetPassword(accountID, oldPassword, newPassword);
			return Json(new { Success = success, ErrorMessage = success ? "" : "Unable to reset your password" });
		}

		[HttpPost]
		public ActionResult Pricing(PricingTierModel model)
		{
			var pricingTier = pricingTierRepo.Query().Where(s => s.ID == 1).First();

			model.PriceLevels.ToList().ForEach(l =>
			{
				l.Fees.RemoveAll<PriceLevelFee>(f => String.IsNullOrWhiteSpace(f.Description) || f.Fee == 0);
			});

			Mapper.CreateMap<PricingTier, PricingTier>()
				.ForMember(dest => dest.PriceLevels, opt => opt.Ignore())
				.ForMember(dest => dest.ServiceAreas, opt => opt.Ignore());

			Mapper.CreateMap<PriceLevel, PriceLevel>()
				.ForMember(dest => dest.PricingTier, opt => opt.Ignore())
				.ForMember(dest => dest.Fees, opt => opt.Ignore());

			Mapper.CreateMap<PriceLevelFee, PriceLevelFee>()
				.ForMember(dest => dest.PriceLevel, opt => opt.Ignore());

			Mapper.Map(model.PricingTier, pricingTier);
			model.PriceLevels.Each(l =>
			{
				PriceLevel level = pricingTier.PriceLevels.Where(pl => pl.ID == l.ID).FirstOrDefault();
				if (level != null)
				{
					Mapper.Map(l, level);
					level.Fees.Each(f =>
					{
						PriceLevelFee fee = l.Fees.Where(pf => pf.ID == f.ID).FirstOrDefault();
						if (fee != null)
						{
							Mapper.Map(fee, f);
						}
					});
				}
				else
				{
					pricingTier.PriceLevels.Add(l);
				}
			});

			pricingTierRepo.Update(pricingTier);
			model.ServiceAreas = pricingTier.ServiceAreas.ToList();
			var deletedPrices = pricingTier.PriceLevels.Where(l => !model.PriceLevels.Any(p => p.ID == l.ID)).ToList();
			deletedPrices.Each(p =>
			{
				pricingTierRepo.DeletePriceLevel(p);
			});

			return View(model);
		}

		public ActionResult Pricing()
		{
			var pricingTier = pricingTierRepo.Query().Where(s => s.ID == 1).First();
			var model = new PricingTierModel
			{
				PricingTier = pricingTier,
				ServiceAreas = pricingTier.ServiceAreas.ToList(),
				PriceLevels = pricingTier.PriceLevels.ToList()
			};
			return View(model);
		}

		public ActionResult ServiceArea()
		{
			var serviceArea = serviceAreaTownRepo.Query().Include(a => a.ServiceAreas).Select(
				t => new TownModel
				{
					Name = t.Name,
					Active = t.ServiceAreas.Count() > 0
				}).Distinct().ToList();

			return View(serviceArea);
		}

		[HttpPost]
		public ActionResult ServiceArea(List<TownModel> towns)
		{
			var selectedTownNames = towns.Where(t => t.Active).Select(t => t.Name);
			var serviceAreaTowns = serviceAreaTownRepo.Query().Where(t => selectedTownNames.Contains(t.Name)).ToList().Select
				(s => new ServiceArea
				{
					OilDeliveryPricingTierID = DefaultPricingTierID,
					Zip = s.Zip
				});

			serviceAreaRepo.Clear(DefaultPricingTierID);
			serviceAreaRepo.BatchAdd(serviceAreaTowns);

			return View(towns);
		}
	}
}