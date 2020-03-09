using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiBackend.Model
{
    public enum PaymentType
    {
        Rent, Electricity, Water, Internet, Groceries, Other
    }

    public enum Frequency
    {
        OneOff, Weekly, Fortnightly, Monthly
    }

    public class Payment
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public bool Fixed { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public User Users { get; set; }
        public PaymentType PaymentType { get; set; }
        public Frequency Frequency { get; set; }
    }
}
