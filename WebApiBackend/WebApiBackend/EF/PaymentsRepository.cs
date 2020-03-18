using WebApiBackend.Model;

namespace WebApiBackend.EF
{
    public class PaymentsRepository : EfRepository<Payment,FlatManagementContext>
    {
        public PaymentsRepository (FlatManagementContext context) : base(context)
        {

        }
        
    }
}

