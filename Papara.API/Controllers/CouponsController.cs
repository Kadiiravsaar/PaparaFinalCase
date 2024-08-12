using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Papara.Core.DTOs.Request;
using Papara.Service.Services.Abstract;

namespace Papara.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles = "Admin")]
	public class CouponsController : ControllerBase
	{
		private readonly ICouponService _couponService;

		public CouponsController(ICouponService couponService)
		{
			_couponService = couponService;
		}


		[HttpGet("{id}/details")]
		public async Task<IActionResult> GetCouponWithDetails(int id)
		{
			var result = await _couponService.GetCouponWithDetailAsync(c => c.Id == id);
			return Ok(result);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetCoupon(int id)
		{
			var response = await _couponService.GetAsync(p => p.Id == id);
			return Ok(response);
		}


		[HttpGet("details")]
		public async Task<IActionResult> GetAllCouponsWithDetails()
		{
			var result = await _couponService.GetAllCouponsWithDetailAsync();
			return Ok(result);
		}

		[HttpGet]
		public async Task<IActionResult> GetAllCoupons()
		{
			var response = await _couponService.GetListAsync();
			return Ok(response);
		}



		// rolü sadece admin olanlar 
		[HttpPost]
		public async Task<IActionResult> AddCoupon([FromBody] CouponRequestDTO CouponRequestDTO)
		{
			var response = await _couponService.AddAsync(CouponRequestDTO);
			return StatusCode(response.StatusCode, response);
		}


		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateCoupon(int id, [FromBody] CouponRequestDTO CouponRequestDTO)
		{
			var response = await _couponService.UpdateAsync(id, CouponRequestDTO);
			return StatusCode(response.StatusCode, response);
		}


		[HttpPatch("soft/{id}")]
		public async Task<IActionResult> SoftDeleteCoupon(int id)
		{
			var response = await _couponService.SoftDeleteAsync(id);
			return StatusCode(response.StatusCode, response);
		}


		[HttpDelete("hard/{id}")]
		public async Task<IActionResult> HardDeleteCoupon(int id)
		{
			await _couponService.HardDeleteAsync(id);
			return NoContent();
		}
	}
}
