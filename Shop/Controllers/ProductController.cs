using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.ViewModels;

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
            List<ProductViewModel> Products = new List<ProductViewModel>();
            foreach (var product in _context.Products)
            {
                ProductViewModel productViewModel = new ProductViewModel() { Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    CategoryName = _context.Categories.FirstOrDefault(n => n.Id == product.CategoryId).Name.ToString()
                };
                Products.Add(productViewModel);
            }
            return View(Products);
        }
        //Get: Products/Details/5
        public async Task <IActionResult> Details(int? id)
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
            return View(product);
        }
        //Get: Product/Create
        public IActionResult Create()
        {
            return View();
        }

        //Post: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create ([Bind("Id,Name,Description,Price,CategoryName")] ProductViewModel product)
        {
            if (ModelState.IsValid)
            {
                //ProductViewModel => Product
                //Add(Product)
                int categoriesId = 0;
                bool lExecution = true;
                if (lExecution && !_context.Categories.Any(n => n.Name.Contains(product.Name)))
                {
                    lExecution = false;
                }

                if (lExecution)
                {
                    categoriesId = _context.Categories.FirstOrDefault(n => n.Name.Contains(product.Name)).Id;
                    Models.Product productModels = new Models.Product()
                    {
                        Id = product.Id,
                        Name = product.Name,
                        Description = product.Description,
                        Price = product.Price,
                        CategoryId = categoriesId
                    };
                    _context.Products.Add(productModels);
                    await _context.SaveChangesAsync();
                }
                
                
               
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }
        
        //Get: Product/Edit/n
        public async Task<IActionResult> Edit (int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return View();
        }

        //Post Product/Edit/n
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,[Bind("Id,Name,Description,Price,CategoryName")] ProductViewModel product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {

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
        public async Task<IActionResult> Delete (int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var product = _context.Products.
                FirstOrDefaultAsync(n => n.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // Post: Products/Delete/n
        [HttpPost, ActionName("Delete")]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FirstOrDefaultAsync(n => n.Id == id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction();
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(p => p.Id == id);
        }


    }
}
