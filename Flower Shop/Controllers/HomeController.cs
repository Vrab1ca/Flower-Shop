using FlowerShopOnlineOrderSystem.Models;
using FlowerShopOnlineOrderSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Flower_Shop.Controllers
{
    public class HomeController : Controller
    {
        private readonly IFlowerService _flowerService;

        public HomeController(IFlowerService flowerService)
        {
            _flowerService = flowerService;
        }

        public async Task<IActionResult> Index()
        {
            var flowers = await _flowerService.GetCatalogAsync();
            return View(flowers);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
