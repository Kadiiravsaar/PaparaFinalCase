using FluentValidation;
using Papara.Core.DTOs.Request;


namespace Papara.Service.Validations
{
	public class OrderValidator : AbstractValidator<OrderRequestDTO>
	{
		public OrderValidator()
		{
			RuleFor(o => o.OrderNumber)  // arkada kendisi oluşacak
				.NotEmpty().WithMessage("Order number is required.")
				.Length(9).WithMessage("Order number must be exactly 9 characters.");

			RuleFor(o => o.TotalAmount)
				.GreaterThan(0).WithMessage("Total amount must be greater than zero.")
				.ScalePrecision(2, 18).WithMessage("Total amount must be a decimal with up to 18 digits and 2 decimal places.");


			RuleFor(o => o.PointUsed)
				.GreaterThanOrEqualTo(0).WithMessage("Points used must be non-negative.")
				.ScalePrecision(2, 18).WithMessage("Points used must be a decimal with up to 18 digits and 2 decimal places.");

		}
	}
}
