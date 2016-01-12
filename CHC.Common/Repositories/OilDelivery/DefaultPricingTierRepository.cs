using System.Linq;
using CHC.Entities.Services.OilDelivery;
using System.Data.Entity;

namespace CHC.Common.Repositories.OilDelivery
{
	public class DefaultPricingTierRepository : AbstractDbRepository<PricingTier>, IPricingTierRepository
	{
		public DefaultPricingTierRepository(IDbContextFactory factory)
		 : base(factory)
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

		public void DeletePriceLevel(PriceLevel priceLevel)
		{
			this.Delete(priceLevel);
		}

		private bool Delete<T>(T entity) where T : PriceLevel
		{
			Guard.NotNull(entity, "entity");

			foreach (PriceLevelFee fee in entity.Fees.ToList())
			{
				var child = this.dbContext.Entry(fee);
				child.State = EntityState.Deleted;
			}

			var entry = this.dbContext.Entry(entity);
			entry.State = EntityState.Deleted;
			this.dbContext.SaveChanges();

			return true;
		}
	}
}
