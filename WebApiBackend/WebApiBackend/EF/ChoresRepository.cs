using WebApiBackend.Model;

namespace WebApiBackend.EF
{
    public class ChoresRepository : EfRepository<Chores, FlatManagementContext>
    {
        public ChoresRepository(FlatManagementContext context) : base(context)
        {

        }

    }
}

