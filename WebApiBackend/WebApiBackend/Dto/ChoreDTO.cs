using System;
using WebApiBackend.Interfaces;
using WebApiBackend.Model;

namespace WebApiBackend.Dto
{
    public class ChoreDTO : IEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Assignee { get; set; }
        public DateTime DueDate { get; set; }
        public bool Completed { get; set; }
        public bool Recurring { get; set; }

        public ChoreDTO() {}

        public ChoreDTO(Model.Chore chore)
        {
            Id = chore.Id;
            Title = chore.Title;
            Description = chore.Description;
            Assignee = chore.AssignedUser.Id; //need to map a user object back to an id
            DueDate = chore.DueDate;
            Completed = chore.Completed;
            Recurring = chore.Recurring;
        }
    }
}
