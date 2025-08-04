using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TEST1.Data;
using TEST1.Models;
using TEST1.Models.ViewModels;

namespace TEST1.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Listeleme
        public IActionResult Index(string title, int categoryId = 0, bool? isPublished = null)
        {
            var query = _context.Products.Include(p => p.Category).AsQueryable();

            if (!string.IsNullOrWhiteSpace(title))
                query = query.Where(p => p.Title.Contains(title));

            if (categoryId > 0)
                query = query.Where(p => p.CategoryId == categoryId);

            if (isPublished.HasValue)
                query = query.Where(p => p.IsPublished == isPublished.Value);

            var model = new ProductFilterViewModel
            {
                Title = title,
                CategoryId = categoryId,
                IsPublished = isPublished,
                Products = query.ToList(),
                Categories = _context.Categories.ToList()
            };

            return View(model);
        }



        // Detaylar (isteğe bağlı)
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products.Include(p => p.Category)
                                                 .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null) return NotFound();

            return View(product);
        }

        // CREATE - GET
        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.ToList();
            return View();
        }

        // CREATE - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                var category = await _context.Categories.FindAsync(product.CategoryId);

                if (product.IsPublished)
                {
                    if (category == null)
                    {
                        ModelState.AddModelError("", "Yayınlamak için geçerli bir kategori seçilmelidir.");
                    }
                    else if (product.Stock < category.MinStock)
                    {
                        ModelState.AddModelError("", $"Yayınlamak için en az {category.MinStock} stok gereklidir.");
                    }
                }

                if (!ModelState.IsValid)
                {
                    ViewBag.Categories = _context.Categories.ToList();
                    return View(product);
                }

                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Categories = _context.Categories.ToList();
            return View(product);
        }

        // EDIT - GET
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products.FindAsync(id);
            if (product == null) return NotFound();

            ViewBag.Categories = _context.Categories.ToList();
            return View(product);
        }

        // EDIT - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var category = await _context.Categories.FindAsync(product.CategoryId);

                if (product.IsPublished)
                {
                    if (category == null)
                    {
                        ModelState.AddModelError("", "Yayınlamak için kategori seçilmelidir.");
                    }
                    else if (product.Stock < category.MinStock)
                    {
                        ModelState.AddModelError("", $"Yayınlamak için stok en az {category.MinStock} olmalıdır.");
                    }
                }

                if (!ModelState.IsValid)
                {
                    ViewBag.Categories = _context.Categories.ToList();
                    return View(product);
                }

                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Products.Any(e => e.Id == id))
                        return NotFound();
                    else
                        throw;
                }

                return RedirectToAction("Index");
            }

            ViewBag.Categories = _context.Categories.ToList();
            return View(product);
        }

        // DELETE - GET
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var product = await _context.Products.Include(p => p.Category)
                                                 .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null) return NotFound();

            return View(product);
        }

        // DELETE - POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
    }
}
