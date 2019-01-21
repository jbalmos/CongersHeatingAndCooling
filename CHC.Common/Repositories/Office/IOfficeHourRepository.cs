using System.Linq;
using CHC.Entities.Office;

namespace CHC.Common.Repositories.Office
{
	public interface IOfficeHourRepository
	{
		OfficeHour Get( int primaryKey );
		bool Update( OfficeHour entity );
		bool Add( OfficeHour entity );
		IQueryable<OfficeHour> Query();
	}
}
