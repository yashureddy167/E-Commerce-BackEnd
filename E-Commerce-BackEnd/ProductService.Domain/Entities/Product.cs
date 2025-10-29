using System.ComponentModel.DataAnnotations;

namespace ProductService.Domain.Entities
{
    public record Product
    {
        [Key]
        public long ProductId { get; set; }
        [Required]
        public string ProductName { get; set; } = null!;
        [Required]
        public string ProductDescription { get; set; } = null!;
        [Required]
        public decimal Price { get; set; }
        [Required]
        public long Stock { get; set; }
        public bool inStock => Stock > 0;
        [Required]
        public string MadeIn { get; set; } = null!;
        [Required]
        public string Category { get; set; } = null!;
        [Required]
        public string Company { get; set; } = null!;

        //Navigation Properties
        public ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
    }
}
