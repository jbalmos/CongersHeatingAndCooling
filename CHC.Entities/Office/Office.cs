using System.Collections.Generic;

namespace CHC.Entities.Office
{
	public class Office
	{
		public Office()
		{
			OfficeHours = new List<OfficeHour>();
		}

		public int ID { get; set; }
		public string Name { get; set; }
		public string Phone { get; set; }
		public bool IsClosed { get; set; }

		public IList<OfficeHour> OfficeHours { get; set; }
	}
}
