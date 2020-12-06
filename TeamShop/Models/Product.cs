using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TeamShop.Models
{
    public class Product
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
        public byte[] Attach1 { get; set; }
    }
}
