using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using WebApiBackend.Interfaces;


namespace WebApiBackend.Model
{
    public class User : IEntity
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Username that the user registers/logs in with
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// First name of a user
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Last name of a user
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// DoB of a user
        /// </summary>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Phone number of a user
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Email of a user
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Medical information of a user
        /// </summary>
        public string MedicalInformation { get; set; }

        /// <summary>
        /// Bank account of a user
        /// </summary>
        public string BankAccount { get; set; }
        public string HashedPassword { get; set; }
        public ICollection<UserPayment> UserPayments { get; set; }

        /// <summary>
        /// Flat associated with an user
        /// </summary>
        public Flat Flat { get; set; }

        /// <summary>
        /// Flat id associated with an user
        /// </summary>
        public int? FlatId { get; set; }
    }
}
