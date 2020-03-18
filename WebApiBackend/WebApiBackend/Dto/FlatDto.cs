using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiBackend.Dto
{
    public class FlatDTO
    {
        public ICollection<UserDTO> FlatMembers { get; set; }

        public FlatDTO(Model.Flat flat)
        {
            FlatMembers = new List<UserDTO>();

            if (flat != null)
            {
                foreach (Model.User user in flat.Users)
                {
                    UserDTO userDTO = new UserDTO(user);
                    FlatMembers.Add(userDTO);
                }
            }
        }
    }
}
