namespace ElnurSolutions.Models
{
	public class ProductSubCategory
	{
		public int Id { get; set; } 
		public string Name { get; set; } 
		public string Description { get; set; } 
		public int ProductCategoryId { get; set; } 
		public DateTime CreationDate { get; set; }
		public DateTime? LastUpdate { get; set; }

		#region Navigation properties
		public ProductCategory? ProductCategory { get; set; }
		#endregion
	}

}
