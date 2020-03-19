using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiBackend.Dto
{
    public class ResetPasswordDTO
    {
        public string Email { get; set; }
        public string ResetToken { get; set; }
    }
}
