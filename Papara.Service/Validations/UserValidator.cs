using FluentValidation;
using Papara.Core.DTOs.Request;
using Papara.Core.Models;


namespace Papara.Service.Validations
{
	public class RegisterRequestValidator : AbstractValidator<RegisterRequestDTO>
	{
		public RegisterRequestValidator()
		{
			RuleFor(x => x.Email)
				.NotEmpty().WithMessage("Email address is required.")
				.EmailAddress().WithMessage("Please enter a valid email address.");

			RuleFor(x => x.UserName)
				.NotEmpty().WithMessage("Username is required.");

			RuleFor(x => x.Password)
				.NotEmpty().WithMessage("Password is required.")
				.MinimumLength(6).WithMessage("Password must be at least 6 characters long.");

			RuleFor(x => x.FirstName)
				.NotEmpty().WithMessage("First name is required.")
				.MaximumLength(50).WithMessage("First name must not exceed 50 characters.");

			RuleFor(x => x.LastName)
				.NotEmpty().WithMessage("Last name is required.")
				.MaximumLength(50).WithMessage("Last name must not exceed 50 characters.");
		}
	}
}