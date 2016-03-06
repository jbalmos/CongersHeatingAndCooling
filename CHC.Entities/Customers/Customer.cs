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
		public bool IsUSMilitaryCustomer { get; set; }
		public bool IsSeniorCitizen { get; set; }
		public bool IsFuelAssistanceCustomer { get; set; }
		public bool IsEmergencyPersonnel { get; set; }

		public virtual Account Account { get; set; }
		public virtual ICollection<Address> Addresses { get; set; }
	}
}
