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

        public override bool Equals(Object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                ChoreDTO otherChore = (ChoreDTO)obj;
                return this.Id.Equals(otherChore.Id) &&
                   this.Title.Equals(otherChore.Title) &&
                   this.Description.Equals(otherChore.Description) &&
                   this.Assignee.Equals(otherChore.Assignee) &&
                   this.DueDate.Equals(otherChore.DueDate) &&
                   this.Completed.Equals(otherChore.Completed) &&
                   this.Recurring.Equals(otherChore.Recurring);
            }
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}
