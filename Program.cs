using ElnurSolutions.DataBase;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ElnurDbContext>(options => options.UseSqlServer(Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")));

builder.Services.AddControllersWithViews();
builder.Services.AddMemoryCache();

var key = Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]);

// 4. Smart Authentication Scheme
builder.Services.AddAuthentication("SmartScheme")
	.AddPolicyScheme("SmartScheme", "JWT or Cookie", options =>
	{
		options.ForwardDefaultSelector = context =>
		{
			// If the request contains a JWT cookie, use JWT, otherwise fall back to cookies
			return context.Request.Cookies.ContainsKey("jwt")
				? JwtBearerDefaults.AuthenticationScheme
				: CookieAuthenticationDefaults.AuthenticationScheme;
		};
	})
	.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = false,
			ValidateAudience = false,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = new SymmetricSecurityKey(key)
		};
		options.Events = new JwtBearerEvents
		{
			OnMessageReceived = context =>
			{
				if (context.Request.Cookies.ContainsKey("jwt"))
				{
					context.Token = context.Request.Cookies["jwt"];
				}
				return Task.CompletedTask;
			},
			OnTokenValidated = context =>
			{
				Console.WriteLine("Token validated for user: " + context.Principal.Identity?.Name);
				return Task.CompletedTask;
			},
			OnAuthenticationFailed = context =>
			{
				Console.WriteLine("JWT auth failed: " + context.Exception?.Message);
				return Task.CompletedTask;
			},
			OnChallenge = context =>
			{
				Console.WriteLine("JWT challenge triggered. Status: " + context.Response.StatusCode);
				context.HandleResponse();
				context.Response.Redirect("/Account/Logout");
				return Task.CompletedTask;
			}
		};
	})
	.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
	{
		options.LoginPath = "/Account/Login";
		options.AccessDeniedPath = "/Account/Login";
	});

builder.Services.AddAuthorization();

var app = builder.Build();

// Pipeline
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseStatusCodePagesWithReExecute("/Home/PageNotFound");

app.Use(async (ctx, next) =>
{
	Console.WriteLine("Authenticated user: " + ctx.User?.Identity?.Name);
	await next();
});

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();
