using ElnurSolutions.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;
using ElnurSolutions.DataBase;

namespace ElnurSolutions.Controllers
{
	public class ContactController : Controller
	{
		private readonly ElnurDbContext _context;
		private readonly IConfiguration _config;

		public ContactController(ElnurDbContext context, IConfiguration config)
		{
			_context = context;
			_config = config;
		}
		public IActionResult Index(string? ProductName = "")
		{
			TempData["ProductName"] = ProductName;
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Send(string Name, string Email, string Message)
		{
			if (!string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Email) && !string.IsNullOrEmpty(Message))
			{
				string subject = $"New Contact Form Submission from {Name}";
				string body = $"You have received a new message from the contact form:\n\n" +
							  $"Name: {Name}\n" +
							  $"Email: {Email}\n" +
							  $"Message:\n{Message}";

				//var adminEmails = await _context.AppUsers
				//	.Where(adminUser => adminUser.IsApproved)
				//	.Select(a => a.Username)
				//	.ToListAsync();
				SendEmail(Name, subject, new List<string> { "gowtham.arya999@gmail.com" }, body);
				SendEmail(Name, subject, new List<string> { "ravindra@elnursolutions.com" }, body);

				//if (adminEmails != null && adminEmails.Any())
				//{
				//	SendEmail(Name, subject, adminEmails, body);
				//}
				//else
				//{
				//	SendEmail(Name, subject, new List<string> { "gowtham.arya999@gmail.com" }, body);
				//}

				TempData["Message"] = $"Thank you, {Name}, for contacting us! Your message has been received successfully.";
			}

			return RedirectToAction("Index");
		}


		public void SendEmail(string Name, string Subject, List<string> Emails, string Message)
		{
			MailMessage mail = new MailMessage();
			mail.From = new MailAddress("info@elnursolutions.com");
			foreach (var email in Emails)
			{
				mail.To.Add(email);
			}
			mail.Subject = Subject;
			mail.Body = Message;

			SmtpClient smtp = new SmtpClient("smtpout.secureserver.net", 587);
			smtp.EnableSsl = true;
			smtp.UseDefaultCredentials = false;
			smtp.Credentials = new NetworkCredential("info@elnursolutions.com", "Elnur@info");

			smtp.Send(mail);
		}
	}
}
