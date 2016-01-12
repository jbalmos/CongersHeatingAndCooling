using CHC.Entities.Customers;

namespace CHC.Common.Repositories.Customers
{
	public interface IAccountRepository
	{
		Account Login(string username, string password, string ipAddress);
		bool ResetPassword(int accountID, string oldPassword, string newPassword);
		bool ResetPassword(string login, string oldPassword, string newPassword);
	}
}
