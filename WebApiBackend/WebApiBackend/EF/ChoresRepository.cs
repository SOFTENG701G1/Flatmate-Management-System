using WebApiBackend.Model;

namespace WebApiBackend.EF
{
    public class ChoresRepository : EfRepository<Chore, FlatManagementContext>
    {
        public ChoresRepository(FlatManagementContext context) : base(context)
        {

        }

    }
}

