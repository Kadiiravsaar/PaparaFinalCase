using FluentValidation;
using Papara.Core.DTOs.Request;


namespace Papara.Service.Validations
{
	public class OrderDetailValidator : AbstractValidator<OrderDetailRequestDTO>
	{
		public OrderDetailValidator()
		{
			RuleFor(od => od.ProductId)
				.GreaterThan(0)
				.WithMessage("Product ID must be greater than zero.");

			RuleFor(od => od.Quantity)
				.GreaterThan(0)
				.WithMessage("Quantity must be greater than zero.");
		}
	}
}
