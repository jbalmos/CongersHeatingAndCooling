namespace CHC.Entities.Services.OilDelivery
{
    public class PriceLevelFee
    {
        public int ID { get; set; }
        public int OilDeliveryPriceLevelID { get; set; }
        public decimal Fee { get; set; }
        public string Description { get; set; }

        public virtual PriceLevel PriceLevel { get; set; }
    }
}
