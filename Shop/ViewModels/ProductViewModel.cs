using System.ComponentModel.DataAnnotations;

namespace Shop.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "نام محصول")]
        public string Name { get; set; }
        [Display(Name = "مشخصات")]
        public string Description { get; set; }
        [Display(Name = "قیمت (ريال)")]
        public decimal Price { get; set; }
        [Display(Name = "دسته بندی")]
        public int CategoryId { get; set; }
        [Display(Name = "دسته بندی")]
        public string? CategoryName { get; set; }
        
    }
}
