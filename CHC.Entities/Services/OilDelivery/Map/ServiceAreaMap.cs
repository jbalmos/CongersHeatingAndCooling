using System.Data.Entity.ModelConfiguration;

namespace CHC.Entities.Services.OilDelivery.Map
{
    public class ServiceAreaMap : EntityTypeConfiguration<ServiceArea>
    {
        public ServiceAreaMap()
        {
            this.HasKey(obj => new { obj.OilDeliveryPricingTierID, obj.Zip });
            //this.HasRequired(l => l.PricingTier).WithMany(p => p.ServiceAreas).HasForeignKey(a => a.OilDeliveryPricingTierID);
            this.HasRequired(l => l.Town).WithMany(p => p.ServiceAreas).HasForeignKey(a => a.Zip);
            this.ToTable("tblOilDeliveryServiceArea");
        }
    }
}
