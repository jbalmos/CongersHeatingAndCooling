using System.Linq;

namespace CHC.Common.Repositories.Office
{
	public interface IOfficeRepository
	{
		Entities.Office.Office Get( int primaryKey );
		bool Update( Entities.Office.Office entity );
		bool Add( Entities.Office.Office entity );
		IQueryable<Entities.Office.Office> Query();
	}
}
