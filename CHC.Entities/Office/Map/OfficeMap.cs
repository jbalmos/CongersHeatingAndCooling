using System.Data.Entity.ModelConfiguration;

namespace CHC.Entities.Office.Map
{
	public class OfficeMap : EntityTypeConfiguration<Office>
	{
		public OfficeMap()
		{
			this.HasKey( obj => obj.ID );
			this.Property( obj => obj.ID ).HasColumnName( "OfficeID" );
			this.HasMany( obj => obj.OfficeHours ).WithOptional().HasForeignKey( t => t.OfficeID );
			this.ToTable( "tblOffice" );
		}
	}
}
