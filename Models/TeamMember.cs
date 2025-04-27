namespace ElnurSolutions.Models
{
	public class TeamMember
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public string ImageGuid { get; set; }
		public int TeamCategoryId { get; set; }
		public DateTime CreationDate { get; set; } = DateTime.Now;
		public DateTime LastUpdate { get; set; } = DateTime.Now;
		public TeamCategory TeamCategory { get; set; }
	}
}
