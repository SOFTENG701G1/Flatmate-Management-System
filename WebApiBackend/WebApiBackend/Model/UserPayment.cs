namespace WebApiBackend.Model
{
    public class UserPayment
    {

        public int UserId { get; set; }
        public int PaymentId { get; set; }
        public User User { get; set; }
        public Payment Payment { get; set; }
    }
}
