using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApiBackend.Interfaces;


namespace WebApiBackend.Model
{
    public enum ScheduleType
    {
        Away
    }

    public class Schedule : IEntity
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }

        /// <summary>
        /// Start date of a schedule
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Start date of a end date
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// The type of leave associated with a schedule
        /// </summary>
        public ScheduleType ScheduleType { get; set; }
    }
}
