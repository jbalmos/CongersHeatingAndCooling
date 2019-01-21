using System;

namespace CHC.Entities.Announcements
{
	public class Announcement
	{
		public int ID { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime? EndDate { get; set; }
		public bool IsAlert { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }

	}
}
