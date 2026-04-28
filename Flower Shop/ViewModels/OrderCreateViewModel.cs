using System.ComponentModel.DataAnnotations;
using FlowerShopOnlineOrderSystem.Validation;

namespace FlowerShopOnlineOrderSystem.ViewModels
{
    public class OrderCreateViewModel
    {
        [Required]
        [Display(Name = "First name")]
        [StringLength(50)]
        public string CustomerFirstName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Last name")]
        [StringLength(50)]
        public string CustomerLastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(254)]
        [Display(Name = "Email")]
        public string CustomerEmail { get; set; } = string.Empty;

        [RequiredOrderItems]
        public List<OrderItemInputViewModel> Items { get; set; } = new();

        public decimal TotalPrice => Items.Sum(i => i.Subtotal);
    }
}
