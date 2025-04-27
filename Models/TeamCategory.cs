namespace ElnurSolutions.Models
{
	public class TeamCategory
	{
		public int Id { get; set; } 
		public string Name { get; set; }
		public DateTime CreationDate { get; set; } = DateTime.Now;
		public DateTime LastUpdate { get; set; } = DateTime.Now;
		public ICollection<TeamMember> TeamMembers { get; set; }

	}
}
