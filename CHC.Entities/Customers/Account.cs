using System;

namespace CHC.Entities.Customers
{
	public class Account
	{
		public int ID { get; set; }
		public string Login { get; set; }
		public string Password { get; set; }
		public DateTime CreatedDtm { get; set;  }
		public DateTime? LastLoginDtm { get; set; }
		public string LastIPAddress { get; set; }
		public AccountType Type { get; set; }

		public virtual Customer Customer { get; set; }
	}

	public enum AccountType
	{
		Customer,
		SysAdmin
	}
}
