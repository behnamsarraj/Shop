using System.ComponentModel.DataAnnotations;

namespace Shop.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name{ get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public decimal Price { get; set; }

        public virtual int CategoryId { get; set; }
        public virtual Category category { get; set; }

        public virtual ICollection<ProductImage> ProductImages { get; set; }
    }
}
