namespace Bangzon.Models
{
    public class UserPaymentType
    {
        public int Id { get; set; } 
        public int UserId { get; set; }
        public int PaymentTypeId { get; set; }
        public List<PaymentType> PaymentTypes { get; set; }
        public List<User> Users { get; set; }

    }
}
