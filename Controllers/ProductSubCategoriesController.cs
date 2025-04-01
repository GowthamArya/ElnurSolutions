using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ElnurSolutions.DataBase;
using ElnurSolutions.Models;

namespace ElnurSolutions.Controllers
{
	public class ProductSubCategoriesController : Controller
	{
		private readonly ElnurDbContext _context;

		public ProductSubCategoriesController(ElnurDbContext context)
		{
			_context = context;
		}

		// GET: ProductSubCategories
		public async Task<IActionResult> Index()
		{
			var elnurDbContext = _context.ProductSubCategories.Include(p => p.ProductCategory);
			return View(await elnurDbContext.ToListAsync());
		}

		// GET: ProductSubCategories/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var productSubCategory = await _context.ProductSubCategories
				.Include(p => p.ProductCategory)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (productSubCategory == null)
			{
				return NotFound();
			}

			return View(productSubCategory);
		}

		// GET: ProductSubCategories/Create
		public IActionResult Create()
		{
			ViewData["ProductCategoryId"] = new SelectList(_context.ProductCategories, "Id", "Id");
			return View();
		}

		// POST: ProductSubCategories/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Id,Name,Description,ProductCategoryId,CreationDate,LastUpdate")] ProductSubCategory productSubCategory)
		{
			if (ModelState.IsValid)
			{
				_context.Add(productSubCategory);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			ViewData["ProductCategoryId"] = new SelectList(_context.ProductCategories, "Id", "Id", productSubCategory.ProductCategoryId);
			return View(productSubCategory);
		}

		// GET: ProductSubCategories/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var productSubCategory = await _context.ProductSubCategories.FindAsync(id);
			if (productSubCategory == null)
			{
				return NotFound();
			}
			ViewData["ProductCategoryId"] = new SelectList(_context.ProductCategories, "Id", "Id", productSubCategory.ProductCategoryId);
			return View(productSubCategory);
		}

		// POST: ProductSubCategories/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,ProductCategoryId,CreationDate,LastUpdate")] ProductSubCategory productSubCategory)
		{
			if (id != productSubCategory.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(productSubCategory);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!ProductSubCategoryExists(productSubCategory.Id))
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
			ViewData["ProductCategoryId"] = new SelectList(_context.ProductCategories, "Id", "Id", productSubCategory.ProductCategoryId);
			return View(productSubCategory);
		}

		// GET: ProductSubCategories/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var productSubCategory = await _context.ProductSubCategories
				.Include(p => p.ProductCategory)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (productSubCategory == null)
			{
				return NotFound();
			}

			return View(productSubCategory);
		}

		// POST: ProductSubCategories/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			var productSubCategory = await _context.ProductSubCategories.FindAsync(id);
			if (productSubCategory != null)
			{
				_context.ProductSubCategories.Remove(productSubCategory);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool ProductSubCategoryExists(int id)
		{
			return _context.ProductSubCategories.Any(e => e.Id == id);
		}
	}
}
