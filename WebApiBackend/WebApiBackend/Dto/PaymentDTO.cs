using System;
using WebApiBackend.Interfaces;
using WebApiBackend.Model;

namespace WebApiBackend.Dto
{
    public class PaymentDTO : IEntity
    {
        public int Id { get; set; }
        public int Amount { get; set; }
        public bool Fixed { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public PaymentType PaymentType { get; set; }
        public Frequency Frequency { get; set; }
        public string Description { get; set; }
    }
}
