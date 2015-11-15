using System.Collections.Generic;
using CHC.Entities.Services.OilDelivery;

namespace CHC.Entities.Customers
{
    public class Address
    {
        public Address()
        {
            this.OilTanks = new HashSet<OilTank>();
            this.DeliveryRequests = new HashSet<DeliveryRequest>();
        }

        public int ID { get; set; }
        public int CustomerID { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set;  }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Phone { get; set; }

        // Navigation Properties
        public virtual Customer Customer { get; set; }
        public virtual ICollection<OilTank> OilTanks { get; set; }
        public virtual ICollection<DeliveryRequest> DeliveryRequests { get; set; }
    }
}
