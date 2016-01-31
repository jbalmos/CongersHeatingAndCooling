using System.Linq;
using CHC.Entities.Services.OilDelivery;
using System.Collections.Generic;

namespace CHC.Common.Repositories.OilDelivery
{
	public class DefaultServiceAreaRepository : AbstractDbRepository<ServiceArea>, IServiceAreaRepository
	{
		public DefaultServiceAreaRepository(IDbContextFactory factory)
			 : base(factory)
		{
		}

		public bool Clear( int pricingTierID )
		{
			dbContext.Set<ServiceArea>().RemoveRange(dbContext.Set<ServiceArea>().Where(
				a => a.OilDeliveryPricingTierID == pricingTierID));
			dbContext.SaveChanges();
			return true;
		}

		public bool BatchAdd( IEnumerable<ServiceArea> serviceAreas )
		{
			dbContext.Set<ServiceArea>().AddRange(serviceAreas);
			dbContext.SaveChanges();
			return true;
		}
	}
}
