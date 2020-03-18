using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiBackend.Model;

namespace WebApiBackend.Dto
{
    public class AddUserToFlatDto
    {
        public int UserId { get; set; }
        public int FlatId   { get; set; }

        /// <summary>
        /// 0  Default/Nothing
        /// 1  Success
        /// 2  NoUser
        /// 3  NoFlat
        /// 4  UserAlreadyInFlat
        /// 5  UserInOtherFlat
        /// </summary>
        public int ResultCode { get; set; }
        
        public AddUserToFlatDto(int flatId, int userId, int resultcode = 0)
        {
            UserId = userId;
            FlatId = flatId;
            ResultCode = resultcode;
        }
    }
}
