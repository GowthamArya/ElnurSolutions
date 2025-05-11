using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ElnurSolutions.Models;
using ElnurSolutions.DataBase;
using Microsoft.AspNetCore.Authorization;

[Authorize]
public class TeamCategoriesController : Controller
{
	private readonly ElnurDbContext _context;

	public TeamCategoriesController(ElnurDbContext context)
	{
		_context = context;
	}

	// GET: TeamCategories
	public async Task<IActionResult> Index()
	{
		return View(await _context.TeamCategories.ToListAsync());
	}

	// GET: TeamCategories/Details/5
	public async Task<IActionResult> Details(int? id)
	{
		if (id == null)
			return NotFound();

		var teamCategory = await _context.TeamCategories
			.FirstOrDefaultAsync(m => m.Id == id);

		if (teamCategory == null)
			return NotFound();

		return View(teamCategory);
	}

	// GET: TeamCategories/Create
	public IActionResult Create()
	{
		return View();
	}

	// POST: TeamCategories/Create
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create(TeamCategory teamCategory)
	{
		if (!string.IsNullOrEmpty(teamCategory.Name))
		{
			_context.Add(teamCategory);
			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}
		return View(teamCategory);
	}

	// GET: TeamCategories/Edit/5
	public async Task<IActionResult> Edit(int? id)
	{
		if (id == null)
			return NotFound();

		var teamCategory = await _context.TeamCategories.FindAsync(id);
		if (teamCategory == null)
			return NotFound();

		return View(teamCategory);
	}

	// POST: TeamCategories/Edit/5
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Edit(int id, TeamCategory teamCategory)
	{
		if (id != teamCategory.Id)
			return NotFound();

		if (ModelState.IsValid)
		{
			try
			{
				_context.Update(teamCategory);
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!TeamCategoryExists(teamCategory.Id))
					return NotFound();
				else
					throw;
			}
			return RedirectToAction(nameof(Index));
		}
		return View(teamCategory);
	}

	// GET: TeamCategories/Delete/5
	public async Task<IActionResult> Delete(int? id)
	{
		if (id == null)
			return NotFound();

		var teamCategory = await _context.TeamCategories
			.FirstOrDefaultAsync(m => m.Id == id);

		if (teamCategory == null)
			return NotFound();

		return View(teamCategory);
	}

	// POST: TeamCategories/Delete/5
	[HttpPost, ActionName("Delete")]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> DeleteConfirmed(int id)
	{
		var teamCategory = await _context.TeamCategories.FindAsync(id);
		if (teamCategory != null)
		{
			_context.TeamCategories.Remove(teamCategory);
			await _context.SaveChangesAsync();
		}

		return RedirectToAction(nameof(Index));
	}

	private bool TeamCategoryExists(int id)
	{
		return _context.TeamCategories.Any(e => e.Id == id);
	}
}
