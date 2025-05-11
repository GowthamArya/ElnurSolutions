using ElnurSolutions.DataBase;
using ElnurSolutions.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore; // for Include

[Authorize]
public class TeamController : Controller
{
	private readonly ElnurDbContext _context;

	public TeamController(ElnurDbContext context)
	{
		_context = context;
	}

	[AllowAnonymous]
	public async Task<IActionResult> Index()
	{
		var categoriesWithMembers = await _context.TeamCategories
			.Include(tc => tc.TeamMembers)
			.ToListAsync();

		return View(categoriesWithMembers);
	}


	#region Teams CRUD

	// GET: TeamMembers
	public async Task<IActionResult> TeamsData()
	{
		var elnurDbContext = _context.TeamMembers.Include(t => t.TeamCategory);
		return View(await elnurDbContext.ToListAsync());
	}

	// GET: TeamMembers/Details/5
	public async Task<IActionResult> Details(int? id)
	{
		if (id == null)
		{
			return NotFound();
		}

		var teamMember = await _context.TeamMembers
			.Include(t => t.TeamCategory)
			.FirstOrDefaultAsync(m => m.Id == id);

		if (teamMember == null)
		{
			return NotFound();
		}

		return View(teamMember);
	}

	// GET: TeamMembers/Create
	public IActionResult Create()
	{
		ViewData["TeamCategoryId"] = new SelectList(_context.TeamCategories, "Id", "Name");
		return View();
	}

	// POST: TeamMembers/Create
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Create(TeamMember teamMember)
	{
		if (!string.IsNullOrEmpty(teamMember.Name))
		{
			_context.Add(teamMember);
			await _context.SaveChangesAsync();
			return Redirect("~/Account");
		}
		ViewData["TeamCategoryId"] = new SelectList(_context.TeamCategories, "Id", "Name", teamMember.TeamCategoryId);
		return View(teamMember);
	}

	// GET: TeamMembers/Edit/5
	public async Task<IActionResult> Edit(int? id)
	{
		if (id == null)
		{
			return NotFound();
		}

		var teamMember = await _context.TeamMembers.FindAsync(id);
		if (teamMember == null)
		{
			return NotFound();
		}
		ViewData["TeamCategoryId"] = new SelectList(_context.TeamCategories, "Id", "Name", teamMember.TeamCategoryId);
		return View(teamMember);
	}

	// POST: TeamMembers/Edit/5
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Edit(int id, TeamMember teamMember)
	{
		if (id != teamMember.Id)
		{
			return NotFound();
		}

		if (ModelState.IsValid)
		{
			try
			{
				_context.Update(teamMember);
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
				if (!TeamMemberExists(teamMember.Id))
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
		ViewData["TeamCategoryId"] = new SelectList(_context.TeamCategories, "Id", "Name", teamMember.TeamCategoryId);
		return View(teamMember);
	}

	// GET: TeamMembers/Delete/5
	public async Task<IActionResult> Delete(int? id)
	{
		if (id == null)
		{
			return NotFound();
		}

		var teamMember = await _context.TeamMembers
			.Include(t => t.TeamCategory)
			.FirstOrDefaultAsync(m => m.Id == id);
		if (teamMember == null)
		{
			return NotFound();
		}

		return View(teamMember);
	}

	// POST: TeamMembers/Delete/5
	[HttpPost, ActionName("Delete")]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> DeleteConfirmed(int id)
	{
		var teamMember = await _context.TeamMembers.FindAsync(id);
		if (teamMember != null)
		{
			_context.TeamMembers.Remove(teamMember);
		}

		await _context.SaveChangesAsync();
		return Redirect("~/Account");
	}

	private bool TeamMemberExists(int id)
	{
		return _context.TeamMembers.Any(e => e.Id == id);
	}

	#endregion

}
