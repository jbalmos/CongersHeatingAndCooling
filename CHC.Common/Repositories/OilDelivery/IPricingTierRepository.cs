using CHC.Entities.Services.OilDelivery;
using System.Linq;

namespace CHC.Common.Repositories.OilDelivery
{
    public interface IPricingTierRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Update(PricingTier entity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Add(PricingTier entity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        PricingTier Get(int primaryKey);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        PricingTier Find(string zipCode);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        PricingTier GetDefault();

        /// <summary>
        /// Gets the available service areas
        /// </summary>
        /// <returns>List of hosts</returns>
        IQueryable<PricingTier> Query();
    }
}
