using System;
using System.Collections.Generic;
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
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        /// <summary>
        /// The type of leave associated with a schedule
        /// </summary>
        public ScheduleType ScheduleType { get; set; }
    }
}
