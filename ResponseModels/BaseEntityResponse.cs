namespace ElnurSolutions.ResponseModels
{
	public class BaseEntityResponse<Entity> : BaseResponse
	{
		public Entity? entity { get; set; }
		public int TotalRecords { get; set; }
	}
}
