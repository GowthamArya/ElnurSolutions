namespace ElnurSolutions.Models
{
	public class BaseEntity
	{
		public int Id { get; set; }
		public DateTime? CreationDate { get; set; } = DateTime.Now;
		public DateTime? LastUpdate { get; set; } = DateTime.Now;
	}
}
