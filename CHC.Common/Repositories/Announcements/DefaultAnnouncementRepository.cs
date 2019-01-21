using CHC.Entities.Announcements;

namespace CHC.Common.Repositories.Announcements
{
	public class DefaultAnnouncementRepository : AbstractDbRepository<Announcement>, IAnnouncementRepository
	{
		public DefaultAnnouncementRepository( IDbContextFactory factory )
		 : base(factory)
		{
		}
	}
}
