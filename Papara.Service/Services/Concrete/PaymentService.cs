using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Papara.Core.DTOs.Request;
using Papara.Core.DTOs.Response;
using Papara.Core.Models;
using Papara.Core.UnitOfWorks;
using Papara.Repository.Context;
using Papara.Service.Constants;
using Papara.Service.Rules;
using Papara.Service.Services.Abstract;

namespace Papara.Service.Services.Concrete
{
	public class PaymentService : IPaymentService

	{
		private readonly UserManager<AppUser> _userManager;
		private readonly MsSqlDbContext _context;
		private readonly IUnitOfWork _unitOfWork;
		private readonly PaymentBusinessRules _paymentBusinessRules;

		public PaymentService(UserManager<AppUser> userManager, MsSqlDbContext context, IUnitOfWork unitOfWork, PaymentBusinessRules paymentBusinessRules)
		{
			_userManager = userManager;
			_context = context;
			_unitOfWork = unitOfWork;
			_paymentBusinessRules = paymentBusinessRules;
		}


		public async Task<PaymentResult> ProcessPayment(PaymentRequest request)
		{
			var userId = await _paymentBusinessRules.GetUserIdFromTokenAsync();

			if (!_paymentBusinessRules.ValidateCardDetails(request.CardNumber, request.Cvv, request.ExpiryDate))
				return PaymentFailed(Messages.InvalidCardDetails);
		

			var user = await _paymentBusinessRules.ValidateUserAndWalletAsync(userId, _userManager, _context);

			user.DigitalWallet.Balance += request.Amount;
			_context.Update(user.DigitalWallet);

			await _unitOfWork.CompleteWithTransaction();

			return PaymentSucceeded(user.DigitalWallet.Balance);
		}

		private PaymentResult PaymentFailed(string message) => new PaymentResult { Success = false, Message = message };

		private PaymentResult PaymentSucceeded(decimal newBalance) => new PaymentResult { Success = true, Message = Messages.PaymentSuccessful, NewBalance = newBalance };
	}


}
