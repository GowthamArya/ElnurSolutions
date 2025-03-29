using Microsoft.AspNetCore.Mvc;

namespace ElnurSolutions.Controllers
{
	public class AboutUsController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
		public IActionResult OurTeam()
		{
			return View("OurTeam");
		}
	}
}
