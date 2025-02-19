using System.ComponentModel.DataAnnotations;

namespace ShopMVC.Models
{
    public class RentalDetail
    {
        [Key]
        public int RentalHeaderDetailId { get; set; }
        public int RentalHeaderId { get; set; }
        public int MovieId { get; set; }

        public string Status { get; set; }

        public RentalHeader? RentalHeader { get; set; }
        public Movie? Movie { get; set; }
    }
}
