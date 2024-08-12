using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Papara.Core.DTOs.Request;
using Papara.Service.Services.Abstract;

namespace Papara.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles = "Admin")]
	public class OrderDetailsController : ControllerBase
	{
		private readonly IOrderDetailService _orderDetailService;

		public OrderDetailsController(IOrderDetailService orderDetailService)
		{
			_orderDetailService = orderDetailService;
		}


		[HttpGet("{id}/details")]
		public async Task<IActionResult> GetOrderDetailWithDetails(int id)
		{
			var result = await _orderDetailService.GetOrderDetailWithDetailAsync(c => c.Id == id);
			return Ok(result);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetOrderDetail(int id)
		{
			var response = await _orderDetailService.GetAsync(p => p.Id == id);
			return Ok(response);
		}

		[HttpGet("details")]
		public async Task<IActionResult> GetAllOrderDetailsWithDetails()
		{
			var result = await _orderDetailService.GetAllOrderDetailsWithDetailAsync(); 
			return Ok(result);
		}

		[HttpGet]
		public async Task<IActionResult> GetAllOrderDetails()
		{
			var response = await _orderDetailService.GetListAsync();	
			return Ok(response);
		}


		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateOrderDetail(int id, [FromBody] OrderDetailRequestDTO OrderDetailRequestDTO)
		{
			var response = await _orderDetailService.UpdateAsync(id, OrderDetailRequestDTO);
			return StatusCode(response.StatusCode, response);
		}


		[HttpPatch("soft/{id}")]
		public async Task<IActionResult> SoftDeleteOrderDetail(int id)
		{
			var response = await _orderDetailService.SoftDeleteAsync(id);
			return StatusCode(response.StatusCode, response);
		}


		[HttpDelete("hard/{id}")]
		public async Task<IActionResult> HardDeleteOrderDetail(int id)
		{

			await _orderDetailService.HardDeleteAsync(id);
			return NoContent();
		}
	}
}
