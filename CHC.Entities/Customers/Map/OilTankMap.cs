using System.Data.Entity.ModelConfiguration;

namespace CHC.Entities.Customers.Map
{
    public class OilTankMap : EntityTypeConfiguration<OilTank>
    {
        public OilTankMap()
        {
            this.HasKey(obj => obj.ID);
            this.Property(obj => obj.ID).HasColumnName("CustomerOilTankID");
            this.Property(obj => obj.AddressID).HasColumnName("CustomerAddressID");
            this.ToTable("tblCustomerOilTank");
        }
    }
}
