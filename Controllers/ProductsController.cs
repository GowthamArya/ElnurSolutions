using ElnurSolutions.DataBase;
using ElnurSolutions.Models;
using ElnurSolutions.ResponseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Collections.Generic;

namespace ElnurSolutions.Controllers
{
	[Authorize]
	public class ProductsController : Controller
	{
		private readonly ElnurDbContext _context;
		[AllowAnonymous]
		public IActionResult Index()
		{
			return View();
		}

		[AllowAnonymous]
		public async Task<BaseEntityResponse<List<Product>>> GetProductsBySearchCriteria(string lookupText, int? categoryId, int page = 1, int pageSize = 10)
		{
			var response = new BaseEntityResponse<List<Product>>();

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
				.ToListAsync();

			response.entity = products;
			response.TotalRecords = totalRecords;

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
		public ProductsController(ElnurDbContext context)
		{
			_context = context;
		}

		#region Products CRUD
		// GET: Products
		public async Task<IActionResult> ProductsData()
		{
			var elnurDbContext = _context.Products.Include(p => p.ProductCategory);
			return View(await elnurDbContext.ToListAsync());
		}

		// GET: Products/Details/5
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

		// POST: Products/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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

		// GET: Products/Delete/5
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

		// POST: Products/Delete/5
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

		private bool ProductExists(int id)
		{
			return _context.Products.Any(e => e.Id == id);
		}
		#endregion
	}
}
