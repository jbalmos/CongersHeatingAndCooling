using CHC.Entities.Customers;

namespace CHC.Common.Repositories.Customers
{
    public class DefaultFillPipeLocationRepository : AbstractDbRepository<FillPipeLocation>, IFillPipeLocationRepository
    {
        public DefaultFillPipeLocationRepository(IDbContextFactory factory)
			: base( factory )
		{
		}
    }
}
