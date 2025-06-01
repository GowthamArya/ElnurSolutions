using ElnurSolutions.DataBase;
using ElnurSolutions.Models;
using ElnurSolutions.ResponseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using RTE.Converters.Pdf;

namespace ElnurSolutions.Controllers
{
	[Authorize]
	public class ProductsController : Controller
	{
		private readonly ElnurDbContext _context;
		private readonly IMemoryCache _cache;
		
		public ProductsController(ElnurDbContext context, IMemoryCache cache)
		{
			_context = context;
			_cache = cache;
		}

		[AllowAnonymous]
		public IActionResult Index()
		{
			return View();
		}

		[AllowAnonymous]
		public async Task<BaseEntityResponse<List<Product>>> GetProductsBySearchCriteria(string lookupText, int? categoryId, int page = 1, int pageSize = 8)
		{
			var response = new BaseEntityResponse<List<Product>>();
			response.entity = new List<Product>();

			var cacheKey = $"products_{lookupText}_{categoryId}_{page}_{pageSize}";
			var totalCountCacheKey = $"products_{lookupText}_{categoryId}_{page}_{pageSize}_count";

			_cache.TryGetValue(totalCountCacheKey, out int? cachedTotalCount);

			if (!_cache.TryGetValue(cacheKey, out List<Product> cachedProducts) && cachedTotalCount == null)
			{
				var query = _context.Products.AsQueryable();

				if (categoryId.HasValue)
				{
					query = query.Where(p => p.ProductCategoryId == categoryId);
				}

				if (!string.IsNullOrWhiteSpace(lookupText))
				{
					var loweredLookup = lookupText.ToLower();
					query = query.Where(p => p.Name.ToLower().Contains(loweredLookup));
				}

				var totalRecords = await query.CountAsync();
				var products = await query
					.Skip((page - 1) * pageSize)
					.Take(pageSize)
					.OrderBy(p => p.DisplayOrder)
					.ToListAsync();
				var cacheOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromHours(1));

				_cache.Set(cacheKey, products, cacheOptions);
				_cache.Set(totalCountCacheKey, totalRecords, cacheOptions);

				response.entity = products;
				response.TotalRecords = totalRecords;
			}
			else
			{
				response.entity = cachedProducts;
				response.TotalRecords = (int)cachedTotalCount;
			}
			return response;
		}

		[AllowAnonymous]
		public async Task<BaseEntityResponse<Product>> GetDetails(int? id)
		{
			var response = new BaseEntityResponse<Product>();
			if (id == null)
			{
				return response;
			}

			var product = await _context.Products
				.Include(p => p.ProductCategory)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (product != null)
			{
				response.entity = product;
			}

			return response;
		}

		#region Products CRUD
		public async Task<IActionResult> ProductsData()
		{
			var elnurDbContext = _context.Products.Include(p => p.ProductCategory);
			return View(await elnurDbContext.ToListAsync());
		}

		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var product = await _context.Products
				.Include(p => p.ProductCategory)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (product == null)
			{
				return NotFound();
			}

			return View(product);
		}

		// GET: Products/Create
		public IActionResult Create()
		{
			ViewData["ProductCategoryId"] = new SelectList(_context.ProductCategories, "Id", "Name");
			return View();
		}

		// POST: Products/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Product product)
		{
			if (ModelState.IsValid)
			{
				_context.Add(product);
				await _context.SaveChangesAsync();
				return Redirect("~/Account");
			}
			ViewData["ProductCategoryId"] = new SelectList(_context.ProductCategories, "Id", "Name", product.ProductCategoryId);
			return View(product);
		}

		// GET: Products/Edit/5
		public async Task<IActionResult> Edit(int? id)
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
			ViewData["ProductCategoryId"] = new SelectList(_context.ProductCategories, "Id", "Name", product.ProductCategoryId);
			return View(product);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, Product product)
		{
			if (id != product.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(product);
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
				return Redirect("~/Account");
			}
			ViewData["ProductCategoryId"] = new SelectList(_context.ProductCategories, "Id", "Name", product.ProductCategoryId);
			return View(product);
		}
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var product = await _context.Products
				.Include(p => p.ProductCategory)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (product == null)
			{
				return NotFound();
			}

			return View(product);
		}

		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var product = await _context.Products.FindAsync(id);
			if (product != null)
			{
				_context.Products.Remove(product);
			}

			await _context.SaveChangesAsync();
			return Redirect("~/Account");
		}
		[AllowAnonymous]
		public Task<BaseEntityResponse<List<Product>>> GetProductImages()
		{
			BaseEntityResponse<List<Product>> products = new BaseEntityResponse<List<Product>>();
			products.entity = new List<Product>();

			var cacheKey = "productImages";
			if (!_cache.TryGetValue(cacheKey, out List<Product> productImages))
			{
				var productsData = _context.Products.Include(p => p.ProductCategory)
					.Where(p => !string.IsNullOrEmpty(p.ImageGuid))
						.Select(p => new Product
						{
							ImageGuid = p.ImageGuid,
							ProductCategory = p.ProductCategory
						})
					.ToList();
				products.entity = productsData;
				var cacheOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(300));

				_cache.Set(cacheKey, productsData, cacheOptions);
			}
			else
			{
				products.entity = productImages;
			}
			return Task.FromResult(products);
		}
		private bool ProductExists(int id)
		{
			return _context.Products.Any(e => e.Id == id);
		}
		#endregion
	}
}
