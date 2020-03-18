using WebApiBackend.Model;

namespace WebApiBackend.EF
{
    public class UserRepository : EfRepository<User, FlatManagementContext>
    {
        public UserRepository(FlatManagementContext context) : base(context)
        {
        }
    }
}
