using CHC.Entities.Office;

namespace CHC.Common.Repositories.Office
{
	public class DefaultOfficeRepository :  AbstractDbRepository<Entities.Office.Office>, IOfficeRepository
	{
		public DefaultOfficeRepository( IDbContextFactory factory )
		 : base(factory)
		{
		}
	}
}
