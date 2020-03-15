using System;
using WebApiBackend.Model;

namespace WebApiBackend.EF
{
    public class UserPaymentsRepository : EfRepository<UserPayment, FlatManagementContext>
    {
        public UserPaymentsRepository(FlatManagementContext context) : base(context)
        {
        }
    }
}
