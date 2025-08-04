using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using TEST1.Data;
using TEST1.Models;
using TEST1.Models.ViewModels;

public class ProductController : Controller
{
    private readonly ApplicationDbContext _context;

    public ProductController(ApplicationDbContext context)
    {
        _context = context;
    }

    public IActionResult Index(string title, int categoryId = 0, bool? isPublished = null)
    {
        var query = _context.Products.Include(p => p.Category).AsQueryable();

        if (!string.IsNullOrEmpty(title))
        {
            query = query.Where(p => p.Title.Contains(title));
        }

        if (categoryId > 0)
        {
            query = query.Where(p => p.CategoryId == categoryId);
        }

        if (isPublished.HasValue)
        {
            query = query.Where(p => p.IsPublished == isPublished.Value);
        }

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
}
