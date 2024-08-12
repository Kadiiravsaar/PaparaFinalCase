using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Papara.Core.DTOs.Request;
using Papara.Core.DTOs.Response;
using Papara.Service.Services.Abstract;

namespace Papara.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class PaymentsController : ControllerBase
	{
		private readonly IPaymentService _paymentService;

		public PaymentsController(IPaymentService paymentService)
		{
			_paymentService = paymentService;
		}

		[HttpPost("process")]
		public async Task<IActionResult> ProcessPayment([FromBody] PaymentRequest request)
		{
			var result = await _paymentService.ProcessPayment(request);
			return Ok(new PaymentResult() { Success = true, Message = result.Message, NewBalance = result.NewBalance });
		}

	}
}
