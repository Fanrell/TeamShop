using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;


namespace TeamShop.Models
{
    public class ProductView
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public float Price { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public string Tags { get; set; }
        [Required]
        public string Types { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public IFormFile Attach1 { get; set; }
    }
}
