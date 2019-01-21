using CHC.Entities.Office;

namespace CHC.Common.Repositories.Office
{
	public class DefaultOfficeHoursRepository : AbstractDbRepository<OfficeHour>, IOfficeHourRepository
	{
		public DefaultOfficeHoursRepository( IDbContextFactory factory ) : base(factory)
		{
		}
	}
}
