using CHC.Entities.Services.OilDelivery;

namespace CHC.Common.Repositories.OilDelivery
{
    public interface IDeliveryRequestRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Add(DeliveryRequest entity);
    }
}
