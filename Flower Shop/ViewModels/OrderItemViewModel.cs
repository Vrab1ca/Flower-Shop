using System.ComponentModel.DataAnnotations;

namespace FlowerShopOnlineOrderSystem.ViewModels
{
    public class OrderItemViewModel
    {
        [Required]
        public int FlowerId { get; set; }

        public string? FlowerName { get; set; }

        public decimal Price { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }

        public decimal Subtotal => Price * Quantity;
    }
}