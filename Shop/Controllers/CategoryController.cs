using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;
using Shop.ViewModels;

namespace Shop.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _context.Categories.ToListAsync();

            var viewModel = categories
                .Select(category => new CategoryViewModel
                {
                    Id = category.Id,
                    Name = category.Name
                })
                .ToList();
            return View(viewModel);
        }

        public IActionResult Add()
        {
            var viewModel = new CategoryViewModel();
            return View("Form", viewModel);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            var viewModel = new CategoryViewModel
            {
                Id = category.Id,
                Name = category.Name
            };
            return View("Form", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(CategoryViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("From", viewModel);
            }

            if (viewModel.Id == 0)
            {
                var category = new Category
                {
                    Name = viewModel.Name
                };
                _context.Add(category);
            }
            else
            {
                var category = await _context.Categories.FindAsync(viewModel.Id);
                category.Name = viewModel.Name;
                _context.Update(category);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
