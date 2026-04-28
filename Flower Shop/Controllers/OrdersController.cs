using FlowerShopOnlineOrderSystem.Data;
using FlowerShopOnlineOrderSystem.Models;
using FlowerShopOnlineOrderSystem.Services.Interfaces;
using FlowerShopOnlineOrderSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flower_Shop.Controllers
{
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [Authorize(Roles = RoleNames.Staff)]
        public async Task<IActionResult> Index()
        {
            var orders = await _orderService.GetOrdersAsync();
            return View(orders);
        }

        public async Task<IActionResult> Create()
        {
            var model = await _orderService.BuildCreateModelAsync();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(await _orderService.BuildCreateModelAsync(model));
            }

            try
            {
                var orderId = await _orderService.CreateOrderAsync(model);
                TempData["SuccessMessage"] = "Order placed successfully.";
                return RedirectToAction(nameof(Details), new { id = orderId });
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(await _orderService.BuildCreateModelAsync(model));
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            var order = await _orderService.GetOrderDetailsAsync(id);
            return order == null ? NotFound() : View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleNames.Staff)]
        public async Task<IActionResult> UpdateStatus(int id, OrderStatus status)
        {
            var updated = await _orderService.UpdateStatusAsync(id, status);

            if (!updated)
            {
                return NotFound();
            }

            TempData["SuccessMessage"] = "Order status updated and notification queued.";
            return RedirectToAction(nameof(Details), new { id });
        }
    }
}
