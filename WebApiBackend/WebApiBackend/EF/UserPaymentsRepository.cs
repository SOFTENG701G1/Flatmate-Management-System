using System.Threading.Tasks;
using WebApiBackend.Model;

namespace WebApiBackend.EF
{
    public class UserPaymentsRepository : EfRepository<UserPayment, FlatManagementContext>
    {

        private readonly FlatManagementContext _flatManagementContext;
        public UserPaymentsRepository(FlatManagementContext context) : base(context)
        {
            this._flatManagementContext = context;
        }

            // Deletes the Userpayment mapping for a particular user and payment
            public async Task<UserPayment> DeleteUserFromPayment(int userId, int paymentId)
            {
                var entity = await _flatManagementContext.Set<UserPayment>().FindAsync(userId, paymentId);
                if (entity == null)
                {
                    return entity;
                }

                _flatManagementContext.Set<UserPayment>().Remove(entity);
                await _flatManagementContext.SaveChangesAsync();

                return entity;
        }
    }
}
