using System.Collections.Generic;

namespace CHC.Entities.Services.OilDelivery
{
    public class PricingTier
    {
        public PricingTier()
        {
            this.ServiceAreas = new HashSet<ServiceArea>();
            this.PriceLevels = new HashSet<PriceLevel>();
        }

        public int ID { get; set; }
        public string Description { get; set; }
        public decimal CreditCardSurcharge { get; set; }
        public decimal BurnerPrimingFee { get; set; }
        public bool ShowPricing { get; set; }
        public virtual ICollection<ServiceArea> ServiceAreas { get; set; }
        public virtual ICollection<PriceLevel> PriceLevels { get; set; }
    }
}
