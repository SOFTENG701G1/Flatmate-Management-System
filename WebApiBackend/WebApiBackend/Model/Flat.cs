using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApiBackend.Interfaces;


namespace WebApiBackend.Model
{
    public class Flat : IEntity
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
