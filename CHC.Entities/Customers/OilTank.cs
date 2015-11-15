namespace CHC.Entities.Customers
{
    public class OilTank
    {
        public int ID { get; set; }
        public int AddressID { get; set; }
        public string FillPipeLocation { get; set; }
        public bool IsIndoor { get; set; }
        public int Size { get; set; }

        public virtual Address Address { get; set; }
    }
}
