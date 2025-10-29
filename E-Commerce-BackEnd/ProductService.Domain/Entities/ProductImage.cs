using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProductService.Domain.Entities
{
    public record ProductImage
    {
        public long ProductImageId { get; set; }
        [Required]
        public string ProuductImageBase64String { get; set; } = null!;
        [ForeignKey("Product")]
        public long ProductId { get; set; }

        //Navigation Properties
        public Product Product { get; set; } = null!;
    }
}
