using WebApiBackend.Model;

namespace WebApiBackend.EF
{
    public class FlatRepository : EfRepository<Flat, FlatManagementContext>
    {
        public FlatRepository(FlatManagementContext context) : base(context)
        {
        }
    }
}
