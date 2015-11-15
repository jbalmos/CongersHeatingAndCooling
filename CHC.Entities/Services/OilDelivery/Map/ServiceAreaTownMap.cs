using System.Data.Entity.ModelConfiguration;

namespace CHC.Entities.Services.OilDelivery.Map
{
    public class ServiceAreaTownMap : EntityTypeConfiguration<ServiceAreaTown>
    {
        public ServiceAreaTownMap()
        {
            this.HasKey(obj =>  obj.Zip );
            this.ToTable("tblServiceAreaTown");
        }
    }
}
