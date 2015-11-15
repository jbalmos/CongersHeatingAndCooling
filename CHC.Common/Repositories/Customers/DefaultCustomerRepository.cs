using CHC.Entities.Customers;

namespace CHC.Common.Repositories.Customers
{
    public class DefaultCustomerRepository : AbstractDbRepository<Customer>, ICustomerRepository
    {
        public DefaultCustomerRepository(IDbContextFactory factory)
			: base( factory )
		{
		}
    }
}
