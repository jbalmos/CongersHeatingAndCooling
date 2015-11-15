using System.Linq;
using CHC.Entities.Services.OilDelivery;

namespace CHC.Common.Repositories.OilDelivery
{
    public class DefaultServiceAreaTownRepository : AbstractDbRepository<ServiceAreaTown>, IServiceAreaTownRepository
    {
        public DefaultServiceAreaTownRepository(IDbContextFactory factory)
            : base(factory)
        {
        }

        public ServiceAreaTown Find(string primaryKey)
        {
            return this.Query().Where(t => t.Zip == primaryKey).FirstOrDefault();
        }
    }
}
