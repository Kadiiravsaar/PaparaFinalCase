using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Papara.Core.DTOs.Request;
using Papara.Service.Services.Abstract;

namespace Papara.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class OrdersController : ControllerBase
	{
		private readonly IOrderService _orderService;

		public OrdersController(IOrderService orderService)
		{
			_orderService = orderService;
		}


		[HttpGet("{id}/details")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> GetOrderWithDetails(int id)
		{
			var result = await _orderService.GetOrderWithDetailAsync(c => c.Id == id);
			return Ok(result);
		}


		[HttpGet("{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> GetOrder(int id)
		{
			var response = await _orderService.GetAsync(p => p.Id == id);
			return Ok(response);
		}


		[HttpGet("details")]
		[Authorize(Roles = "Admin")]

		public async Task<IActionResult> GetAllOrdersWithDetails()
		{
			var result = await _orderService.GetAllOrdersWithDetailAsync(); 
			return Ok(result);
		}


		[HttpGet]
		[Authorize(Roles = "Admin")]

		public async Task<IActionResult> GetAllOrders()
		{
			var response = await _orderService.GetListAsync();
			return Ok(response);
		}


		[HttpPost]
		[Authorize(Roles = "Admin,User")]
		public async Task<IActionResult> CreateOrderAsync()
		{ 
			var response = await _orderService.CreateOrder();
			return StatusCode(response.StatusCode, response);
		}


		[HttpGet("myOrders")]
		[Authorize(Roles = "Admin,User")]
		public async Task<IActionResult> GetMyOrders()
		{
			var result = await _orderService.GetMyOrders();
			return Ok(result);
		}


		[HttpPut("{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> UpdateOrder(int id, [FromBody] OrderRequestDTO OrderRequestDTO)
		{
			var response = await _orderService.UpdateAsync(id, OrderRequestDTO);
			return StatusCode(response.StatusCode, response);
		}


		[HttpPatch("soft/{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> SoftDeleteOrder(int id)
		{
			var response = await _orderService.SoftDeleteAsync(id);
			return StatusCode(response.StatusCode, response);
		}


		[HttpDelete("hard/{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> HardDeleteOrder(int id)
		{
			await _orderService.HardDeleteAsync(id);
			return NoContent();
		}
	}
}
