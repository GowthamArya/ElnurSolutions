using System.ComponentModel.DataAnnotations;

namespace ElnurSolutions.Models
{
	public class AppUser : BaseEntity
	{
		[Required]
		public string Username { get; set; }

		[Required]
		public string PasswordHash { get; set; }
	}
}

