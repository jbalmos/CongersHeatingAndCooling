using CHC.Common.Repositories.OilDelivery;
using CongerHeatingAndCooling.Models.Manage;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CongerHeatingAndCooling.Controllers
{
    public class ManageController : Controller
    {
        readonly IServiceAreaTownRepository serviceAreaTownRepo;
        readonly IPricingTierRepository pricingTierRepo;

        public ManageController(IServiceAreaTownRepository serviceAreaTownRepo,
            IPricingTierRepository pricingTierRepo)
        {
            this.serviceAreaTownRepo = serviceAreaTownRepo;
            this.pricingTierRepo = pricingTierRepo;
        }

        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (model.Username == "jeremy" && model.Password == "password")
            {
                Session["IsLoggedIn"] = "True";
                return RedirectToAction("Pricing");
            }
            return View("Pricing", model);
        }

        [HttpPost]
        public ActionResult Pricing(PricingTierModel model)
        {
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
	}
}