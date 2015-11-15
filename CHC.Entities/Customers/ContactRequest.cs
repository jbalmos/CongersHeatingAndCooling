using System;

namespace CHC.Entites.Customers
{
    public class ContactRequest
    {
        public int ContactRequestID { get; set; }
        public int CustomerID { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public DateTime RequestDtm { get; set; }
    }
}
