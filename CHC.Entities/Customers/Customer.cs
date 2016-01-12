using System.Collections.Generic;

namespace CHC.Entities.Customers
{
	public class Customer
	{
		public Customer()
		{
			this.Addresses = new HashSet<Address>();
		}

		public int ID { get; set; }
		public string Email { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Mobile { get; set; }

		public virtual Account Account { get; set; }
		public virtual ICollection<Address> Addresses { get; set; }
	}
}
