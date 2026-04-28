using System.ComponentModel.DataAnnotations;
using FlowerShopOnlineOrderSystem.ViewModels;

namespace FlowerShopOnlineOrderSystem.Validation
{
    public class RequiredOrderItemsAttribute : ValidationAttribute
    {
        public RequiredOrderItemsAttribute()
        {
            ErrorMessage = "Select at least one flower before placing the order.";
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is IEnumerable<OrderItemInputViewModel> items && items.Any(i => i.Quantity > 0))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult(ErrorMessage);
        }
    }
}
