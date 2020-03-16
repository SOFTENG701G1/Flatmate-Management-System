using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiBackend.Model
{
    public class User
    {
        [Key]
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string MedicalInformation { get; set; }
        public string BankAccount { get; set; }
        public string HashedPassword { get; set; }
        public ICollection<UserPayment> UserPayments { get; set; }
        public Flat Flat { get; set; }
        public int? FlatId { get; set; }
    }
}
