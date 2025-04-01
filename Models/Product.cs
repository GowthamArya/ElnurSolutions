namespace ElnurSolutions.Models
{
	public class Product
	{
		public int Id { get; set; } 
		public string Name { get; set; } 
		public string Description { get; set; }
		public string RichTextArea { get; set; }
		public int ProductSubCategoryId { get; set; }
		public DateTime CreationDate { get; set; }
		public DateTime? LastUpdate { get; set; }

		#region Navigation property
		public ProductSubCategory? ProductSubCategory { get; set; } 
		#endregion
	}

}
