using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebApiBackend.Interfaces;

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

    public class Payment : IEntity
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Amount of money set to be paid 
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// True if the payment is fixed, false if the payment is variable
        /// </summary>
        public bool Fixed { get; set; }

        /// <summary>
        /// Start date of a payment
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// End date of a payment
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// What the payment is for
        /// </summary>
        public PaymentType PaymentType { get; set; }

        /// <summary>
        /// How often the payment is repeated
        /// </summary>
        public Frequency Frequency { get; set; }

        /// <summary>
        /// Allows many-to-many association
        /// </summary>
        public ICollection<UserPayment> UserPayments { get; set; }

        /// <summary>
        /// Short description of a payment
        /// </summary>
        public String Description { get; set; }
    }
}
