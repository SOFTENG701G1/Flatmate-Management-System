using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiBackend.Model
{
    public enum ScheduleType
    {
        Away
    }

    public class Schedule
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ScheduleType ScheduleType { get; set; }
    }
}
