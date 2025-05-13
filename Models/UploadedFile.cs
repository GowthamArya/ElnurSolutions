using Microsoft.AspNetCore.Http.HttpResults;
using NuGet.Protocol.Plugins;
using System;
using System.ComponentModel.DataAnnotations;

namespace ElnurSolutions.Models
{
	public class UploadedFile
	{
		[Key]
		public Guid Id { get; set; }

		[Required]
		[MaxLength(255)]
		public string FileName { get; set; }

		[MaxLength(100)]
		public string ContentType { get; set; }

		public byte[] FileData { get; set; }

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
	}

}
