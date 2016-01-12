using CHC.Entities.Services.OilDelivery;

namespace CHC.Common.Repositories.OilDelivery
{
	public class DefaultPriceLevelRepository : AbstractDbRepository<PriceLevel>, IPriceLevelRepository
	{
		public DefaultPriceLevelRepository(IDbContextFactory factory)
			: base( factory )
		{
		}
	}
}
