namespace CHC.Entities.Services.OilDelivery
{
    public class ServiceArea
    {
        public int OilDeliveryPricingTierID { get; set; }
        public string Zip { get; set; }

        //public PricingTier PricingTier { get; set; }
        public virtual ServiceAreaTown Town { get; set; }
    }
}
