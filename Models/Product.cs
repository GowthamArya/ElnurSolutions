using System.ComponentModel.DataAnnotations.Schema;

namespace ElnurSolutions.Models
{
	public class Product : BaseEntity
	{
		public string? Name { get; set; } 
		public string? Description { get; set; }
		public string? RichTextArea { get; set; }
		public string? ImageGuid { get; set; }

		[ForeignKey("ProductCategory")]
		public int ProductCategoryId { get; set; }
		

		#region Navigation property
		public ProductCategory? ProductCategory { get; set; } 
		#endregion
	}
}
