using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Papara.Core.DTOs.Request;
using Papara.Service.Services.Abstract;

namespace Papara.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles = "Admin")]
	public class CouponUsagesController : ControllerBase
	{
		private readonly ICouponUsageService _couponUsageService;

		public CouponUsagesController(ICouponUsageService couponUsageService)
		{
			_couponUsageService = couponUsageService;
		}


		[HttpGet("{id}/details")]
		public async Task<IActionResult> GetCouponUsageWithDetails(int id)
		{
			var result = await _couponUsageService.GetCouponUsageWithDetailAsync(c => c.Id == id);
			return Ok(result);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetCouponUsage(int id)
		{
			var response = await _couponUsageService.GetAsync(p => p.Id == id);
			return Ok(response);
		}



		[HttpGet("details")]
		public async Task<IActionResult> GetAllCouponUsagesWithDetails()
		{
			var result = await _couponUsageService.GetAllCouponUsagesWithDetailAsync(); 
			return Ok(result);
		}

		[HttpGet]
		public async Task<IActionResult> GetAllCouponUsages()
		{
			var response = await _couponUsageService.GetListAsync();
			return Ok(response);
		}


	
		[HttpPost]
		public async Task<IActionResult> AddCouponUsage([FromBody] CouponUsageRequestDTO CouponUsageRequestDTO)
		{
			var response = await _couponUsageService.AddAsync(CouponUsageRequestDTO);
			return StatusCode(response.StatusCode, response);
		}


		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateCouponUsage(int id, [FromBody] CouponUsageRequestDTO CouponUsageRequestDTO)
		{
			var response = await _couponUsageService.UpdateAsync(id, CouponUsageRequestDTO);
			return StatusCode(response.StatusCode, response);
		}


		[HttpPatch("soft/{id}")]
		public async Task<IActionResult> SoftDeleteCouponUsage(int id)
		{
			var response = await _couponUsageService.SoftDeleteAsync(id);
			return StatusCode(response.StatusCode, response);
		}


		[HttpDelete("hard/{id}")]
		public async Task<IActionResult> HardDeleteCouponUsage(int id)
		{
			await _couponUsageService.HardDeleteAsync(id);
			return NoContent();
		}
	}
}
