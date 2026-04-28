using FlowerShopOnlineOrderSystem.Data;
using FlowerShopOnlineOrderSystem.Services.Interfaces;
using FlowerShopOnlineOrderSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Flower_Shop.Controllers
{
    public class FlowersController : Controller
    {
        private readonly IFlowerService _flowerService;

        public FlowersController(IFlowerService flowerService)
        {
            _flowerService = flowerService;
        }

        public async Task<IActionResult> Index()
        {
            var flowers = await _flowerService.GetCatalogAsync();
            return View(flowers);
        }

        [Authorize(Roles = RoleNames.Staff)]
        public IActionResult Create()
        {
            return View(new FlowerViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleNames.Staff)]
        public async Task<IActionResult> Create(FlowerViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            await _flowerService.CreateAsync(model);
            TempData["SuccessMessage"] = "Flower added to inventory.";
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = RoleNames.Staff)]
        public async Task<IActionResult> Edit(int id)
        {
            var flower = await _flowerService.GetByIdAsync(id);
            return flower == null ? NotFound() : View(flower);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleNames.Staff)]
        public async Task<IActionResult> Edit(int id, FlowerViewModel model)
        {
            if (id != model.FlowerId)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var updated = await _flowerService.UpdateAsync(model);

            if (!updated)
            {
                return NotFound();
            }

            TempData["SuccessMessage"] = "Inventory updated.";
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = RoleNames.Staff)]
        public async Task<IActionResult> Delete(int id)
        {
            var flower = await _flowerService.GetByIdAsync(id);
            return flower == null ? NotFound() : View(flower);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleNames.Staff)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deleted = await _flowerService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            TempData["SuccessMessage"] = "Flower removed from inventory.";
            return RedirectToAction(nameof(Index));
        }
    }
}
