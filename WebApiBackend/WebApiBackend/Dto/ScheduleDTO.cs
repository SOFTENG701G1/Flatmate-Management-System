using System;
using WebApiBackend.Interfaces;
using WebApiBackend.Model;

namespace WebApiBackend.Dto
{
    public class ScheduleDTO : IEntity
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public ScheduleType ScheduleType { get; set; }
    }
}
