using System;
using System.Collections.Generic;

namespace CongerHeatingAndCooling.Models
{
	public class NavigationModel
	{
		public NavigationModel()
		{
			Alerts = new List<Tuple<string, string>>();
			Announcements = new List<Tuple<string, string>>();
		}

		public string PricePerGallon { get; set; }
		public string MimimumGallons { get; set; }
		public List<Tuple<string,string>> Alerts {get;set;}
		public List<Tuple<string, string>> Announcements { get; set; }

	}
}