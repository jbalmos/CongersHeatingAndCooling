using CHC.Entites.Customers;
using System.Data.Entity.ModelConfiguration;

namespace CHC.Entities.Customers.Map
{
    public class ContactRequestMap : EntityTypeConfiguration<ContactRequest>
    {
        public ContactRequestMap()
		{
            this.HasKey(obj => obj.ContactRequestID);
            this.ToTable("tblContactRequest");
		}
    }
}
