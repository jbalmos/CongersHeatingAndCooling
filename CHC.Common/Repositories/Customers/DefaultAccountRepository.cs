using System;
using System.Linq;
using CHC.Entities.Customers;

namespace CHC.Common.Repositories.Customers
{
	public class DefaultAccountRepository : AbstractDbRepository<Account>, IAccountRepository
	{
		public DefaultAccountRepository(IDbContextFactory factory)
		 : base(factory)
		{
		}

		public Account Login(string login, string password, string IPAddress)
		{
			var account = this.dbContext.Set<Account>().FirstOrDefault(x =>
			  x.Login.Equals(login, StringComparison.InvariantCultureIgnoreCase) &&
			  x.Password.Equals(password, StringComparison.InvariantCultureIgnoreCase));

			if (account == null) return null;

			account.LastLoginDtm = DateTime.Now;
			account.LastIPAddress = IPAddress;

			this.Update(account);

			return account;
		}

		public bool ResetPassword(int accountID, string oldPassword, string newPassword)
		{
			var account = this.Get(accountID);
			return ResetPassword(account, oldPassword, newPassword);
		}

		public bool ResetPassword(string login, string oldPassword, string newPassword)
		{
			var account = this.dbContext.Set<Account>().Where(a =>
				a.Login.Equals(login) &&
				!string.IsNullOrWhiteSpace(login) &&
				a.Password.Equals(oldPassword.Trim()) &&
				!string.IsNullOrWhiteSpace(oldPassword)
			).FirstOrDefault();
			if (account == null) return false;
			return ResetPassword(account, oldPassword, newPassword);
		}

		private bool ResetPassword(Account account, string oldPassword, string newPassword)
		{
			account.Password = newPassword;
			return Update(account);
		}
	}
}
