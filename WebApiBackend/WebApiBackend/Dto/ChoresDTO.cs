 using System;
namespace WebApiBackend.Dto
{
    public class ChoresDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public bool Recurring { get; set; }
    }
}
