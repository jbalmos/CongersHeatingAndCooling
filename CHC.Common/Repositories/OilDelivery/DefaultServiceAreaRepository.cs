using CHC.Entities.Services.OilDelivery;

namespace CHC.Common.Repositories.OilDelivery
{
    public class DefaultServiceAreaRepository : AbstractDbRepository<ServiceArea>, IServiceAreaRepository
    {
        public DefaultServiceAreaRepository(IDbContextFactory factory)
            : base(factory)
        {
        }
    }
}
