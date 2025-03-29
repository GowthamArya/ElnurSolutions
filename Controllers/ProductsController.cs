using Microsoft.AspNetCore.Mvc;

namespace ElnurSolutions.Controllers
{
	public class ProductsController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
