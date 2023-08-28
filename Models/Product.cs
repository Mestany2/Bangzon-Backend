using System.Collections;
using System.ComponentModel.DataAnnotations;
namespace Bangzon.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string ProductName { get; set; }
        public int UserId { get; set; }
        public decimal ProductPrice { get; set; }
        public int CategoryId { get; set; }
        public string ProductDescription { get; set; }
        public List<Order> Orders { get; set; }

    }
}
