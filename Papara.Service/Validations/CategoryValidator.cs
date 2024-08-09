using FluentValidation;
using Papara.Core.DTOs.Request;


namespace Papara.Service.Validations
{
	public class CategoryValidator : AbstractValidator<CategoryRequestDTO>
	{
		public CategoryValidator()
		{
			RuleFor(c => c.Name)
				.NotEmpty().WithMessage("Category name is required.")
				.MaximumLength(100).WithMessage("Category name must not exceed 100 characters.");

			RuleFor(c => c.Url)
				.NotEmpty().WithMessage("Category URL is required.")
				.MaximumLength(200).WithMessage("Category URL must not exceed 200 characters.");

			RuleFor(c => c.Tags)
				.MaximumLength(500).WithMessage("Tags must not exceed 500 characters.");
			
		}
	}
}
