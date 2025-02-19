using System.ComponentModel.DataAnnotations;

namespace ShopMVC.Models
{
    public class RentalHeader
    {
        [Key]
        public int RentalHeaderId { get; set; }
        public int CustomerId { get; set; }
        public DateTime RentalDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public Customer? Customer { get; set; }
        public ICollection<RentalDetail> RentalDetails { get; set; } = new List<RentalDetail>();
    }
}
