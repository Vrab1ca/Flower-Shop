using System.ComponentModel.DataAnnotations;

namespace FlowerShopOnlineOrderSystem.ViewModels
{
    public class OrderCreateViewModel
    {
        [Required]
        public int CustomerId { get; set; }

        public List<OrderItemViewModel> Items { get; set; } = new();

        public decimal TotalPrice => Items.Sum(i => i.Subtotal);
    }
}