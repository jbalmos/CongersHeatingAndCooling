using CHC.Entities.Services.OilDelivery;

namespace CHC.Common.Repositories.OilDelivery
{
    public class DefaultDeliveryRequestRepository : AbstractDbRepository<DeliveryRequest>, IDeliveryRequestRepository
    {
        public DefaultDeliveryRequestRepository(IDbContextFactory factory)
            : base(factory)
        {
        }
    }
}
