using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using ElnurSolutions.DataBase;
using ElnurSolutions.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ElnurSolutions.ResponseModels;

namespace ElnurSolutions.Controllers
{
	public class AccountController : Controller
	{
		private readonly ElnurDbContext _context;
		private readonly IConfiguration _config;

		public AccountController(ElnurDbContext context, IConfiguration config)
		{
			_context = context;
			_config = config;
		}

		// GET: /Account/Register
		public IActionResult Register() => View();

		// POST: /Account/Register
		[HttpPost]
		public async Task<IActionResult> Register(UserDto model)
		{
			if (_context.AppUsers.Any(u => u.Username == model.Username))
			{
				ModelState.AddModelError("", "User already exists");
				return View();
			}

			var hashed = HashPassword(model.Password);
			var user = new AppUser { Username = model.Username, PasswordHash = hashed };

			_context.AppUsers.Add(user);
			await _context.SaveChangesAsync();

			TempData["Message"] = "Registration successful! Please login.";
			return RedirectToAction("Login");
		}

		// GET: /Account/Login
		public IActionResult Login() {
			if (User.Identity != null && User.Identity.IsAuthenticated)
			{
				return RedirectToAction("Index");
			}
			return View();
		}

		// POST: /Account/Login
		[HttpPost]
		public IActionResult Login(UserDto model)
		{
			var user = _context.AppUsers.FirstOrDefault(u => u.Username == model.Username);
			if (user == null || user.PasswordHash != HashPassword(model.Password))
			{
				ModelState.AddModelError("", "Invalid username or password");
				TempData["Message"] = "Invalid username or password.";
				return View();
			}
			var token = GenerateJwtToken(user);
			TempData["Token"] = token;
			Response.Cookies.Delete("jwt");
			Response.Cookies.Append("jwt", token, new CookieOptions
			{
				HttpOnly = true,
				Secure = false, // Set to true in production
				SameSite = SameSiteMode.Strict,
				Expires = DateTime.UtcNow.AddHours(1)
			});

			return RedirectToAction("Index");
		}

		[Authorize]
		[HttpGet]
		public async Task<IActionResult> Index()
		{
			AdminDashBoardModel adminDashBoardModel = new AdminDashBoardModel
			{
				Products = await _context.Products.Include(p => p.ProductCategory).ToListAsync(),
				Categories = await _context.ProductCategories.ToListAsync()
			};

			ViewBag.Token = TempData["Token"];

			return View(adminDashBoardModel);
		}

		public IActionResult Logout()
		{
			Response.Cookies.Delete("jwt");
			return RedirectToAction("Login");
		}

		public async Task<BaseEntityResponse<string>> UploadToCloudinary(IFormFile file)
		{
			var response = new BaseEntityResponse<string>();
			var account = new Account("gowthamarya", "264251474786551", "hTcFJ_-7_rIrOAimNPeNbCGOXHk");
			var cloudinary = new Cloudinary(account);

			await using var stream = file.OpenReadStream();
			var uploadParams = new ImageUploadParams()
			{
				File = new FileDescription(file.FileName, stream),
				PublicId = Guid.NewGuid().ToString()
			};
			var uploadResult = await cloudinary.UploadAsync(uploadParams);
			response.entity = uploadResult.SecureUrl.ToString();
			return response; 
		}

		private string HashPassword(string password)
		{
			using var sha = SHA256.Create();
			return Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(password)));
		}

		private string GenerateJwtToken(AppUser user)
		{
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				claims: new[] { new Claim(ClaimTypes.Name, user.Username) },
				expires: DateTime.UtcNow.AddMinutes(60),
				signingCredentials: creds);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}
