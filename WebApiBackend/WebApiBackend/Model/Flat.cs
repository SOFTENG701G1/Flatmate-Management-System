using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApiBackend.Model
{
    public class Flat
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Address associated with a flat
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// List of Users associated with a flat
        /// </summary>
        public ICollection<User> Users { get; set; }

        /// <summary>
        /// List of Schedules associated with a flat
        /// </summary>
        public ICollection<Schedule> Schedules { get; set; }

        /// <summary>
        /// List of Payments associated with a flat
        /// </summary>
        public ICollection<Payment> Payments { get; set; }
    }
}
