using System;
using WebApiBackend.Interfaces;

namespace WebApiBackend.Dto
{
    public class FlatDTO : IEntity
    {
        public int Id { get; set; }
        public string Address { get; set; }
    }
}
