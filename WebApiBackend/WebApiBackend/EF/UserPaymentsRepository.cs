using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebApiBackend.Interfaces;
using WebApiBackend.Model;

namespace WebApiBackend.EF
{
    public class UserPaymentsRepository : EfRepository<UserPayment, FlatManagementContext>
    {

        private readonly FlatManagementContext flatManagementContext;
        public UserPaymentsRepository(FlatManagementContext context) : base(context)
        {
            this.flatManagementContext = context;
        }

            // Deletes the Userpayment mapping for a particular user and payment
            public async Task<UserPayment> DeleteUserFromPayment(int userId, int paymentId)
            {
                var entity = await flatManagementContext.Set<UserPayment>().FindAsync(userId, paymentId);
                if (entity == null)
                {
                    return entity;
                }

                flatManagementContext.Set<UserPayment>().Remove(entity);
                await flatManagementContext.SaveChangesAsync();

                return entity;
        }
    }
}
