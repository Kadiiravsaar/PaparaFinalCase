using FluentValidation;
using Papara.Core.DTOs.Request;


namespace Papara.Service.Validations
{
	public class ProductCategoryValidator : AbstractValidator<ProductCategoryRequestDTO>
	{
		public ProductCategoryValidator()
		{
			RuleFor(pc => pc.ProductId)
				.GreaterThan(0)
				.WithMessage("Product ID must be greater than zero to be valid.");

			RuleFor(pc => pc.CategoryId)
				.GreaterThan(0)
				.WithMessage("Category ID must be greater than zero to be valid.");
		}
	}
}

