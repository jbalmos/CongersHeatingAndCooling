using CHC.Entities.Services.OilDelivery;
using System.Collections.Generic;
using System.Linq;

namespace CHC.Common.Repositories.OilDelivery
{
	public interface IServiceAreaRepository
	{
		/// <summary>
		/// Gets the available service areas
		/// </summary>
		/// <returns>List of hosts</returns>
		IQueryable<ServiceArea> Query();

		/// <summary>
		/// 
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		ServiceArea Get(int primaryKey);

		bool BatchAdd(IEnumerable<ServiceArea> serviceAreas);

		bool Clear(int pricingTierID);
	}
}
