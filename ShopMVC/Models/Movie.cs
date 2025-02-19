using System.ComponentModel.DataAnnotations;

namespace ShopMVC.Models
{
    public class Movie
    {
        [Key]
        public int MovieId { get; set; }
        public string Title { get; set; }
        public string Genre { get; set; }
        public int ReleaseYear { get; set; }
        public string Description { get; set; }
        public string Director { get; set; }
        public decimal RentalPrice { get; set; }

    }
}
