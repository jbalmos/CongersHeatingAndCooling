using System.Data.Entity.ModelConfiguration;

namespace CHC.Entities.Announcements.Map
{
	public class AnnouncementMap : EntityTypeConfiguration<Announcement>
	{
		public AnnouncementMap()
		{
			this.HasKey( obj => obj.ID );
			this.Property( obj => obj.ID ).HasColumnName( "AnnouncementID" );

			this.ToTable( "tblAnnouncement" );
		}
	}
}
