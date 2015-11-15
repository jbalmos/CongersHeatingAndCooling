using System.Collections.Generic;
using CHC.Entities.Services.OilDelivery;

namespace CongerHeatingAndCooling.Models.Manage
{
    public class PricingTierModel
    {
        public PricingTierModel()
        {
            this.ServiceAreas = new List<ServiceArea>();
            this.PriceLevels = new List<PriceLevel>();
        }

        public IList<ServiceArea> ServiceAreas { get; set; }
        public IList<PriceLevel> PriceLevels { get; set; }
        public PricingTier PricingTier { get; set; }
    }
}