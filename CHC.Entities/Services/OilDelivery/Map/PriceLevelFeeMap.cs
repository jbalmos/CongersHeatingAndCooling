using System.Data.Entity.ModelConfiguration;

namespace CHC.Entities.Services.OilDelivery.Map
{
    public class PriceLevelFeeMap : EntityTypeConfiguration<PriceLevelFee>
    {
        public PriceLevelFeeMap()
        {
            this.HasKey(obj => obj.ID);
            this.Property(obj => obj.ID).HasColumnName("OilDeliveryPriceLevelFeeID");
            this.HasRequired(l => l.PriceLevel).WithMany(p => p.Fees).HasForeignKey(a => a.OilDeliveryPriceLevelID).WillCascadeOnDelete(true);
            this.ToTable("tblOilDeliveryPriceLevelFee");
        }
    }
}
