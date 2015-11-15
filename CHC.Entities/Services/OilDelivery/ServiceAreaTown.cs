using System.Collections.Generic;

namespace CHC.Entities.Services.OilDelivery
{
    public class ServiceAreaTown
    {
        public ServiceAreaTown()
        {
            this.ServiceAreas = new HashSet<ServiceArea>();
        }

        public string Zip { get; set; }
        public string Name { get; set; }

        public virtual ICollection<ServiceArea> ServiceAreas { get; set; }
    }
}
