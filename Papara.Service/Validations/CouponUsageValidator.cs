using FluentValidation;
using Papara.Core.DTOs.Request;


namespace Papara.Service.Validations
{
	public class CouponUsageValidator : AbstractValidator<CouponUsageRequestDTO>
	{

		public CouponUsageValidator()
		{
			RuleFor(cu => cu.CouponId)
				.GreaterThan(0).WithMessage("Coupon ID must be greater than zero.");

			RuleFor(cu => cu.UsedDate)
				.NotEmpty().WithMessage("Used date is required.")
				.GreaterThan(DateTime.Now).WithMessage("Used date must be in the future to schedule the use of the coupon.");

			RuleFor(cu => cu.UsedDate)
				.LessThanOrEqualTo(DateTime.Now.AddYears(1)).WithMessage("Used date must be within one year from today.");
		}
	}
}

//{
//	"CouponId": 123,
//  "UserId": "e1b2f4c-974e-488e-a0aa-a320627f804b",
//  "UsedDate": "2024-08-05T14:00:00Z"
//}

