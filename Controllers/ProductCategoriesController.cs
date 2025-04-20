using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ElnurSolutions.DataBase;
using ElnurSolutions.Models;
using ElnurSolutions.ResponseModels;
using System.Collections.Generic;

namespace ElnurSolutions.Controllers
{
	public class ProductCategoriesController : Controller
	{
		private readonly ElnurDbContext _context;

		public ProductCategoriesController(ElnurDbContext context)
		{
			_context = context;
		}

		// GET: ProductCategories
		public async Task<IActionResult> Index()
		{
			return View(await _context.ProductCategories.ToListAsync());
		}

		// GET: ProductCategories/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var productCategory = await _context.ProductCategories
				.FirstOrDefaultAsync(m => m.Id == id);
			if (productCategory == null)
			{
				return NotFound();
			}

			return View(productCategory);
		}

		// GET: ProductCategories/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: ProductCategories/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Id,Name,Description")] ProductCategory productCategory)
		{
			if (ModelState.IsValid)
			{
				_context.Add(productCategory);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			if (!ModelState.IsValid)
			{
				foreach (var key in ModelState.Keys)
				{
					var errors = ModelState[key].Errors;
					foreach (var error in errors)
					{
						Console.WriteLine($"Field: {key} - Error: {error.ErrorMessage}");
					}
				}
			}

			return View(productCategory);
		}

		// GET: ProductCategories/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var productCategory = await _context.ProductCategories.FindAsync(id);
			if (productCategory == null)
			{
				return NotFound();
			}
			return View(productCategory);
		}

		// POST: ProductCategories/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,CreationDate,LastUpdate")] ProductCategory productCategory)
		{
			if (id != productCategory.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(productCategory);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!ProductCategoryExists(productCategory.Id))
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
			return View(productCategory);
		}

		// GET: ProductCategories/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var productCategory = await _context.ProductCategories
				.FirstOrDefaultAsync(m => m.Id == id);
			if (productCategory == null)
			{
				return NotFound();
			}

			return View(productCategory);
		}

		// POST: ProductCategories/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var productCategory = await _context.ProductCategories.FindAsync(id);
			if (productCategory != null)
			{
				_context.ProductCategories.Remove(productCategory);
			}
				
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}
		public BaseEntityResponse<List<ProductCategory>> GetProductCategories()
		{
			var response = new BaseEntityResponse<List<ProductCategory>>();
			response.entity = _context.ProductCategories.ToListAsync().Result;
			return response;
		}
		private bool ProductCategoryExists(int id)
		{
			return _context.ProductCategories.Any(e => e.Id == id);
		}
	}
}
