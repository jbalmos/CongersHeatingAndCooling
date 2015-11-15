using System.Data.Entity.ModelConfiguration;

namespace CHC.Entities.Services.OilDelivery.Map
{
    public class PricingTierMap : EntityTypeConfiguration<PricingTier>
    {
        public PricingTierMap()
        {
            this.HasKey(obj => obj.ID);
            this.Property(obj => obj.ID).HasColumnName("OilDeliveryPricingTierID");
            this.HasMany(obj => obj.ServiceAreas).WithOptional().HasForeignKey(a => a.OilDeliveryPricingTierID);
            this.HasMany(obj => obj.PriceLevels).WithOptional().HasForeignKey(a => a.OilDeliveryPricingTierID);
            this.ToTable("tblOilDeliveryPricingTier");
        }
    }
}
