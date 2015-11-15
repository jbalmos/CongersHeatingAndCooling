
namespace CHC.Entities.Services.OilDelivery
{
    public class DeliveryRequestFee
    {
        public int ID { get; set; }
        public int DeliveryRequestID { get; set; }
        public decimal Fee { get; set; }
        public string Description { get; set; }

        public virtual DeliveryRequest DeliveryRequest { get; set; }
    }
}
