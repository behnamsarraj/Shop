using System.ComponentModel.DataAnnotations;

namespace Shop.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public string CategoryName { get; set; }
    }
}
