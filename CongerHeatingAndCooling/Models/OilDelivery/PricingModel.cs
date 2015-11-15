
namespace CongerHeatingAndCooling.Models.OilDelivery
{
    public class PricingModel
    {
        public string PricingLevelJson { get; set; }
        public decimal BurnerPrimingFee { get; set; }
        public string ZipCode { get; set; }
        public string Town { get; set; }
        public int MinGallons { get; set; }
        public int MaxGallons { get; set; }
        public bool ShowPricing { get; set; }
    }
}