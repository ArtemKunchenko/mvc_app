using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mvc_app.Models;
using mvc_app.Services;

namespace mvc_app.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IServiceProducts _serviceProducts;


        public ProductsController(IServiceProducts serviceProducts)
        {
            _serviceProducts = serviceProducts;

        }
        [HttpGet]
        public async Task<ViewResult> Index()
        {
            var products = await _serviceProducts.ReadAsync();
            return View(products);

        }
        [HttpGet]
        public async Task<ViewResult> Details(int id)
        {
            var product = _serviceProducts.GetByIdAsync(id);
            return View(product);
        }
        [Authorize(Roles = "admin")]
        public ViewResult Create() => View();
        [HttpGet]
        [Authorize(Roles = "admin")]
        public ViewResult Delete() => View();
        [HttpGet]
        [Authorize(Roles = "admin")]
        public ViewResult Update() => View();
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Description")] Product product)
        {
            if (ModelState.IsValid)

            {
               await _serviceProducts.CreateAsync(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Update(int id, [Bind("Id,Name,Price,Description")] Product product)
        {
            if (ModelState.IsValid)
            {
               await _serviceProducts?.UpdateAsync(id, product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _serviceProducts?.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
