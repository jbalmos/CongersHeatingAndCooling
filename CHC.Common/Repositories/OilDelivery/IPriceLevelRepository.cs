using CHC.Entities.Services.OilDelivery;
using System.Linq;

namespace CHC.Common.Repositories.OilDelivery
{
	public interface IPriceLevelRepository
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		bool Update(PriceLevel entity);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		bool Add(PriceLevel entity);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		bool Delete(PriceLevel entity);

		/// <summary>
		/// Gets the available service areas
		/// </summary>
		/// <returns>List of hosts</returns>
		IQueryable<PriceLevel> Query();
	}
}
