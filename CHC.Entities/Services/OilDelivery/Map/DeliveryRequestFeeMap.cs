using System.Data.Entity.ModelConfiguration;

namespace CHC.Entities.Services.OilDelivery.Map
{
    public class DeliveryRequestFeeMap : EntityTypeConfiguration<DeliveryRequestFee>
    {
        public DeliveryRequestFeeMap()
        {
            this.HasKey(obj => obj.ID);
            this.Property(obj => obj.ID).HasColumnName("OilDeliveryRequestFeeID");
            this.Property(obj => obj.DeliveryRequestID).HasColumnName("OilDeliveryRequestID");
            this.HasRequired(l => l.DeliveryRequest).WithMany(p => p.DeliveryRequestFees).HasForeignKey(a => a.DeliveryRequestID);
            this.ToTable("tblOilDeliveryRequestFee");
        }
    }
}
