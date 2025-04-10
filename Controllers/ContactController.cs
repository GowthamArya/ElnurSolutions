using Microsoft.AspNetCore.Mvc;

namespace ElnurSolutions.Controllers
{
	public class ContactController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
