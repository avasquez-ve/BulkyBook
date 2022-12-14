using System.ComponentModel.DataAnnotations;

namespace BulkyBook.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string? Description { get; set; }
        [Required]
        public string ISBN { get; set; }
        public string Author { get; set; }
        [Required]
        [Range(0,10000)]
        public double ListPrice { get; set; }
        [Required]
        [Range(0,10000)]
        public double Price { get; set; }
        [Required]
        [Range(0, 10000)]
        public double PriceFor50 { get; set; }
        [Required]
        [Range(0, 10000)]
        public double PriceFor100 { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public Category Category { get; set; }
        [Required]
        public int CoverTypeId { get; set; }
        [Required]
        public CoverType CoverType { get; set; }


    }
}
