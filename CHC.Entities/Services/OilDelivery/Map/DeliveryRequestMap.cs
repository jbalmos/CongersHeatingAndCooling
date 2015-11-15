using System.Data.Entity.ModelConfiguration;

namespace CHC.Entities.Services.OilDelivery.Map
{
    public class DeliveryRequestMap : EntityTypeConfiguration<DeliveryRequest>
    {
        public DeliveryRequestMap()
        {
            this.HasKey(obj => obj.ID);
            this.Property(obj => obj.ID).HasColumnName("OilDeliveryRequestID");
            this.ToTable("tblOilDeliveryRequest");
        }
    }
}
