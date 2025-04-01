namespace ElnurSolutions.ResponseModels
{
	public class BaseResponse
	{
		public StatusCode StatusCode { get; set; }
		public string? Message { get; set; }
	}
}
