using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Key]
        public int Id { get; set; }
        public int Amount { get; set; }
        public bool Fixed { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public PaymentType PaymentType { get; set; }
        public Frequency Frequency { get; set; }
        public ICollection<UserPayment> UserPayments { get; set; }
        public String Description { get; set; }
    }
}
