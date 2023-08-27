using System;
namespace GG_shopping_cart.DTO
{
	public class ResponseDto
	{
		public bool IsSuccess { get; set; } = true;

		public List<string>? Errors { get; set; }

		public object? Result { get; set; }

		public string? Message { get; set; }
	}
}

