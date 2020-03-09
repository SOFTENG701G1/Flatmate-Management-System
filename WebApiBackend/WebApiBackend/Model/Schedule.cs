using System;
using System.Collections.Generic;
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
        public string UserName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int MyProperty { get; set; }
        public ScheduleType ScheduleType { get; set; }
    }
}
