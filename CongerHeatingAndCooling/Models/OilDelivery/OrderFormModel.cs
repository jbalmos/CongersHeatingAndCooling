using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace CongerHeatingAndCooling.Models.OilDelivery
{
	public class OrderFormModel
	{
		public string Email { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Mobile { get; set; }
		public string Address1 { get; set; }
		public string Address2 { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string Zip { get; set; }
		public string Phone { get; set; }
		public string FillerPipeLocation { get; set; }
		public bool IsTankIndoors { get; set; }
		public int TankSize { get; set; }
		public int TankLevel { get; set; }
		public int? OilDeliveryPriceLevelID { get; set; }
		public bool RequiresBurnerPriming { get; set; }
		public int EstimatedGallons { get; set; }
		public decimal QuotedPricePerGallon { get; set; }
		public bool isFillUp { get; set; }
		public bool IsUSMilitaryCustomer { get; set; }
		public bool IsSeniorCitizen { get; set; }
		public bool IsFuelAssistanceCustomer { get; set; }
		public bool IsEmergencyPersonnel { get; set; }
		

		public IEnumerable<SelectListItem> FillerPipeLocations { get; set; }


	}
}