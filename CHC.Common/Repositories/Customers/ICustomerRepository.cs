using CHC.Entities.Customers;

namespace CHC.Common.Repositories.Customers
{
    public interface ICustomerRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Update(Customer entity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool Add(Customer entity);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Customer Get(int primaryKey);
    }
}
