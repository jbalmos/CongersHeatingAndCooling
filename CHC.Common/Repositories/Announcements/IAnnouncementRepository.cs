using System.Linq;
using CHC.Entities.Announcements;

namespace CHC.Common.Repositories.Announcements
{
	public interface IAnnouncementRepository
	{
		Announcement Get( int primaryKey );
		bool Update( Announcement entity );
		bool Add( Announcement entity );
		bool Delete( Announcement entity );
		//bool BatchAdd( IEnumerable<Announcement> announcements );
		IQueryable<Announcement> Query();
	}
}
