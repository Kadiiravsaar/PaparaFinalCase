using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Papara.Core.DTOs.Request;
using Papara.Service.Services.Abstract;

namespace Papara.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]

	public class DigitalWalletsController : ControllerBase
	{
		private readonly IDigitalWalletService _digitalWalletService;

		public DigitalWalletsController(IDigitalWalletService digitalWalletService)
		{
			_digitalWalletService = digitalWalletService;
		}


		[HttpGet("{id}/details")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> GetDigitalWalletWithDetails(int id)
		{
			var result = await _digitalWalletService.GetDigitalWalletWithDetailAsync(c => c.Id == id);
			return Ok(result);
		}

		[HttpGet("myWallet")]
		[Authorize(Roles = "Admin,User")]
		public async Task<IActionResult> GetMyWallet()
		{
			var result = await _digitalWalletService.GetWalletByUserId();
			return Ok(result.Data);
		}


		[HttpGet("{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> GetDigitalWallet(int id)
		{
			var response = await _digitalWalletService.GetAsync(p => p.Id == id);
			return Ok(response);
		}


		[HttpGet("details")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> GetAllDigitalWalletsWithDetails()
		{
			var result = await _digitalWalletService.GetAllDigitalWalletsWithDetailAsync(); 
			return Ok(result);
		}


		[HttpGet]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> GetAllDigitalWallets()
		{
			var response = await _digitalWalletService.GetListAsync();
			return Ok(response);
		}


		[HttpPost]
		[Authorize(Roles ="User,Admin")]
		public async Task<IActionResult> CreateWallet()
		{
			var result = await _digitalWalletService.CreateDigitalWalletAsync();
			return Ok(result);
		}


		[HttpPut("{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> UpdateDigitalWallet(int id, [FromBody] DigitalWalletRequestDTO DigitalWalletRequestDTO)
		{
			var response = await _digitalWalletService.UpdateAsync(id, DigitalWalletRequestDTO);
			return StatusCode(response.StatusCode, response);
		}


		[HttpPatch("soft/{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> SoftDeleteDigitalWallet(int id)
		{
			var response = await _digitalWalletService.SoftDeleteAsync(id);
			return StatusCode(response.StatusCode, response);
		}


		[HttpDelete("hard/{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> HardDeleteDigitalWallet(int id)
		{

			await _digitalWalletService.HardDeleteAsync(id);
			return NoContent();
		}
	}
}
