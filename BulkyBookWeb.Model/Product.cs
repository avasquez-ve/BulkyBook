using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Display(Name ="List Price")]
        [Required]
        [Range(0,10000)]
        public double ListPrice { get; set; }
        [Display(Name = "Price for 1-50")]
        [Required]
        [Range(0,10000)]
        public double Price { get; set; }
        [Display(Name = "Price for 5-100")]
        [Required]
        [Range(0, 10000)]
        public double PriceFor50 { get; set; }
        [Display(Name = "Price for 100+")]
        [Required]
        [Range(0, 100000)]
        public double PriceFor100 { get; set; }

        [ValidateNever]
        public string ImageUrl { get; set; }
        
        [Required]
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        [ValidateNever]
        public Category Category { get; set; }
        [Required]
        public int CoverTypeId { get; set; }

        [ForeignKey("CoverTypeId")]
        [Display(Name = "Cover Type")]
        [ValidateNever]
        public CoverType CoverType { get; set; }


    }
}
