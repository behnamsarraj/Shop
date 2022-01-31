using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.ViewModels;
using Shop.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Shop.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductController(ApplicationDbContext context)
        {
            _context = context;
        }

        //Get: Products
        public async Task<IActionResult> Index()
        {
            List<ProductViewModel> productViewModel = new List<ProductViewModel>();
            var products = await _context.Products.Include(p => p.Category).ToListAsync();
            foreach (var product in products)
            {
                ProductViewModel viewModel = new ProductViewModel()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    CategoryName = product.Category.Name
                };
                productViewModel.Add(viewModel);
            }
            return View(productViewModel);
        }
        //Get: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var product = await _context.Products.
                FirstOrDefaultAsync(n => n.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            ProductViewModel productViewModel = new ProductViewModel()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryName = _context.Categories.FirstOrDefault(n => n.Id == product.CategoryId).Name
            };

            return View(productViewModel);
        }
        //Get: Product/Create
        public IActionResult Create()
        {
            var itemCategories = _context.Categories.Select(category => new SelectListItem
            {
                Value = category.Id.ToString(),
                Text = category.Name
            });
            ViewBag.itemCategories = itemCategories;
            return View("Form", new ProductViewModel());
        }

        //Post: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Price,CategoryId")] ProductViewModel product)
        {
            if (ModelState.IsValid)
            {
                //ProductViewModel => Product
                //Add(Product)

                Product productModels = new Product()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    CategoryId = product.CategoryId
                };
                _context.Add(productModels);
                await _context.SaveChangesAsync();



                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        //Get: Product/Edit/n
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            ProductViewModel productModel = new ProductViewModel()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.Category.Id,
                CategoryName = product.Category.Name
            };

            List<SelectListItem> itemCategories = _context.Categories.Select(categury => new SelectListItem
            {
                Value = categury.Id.ToString(),
                Text = categury.Name
            }).ToList<SelectListItem>();

            ViewBag.itemCategories = itemCategories;

            return View("Form",productModel);
        }

        //Post Product/Edit/n
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,CategoryId")] ProductViewModel product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }
            Product productModel = new Product();
            if (ModelState.IsValid)
            {
                try
                {
                    productModel = new Product()
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Description = product.Description,
                        Price = product.Price,
                        CategoryId = product.CategoryId
                    };
                    if (productModel == null)
                    {
                        return NotFound();
                    }
                    _context.Products.Update(productModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // Get: Products/Delete/n
        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var product = _context.Products.
                FirstOrDefault(n => n.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            ProductViewModel productViewModel = new ProductViewModel()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryName = _context.Categories.FirstOrDefault(n => n.Id == product.CategoryId).Name,

            };

            return View(productViewModel);
        }

        // Post: Products/Delete/n
        [HttpPost, ActionName("Delete")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(n => n.Id == id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("Save")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Save(int id,ProductViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Form", viewModel);
            }

            #region Add Mode
            if (viewModel.Id == 0)
            {
                Product product = new Product()
                {
                    Name = viewModel.Name,
                    Price = viewModel.Price,
                    Description = viewModel.Description,
                    CategoryId = viewModel.CategoryId
                };
                _context.Products.Add(product);
            }
            #endregion
            #region Edit Mode
            else
            {
                Product product = await _context.Products.FindAsync(viewModel.Id);
                product.Name = viewModel.Name;
                product.Description = viewModel.Description;
                product.Price = viewModel.Price;
                product.CategoryId = viewModel.CategoryId;

                _context.Update(product);

            }

            #endregion

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        private bool ProductExists(int id)
        {
            return _context.Products.Any(p => p.Id == id);
        }


    }
}
