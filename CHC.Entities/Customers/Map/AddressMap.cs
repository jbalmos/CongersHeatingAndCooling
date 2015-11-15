using System.Data.Entity.ModelConfiguration;

namespace CHC.Entities.Customers.Map
{
    public class AddressMap : EntityTypeConfiguration<Address>
    {
        public AddressMap()
        {
            this.HasKey(obj => obj.ID);
            this.Property(obj => obj.ID).HasColumnName("CustomerAddressID");
            this.HasMany(obj => obj.OilTanks).WithOptional().HasForeignKey( t => t.AddressID );
            this.HasMany(obj => obj.DeliveryRequests).WithOptional().HasForeignKey( t => t.CustomerAddressID );
            this.ToTable("tblCustomerAddress");
        }
    }
}
