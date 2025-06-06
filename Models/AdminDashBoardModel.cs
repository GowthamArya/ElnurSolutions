﻿using ElnurSolutions.Models;

namespace ElnurSolutions.Models
{
	public class AdminDashBoardModel
	{
		public List<ProductCategory> Categories { get; set; }
		public List<Product> Products { get; set; }
		public List<TeamMember> TeamMembers { get; set; }
		public List<TeamCategory> TeamCategories { get; set; }
	}
}