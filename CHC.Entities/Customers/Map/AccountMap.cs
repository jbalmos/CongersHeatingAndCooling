using System.Data.Entity.ModelConfiguration;

namespace CHC.Entities.Customers.Map
{
	public class AccountMap : EntityTypeConfiguration<Account>
	{
		public AccountMap()
		{
			this.HasKey(obj => obj.ID);
			this.Property(obj => obj.ID).HasColumnName("AccountID");
			
			this.ToTable("tblAccount");
		}
	}
}
