using System.ComponentModel.DataAnnotations;

namespace Shop.Models
{
    public class ProductImage
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string ImageAddress { get; set; }

        public virtual int ProductId { get; set; }
        public virtual Product Product { get; set; }
    }
}
