using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Papara.Core.DTOs.Request;
using Papara.Service.Services.Abstract;
using Papara.Service.Services.Concrete;

namespace Papara.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class BasketItemsController : ControllerBase
	{
		private readonly IBasketItemService _basketItemService;

		public BasketItemsController(IBasketItemService basketItemService)
		{
			_basketItemService = basketItemService;
		}


		[HttpGet("{id}/details")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> GetBasketItemWithDetails(int id)
		{
			var result = await _basketItemService.GetBasketItemWithDetailAsync(c => c.Id == id);
			return Ok(result);
		}


		[HttpGet("{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> GetBasketItem(int id)
		{
			var response = await _basketItemService.GetAsync(p => p.Id == id);
			return Ok(response);
		}


		[HttpGet("details")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> GetAllBasketItemsWithDetails()
		{
			var result = await _basketItemService.GetAllBasketItemsWithDetailAsync(); 
			return Ok(result);
		}

		[HttpGet]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> GetAllBasketItems()
		{
			var response = await _basketItemService.GetListAsync();
			return Ok(response);
		}


		[HttpPost]
		[Authorize(Roles = "Admin,User")]
		public async Task<IActionResult> AddBasketItem([FromBody] BasketItemRequestDTO BasketItemRequestDTO)
		{
			var response = await _basketItemService.AddItemToBasketAsync(BasketItemRequestDTO);
			return StatusCode(response.StatusCode, response);
		}


		[Authorize(Roles = "Admin,User")]
		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateBasketItem(int id, [FromBody] BasketItemRequestDTO BasketItemRequestDTO)
		{
			var response = await _basketItemService.UpdateBasketItemAsync(id, BasketItemRequestDTO);
			return StatusCode(response.StatusCode, response);
		}


		[HttpPatch("soft/{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> SoftDeleteBasketItem(int id)
		{
			var response = await _basketItemService.SoftDeleteBasketItemAsync(id);
			return StatusCode(response.StatusCode, response);
		}


		[HttpDelete("hard/{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> HardDeleteBasketItem(int id)
		{
			var response = await _basketItemService.HardDeleteBasketItemAsync(id);
			return StatusCode(response.StatusCode, response);
		}
	}
}
