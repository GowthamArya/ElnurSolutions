using ElnurSolutions.Models;
using Microsoft.AspNetCore.Mvc;

namespace ElnurSolutions.Controllers
{
	public class ContactController : Controller
	{
		public IActionResult Index(string? ProductName = "")
		{
			TempData["ProductName"] = ProductName;
			return View();
		}

		public IActionResult Send(string Name,string Email, string Message)
		{
			TempData["Message"] = $"Thank you, {Name}, for contacting us! Your message has been received successfully.";
			return View("Index");
		}
	}
}
