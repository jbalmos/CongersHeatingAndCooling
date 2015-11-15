using System.Linq;
using CHC.Entities.Customers;

namespace CHC.Common.Repositories.Customers
{
    public interface IFillPipeLocationRepository
    {
        /// <summary>
        /// Gets the available DB hosts for a District
        /// </summary>
        /// <returns>List of hosts</returns>
        IQueryable<FillPipeLocation> Query();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        FillPipeLocation Get(int primaryKey);
    }
}
