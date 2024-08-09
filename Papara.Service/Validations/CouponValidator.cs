using FluentValidation;
using Papara.Core.DTOs.Request;


namespace Papara.Service.Validations
{
	public class CouponValidator : AbstractValidator<CouponRequestDTO>
	{
		public CouponValidator()
		{
			RuleFor(c => c.CouponCode)
				.NotEmpty().WithMessage("Coupon code is required.")
				.Length(1, 10).WithMessage("Coupon code must be between 1 and 10 characters long.");

			RuleFor(c => c.Amount)
				.GreaterThan(0).WithMessage("Amount must be greater than zero.")
				.ScalePrecision(2, 18).WithMessage("Amount must be a decimal with up to 18 digits and 2 decimal places.");

			RuleFor(c => c.ExpiryDate)
				.NotEmpty().WithMessage("Expiry date is required.")
				.GreaterThan(DateTime.Now).WithMessage("Expiry date must be in the future.")
				.LessThan(DateTime.Now.AddMonths(1)).WithMessage("Expiry date must be within one month from today.");
		}

	}
}
