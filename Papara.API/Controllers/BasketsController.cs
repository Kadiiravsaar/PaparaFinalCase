using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Papara.Core.DTOs.Request;
using Papara.Service.Services.Abstract;

namespace Papara.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BasketsController : ControllerBase
	{
		private readonly IBasketService _basketService;

		public BasketsController(IBasketService basketService)
		{
			_basketService = basketService;
		}


		[HttpGet("{id}/details")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> GetBasketWithDetails(int id)
		{
			var result = await _basketService.GetBasketWithDetailAsync(c => c.Id == id);
			return Ok(result);
		}


		[HttpGet("{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> GetBasket(int id)
		{
			var response = await _basketService.GetBasketAsync(p => p.Id == id);
			return Ok(response);
		}


		[HttpGet("details")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> GetAllBasketsWithDetails()
		{
			var result = await _basketService.GetAllBasketsWithDetailAsync();
			return Ok(result);
		}


		[HttpGet]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> GetAllBaskets()
		{
			var response = await _basketService.GetAllBasketsAsync();
			return Ok(response);
		}



		[HttpGet("currentBasket")]
		[Authorize(Roles = "Admin,User")]
		public async Task<IActionResult> GetCurrentUserBasket()
		{
			var result = await _basketService.GetCurrentUserBasket();
			return Ok(result);
		}


		[HttpPost("calculatePrice")]
		[Authorize(Roles = "Admin,User")]
		public async Task<IActionResult> CalculateBasketItemsPriceAsync([FromBody] BasketRequestDTO basketRequestDTO)
		{
			var response = await _basketService.CalculateBasketItemsPriceAsync(basketRequestDTO);
			return Ok(response.Data);
		}



		[HttpPut("{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> UpdateBasket(int id, [FromBody] BasketRequestDTO BasketRequestDTO)
		{
			//var existingBasketResponse = await _BasketService.GetAsync(p => p.Id == id);
			var response = await _basketService.UpdateBasketAsync(id, BasketRequestDTO);
			return StatusCode(response.StatusCode, response);
		}


		[HttpPatch("soft/{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> SoftDeleteBasket(int id)
		{
			var response = await _basketService.SoftDeleteAsync(id);
			return StatusCode(response.StatusCode, response);
		}



		[HttpDelete("hard/{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> HardDeleteBasket(int id)
		{
			await _basketService.HardDeleteAsync(id);
			return NoContent();
		}
	}
}
