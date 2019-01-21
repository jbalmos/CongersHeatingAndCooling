using System.Data.Entity.ModelConfiguration;

namespace CHC.Entities.Office.Map
{
	public class OfficeHourMap : EntityTypeConfiguration<OfficeHour>
	{
		public OfficeHourMap()
		{
			this.HasKey( obj => obj.ID );
			this.Property( obj => obj.ID ).HasColumnName( "OfficeHourID" );

			this.ToTable( "tblOfficeHour" );
		}
	}
}
