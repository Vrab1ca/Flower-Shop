using System.ComponentModel.DataAnnotations;

namespace FlowerShopOnlineOrderSystem.ViewModels
{
    public class FlowerViewModel
    {
        public int FlowerId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        [Range(0.01, 10000)]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue)]
        public int AvailableQuantity { get; set; }

        [StringLength(500)]
        [Display(Name = "Image URL")]
        public string? ImageUrl { get; set; }
    }
}
