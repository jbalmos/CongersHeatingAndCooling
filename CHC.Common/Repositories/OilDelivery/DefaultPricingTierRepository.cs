using System.Linq;
using CHC.Entities.Services.OilDelivery;

namespace CHC.Common.Repositories.OilDelivery
{
    public class DefaultPricingTierRepository : AbstractDbRepository<PricingTier>, IPricingTierRepository
    {
        public DefaultPricingTierRepository(IDbContextFactory factory)
			: base( factory )
		{
        }

        public PricingTier Find(string zipCode)
        {
            return this.Query().Where(p => p.ServiceAreas.Any(a => a.Zip == zipCode)).FirstOrDefault();
        }

        public PricingTier GetDefault()
        {
            return this.Query().Where(p => !p.ServiceAreas.Any()).FirstOrDefault();
        }
    }
}
