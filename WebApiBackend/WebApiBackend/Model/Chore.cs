using System;
using System.Collections.Generic;
using WebApiBackend.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace WebApiBackend.Model
{
    public class Chore : IEntity
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public User AssignedUser { get; set; }
        public DateTime DueDate { get; set; }
        public bool Completed { get; set; }
        public bool Recurring { get; set; }

        public override bool Equals(Object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Chore otherChore = (Chore)obj;
                return this.Id.Equals(otherChore.Id) &&
                   this.Title.Equals(otherChore.Title) &&
                   this.Description.Equals(otherChore.Description) &&
                   this.AssignedUser.Id.Equals(otherChore.AssignedUser.Id) &&
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
