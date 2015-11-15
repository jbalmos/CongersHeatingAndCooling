using System.Linq;
using CHC.Entities.Services.OilDelivery;

namespace CHC.Common.Repositories.OilDelivery
{
    public interface IServiceAreaTownRepository
    {
        /// <summary>
        /// Gets the available DB hosts for a District
        /// </summary>
        /// <returns>List of hosts</returns>
        IQueryable<ServiceAreaTown> Query();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        ServiceAreaTown Find(string primaryKey);
    }
}
