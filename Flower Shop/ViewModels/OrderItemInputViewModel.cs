using System.ComponentModel.DataAnnotations;

namespace FlowerShopOnlineOrderSystem.ViewModels
{
    public class OrderItemInputViewModel
    {
        [Required]
        public int FlowerId { get; set; }

        public string FlowerName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int AvailableQuantity { get; set; }

        public string? ImageUrl { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be negative.")]
        public int Quantity { get; set; }

        public decimal Subtotal => Price * Quantity;
    }
}
