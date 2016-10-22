using System;
using CHC.Entities.Customers;
using System.Collections.Generic;

namespace CHC.Entities.Services.OilDelivery
{
	public class DeliveryRequest
	{
		public DeliveryRequest()
		{
			this.DeliveryRequestFees = new HashSet<DeliveryRequestFee>();
		}

		public int ID { get; set; }
		public int CustomerAddressID { get; set; }
		public bool RequiresBurnerPriming { get; set; }
		public decimal PricePerGallon { get; set; }
		public int EstimatedGallons { get; set; }
		public DateTime DateRequested { get; set; }
		public bool isFillUp { get; set; }

		  public virtual Address CustomerAddress
		{ get; set; }
		public virtual ICollection<DeliveryRequestFee> DeliveryRequestFees { get; set; }
	}
}
