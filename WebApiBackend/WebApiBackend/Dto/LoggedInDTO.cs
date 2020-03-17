using WebApiBackend.Model;

namespace WebApiBackend.Dto
{
    public class LoggedInDto
    {
        public string UserName { get; set; }
        public string Token { get; set; }

        public LoggedInDto(User user, string token)
        {
            UserName = user.UserName;
            Token = token;
        }
    }
}
