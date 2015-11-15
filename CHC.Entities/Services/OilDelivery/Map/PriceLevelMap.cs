using System.Data.Entity.ModelConfiguration;

namespace CHC.Entities.Services.OilDelivery.Map
{
    public class PriceLevelMap : EntityTypeConfiguration<PriceLevel>
    {
        public PriceLevelMap()
        {
            this.HasKey(obj => obj.ID);
            this.Property(obj => obj.ID).HasColumnName("OilDeliveryPriceLevelID");
            this.HasMany(obj => obj.Fees).WithOptional().HasForeignKey(a => a.OilDeliveryPriceLevelID);
            this.HasRequired(l => l.PricingTier).WithMany(p => p.PriceLevels).HasForeignKey(a => a.OilDeliveryPricingTierID);
            this.ToTable("tblOilDeliveryPriceLevel");
        }
    }
}
