using System.Collections;

namespace Bangzon.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int OrderStatusId { get; set; }
        public List <Product> Products { get; set; }

    }
}
