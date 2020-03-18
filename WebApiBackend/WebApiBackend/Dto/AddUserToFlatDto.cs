using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiBackend.Model;

namespace WebApiBackend.Dto
{
    public class AddUserToFlatDto
    {
        public UserDTO AddedUser { get; set; }

        /// <summary>
        /// 0  Default/Nothing
        /// 1  Success
        /// 2  NoUser
        /// 3  NoFlat
        /// 4  UserAlreadyInFlat
        /// 5  UserInOtherFlat
        /// </summary>
        public int ResultCode { get; set; }
        
        public AddUserToFlatDto(UserDTO user, int resultcode = 0)
        {
            AddedUser = user;
            ResultCode = resultcode;
        }
    }
}
