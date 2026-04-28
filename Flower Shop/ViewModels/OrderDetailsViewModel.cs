using FlowerShopOnlineOrderSystem.Models;

namespace FlowerShopOnlineOrderSystem.ViewModels
{
    public class OrderDetailsViewModel
    {
        public int OrderId { get; set; }

        public DateTime OrderDate { get; set; }

        public string CustomerName { get; set; } = string.Empty;

        public string CustomerEmail { get; set; } = string.Empty;

        public decimal TotalPrice { get; set; }

        public OrderStatus Status { get; set; }

        public IReadOnlyCollection<OrderItemViewModel> Items { get; set; } = Array.Empty<OrderItemViewModel>();
    }
}
