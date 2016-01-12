using System;
using System.Collections.Generic;

namespace CHC.Entities.Services.OilDelivery
{
    public class PriceLevel
    {
        public PriceLevel()
        {
            this.Fees = new List<PriceLevelFee>();
        }

        public int ID { get; set; }
        public int OilDeliveryPricingTierID { get; set; }
        public int GallonRangeStart { get; set; }
        public int GallonRangeEnd { get; set; }
        public decimal PricePerGallon { get; set; }
        public virtual PricingTier PricingTier { get; set; }
        public virtual IList<PriceLevelFee> Fees { get; set; }
    }

    public static class PriceLevelExtensions
    {
        public static string FractionalHtmlFormattedPrice(this PriceLevel priceLevel)
        {
            string price = priceLevel.PricePerGallon.ToString();
            string priceLeft = price.Substring(0, (price.IndexOf(".") + 3));
            string priceRight = price.Length > 4 ? price.Substring(price.IndexOf(".") + 3, 1) : "0";

            return priceRight == "0" ? string.Format("{0:C}", priceLevel.PricePerGallon) : String.Format("${0}<sup>{1}</sup>&#8260;<sub>10</sub>", priceLeft, priceRight);
        }
    }
}
