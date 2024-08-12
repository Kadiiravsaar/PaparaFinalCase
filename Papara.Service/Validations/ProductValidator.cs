using FluentValidation;
using Papara.Core.DTOs.Request;


namespace Papara.Service.Validations
{
	public class ProductValidator : AbstractValidator<ProductRequestDTO>
	{
		public ProductValidator()
		{
			RuleFor(p => p.Name)
				.NotEmpty().WithMessage("Product name is required.")
				.MaximumLength(100).WithMessage("Product name must not exceed 100 characters.");

			RuleFor(p => p.Description)
				.MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

			RuleFor(p => p.Price)
				.GreaterThan(0).WithMessage("Price must be greater than zero.")
				.ScalePrecision(2, 18).WithMessage("Price must be a decimal with up to 18 digits and 2 decimal places.");

			RuleFor(p => p.PointsPercentage)
				.InclusiveBetween(0, 100).WithMessage("Points percentage must be between 0 and 100.")
				.ScalePrecision(2, 5).WithMessage("Points percentage must be a decimal with up to 5 digits and 2 decimal places.");

			RuleFor(p => p.MaxPoint)
				.GreaterThanOrEqualTo(0).WithMessage("Max points must be non-negative.")
				.ScalePrecision(2, 18).WithMessage("Max points must be a decimal with up to 18 digits and 2 decimal places.");


			RuleFor(p => p.CategoryIds)
			.NotEmpty().WithMessage("At least one category must be selected.")
			.Must(cids => cids.All(id => id > 0)).WithMessage("All category IDs must be greater than zero.");

		}
	}
}