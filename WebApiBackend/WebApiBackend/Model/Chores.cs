using System;
using System.Collections.Generic;

namespace WebApiBackend.Model
{
    public class Chores
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ICollection<User> Users { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public bool Recurring { get; set; }
    }
}
