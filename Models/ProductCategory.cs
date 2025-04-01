namespace ElnurSolutions.Models
{
	public class ProductCategory
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public DateTime CreationDate { get; set; }
		public DateTime? LastUpdate { get; set; }
	}
}
