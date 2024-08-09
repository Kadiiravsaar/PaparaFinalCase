using FluentValidation;
using Papara.Core.DTOs.Request;


namespace Papara.Service.Validations
{
	public class BasketItemValidator : AbstractValidator<BasketItemRequestDTO>
	{
		public BasketItemValidator()
		{
			RuleFor(x => x.ProductId)
				.NotEmpty().WithMessage("Product ID must be provided.");

			RuleFor(x => x.Quantity)
				.GreaterThan(0).WithMessage("Quantity must be greater than zero.");

			RuleFor(x => x.BasketId)
				.NotEmpty().WithMessage("Basket ID must be provided.");

		}
	}

}
