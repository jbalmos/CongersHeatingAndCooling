using System;

namespace CHC.Entities.Office
{ 
	public class OfficeHour
	{
		public int ID { get; set; }
		public int OfficeID { get; set; }
		public DayOfWeek DayOfWeek { get; set; }
		public TimeSpan? OpeningTime { get; set; }
		public TimeSpan? ClosingTime { get; set; }
	}
}
