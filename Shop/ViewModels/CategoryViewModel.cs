using System.ComponentModel.DataAnnotations;

namespace Shop.ViewModels
{
    public class CategoryViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "نام دسته")]
        public string Name { get; set; }
    }
}
