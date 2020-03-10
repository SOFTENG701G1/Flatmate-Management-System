using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiBackend.Model
{
    public class Flat
    {
        [Key]
        public int Id { get; set; }
        public string Address { get; set; }
        public ICollection<User> Users { get; set; }
        public ICollection<Schedule> Schedules { get; set; }
        public ICollection<Payment> Payments { get; set; }
    }
}
