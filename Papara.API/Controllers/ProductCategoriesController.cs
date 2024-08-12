using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Papara.Core.DTOs.Request;
using Papara.Service.Services.Abstract;

namespace Papara.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles = "Admin")]
	public class ProductCategoriesController : ControllerBase
	{
		private readonly IProductCategoryService _productCategoryService;

		public ProductCategoriesController(IProductCategoryService productCategoryService)
		{
			_productCategoryService = productCategoryService;
		}


		[HttpGet("{id}/details")]
		public async Task<IActionResult> GetProductCategoryWithDetails(int id)
		{
			var result = await _productCategoryService.GetProductCategoryWithDetailAsync(c => c.Id == id);
			return Ok(result);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetProductCategory(int id)
		{
			var response = await _productCategoryService.GetAsync(p => p.Id == id);
			return Ok(response);
		}


		[HttpGet("details")]
		public async Task<IActionResult> GetAllProductCategoriesWithDetails()
		{
			var result = await _productCategoryService.GetAllProductCategoriesWithDetailAsync(); 
			return Ok(result);
		}

		[HttpGet]
		public async Task<IActionResult> GetAllProductCategorys()
		{
			var response = await _productCategoryService.GetListAsync();
			return Ok(response);
		}


		[HttpPost]
		public async Task<IActionResult> AddProductCategory([FromBody] ProductCategoryRequestDTO ProductCategoryRequestDTO)
		{
			var response = await _productCategoryService.AddAsync(ProductCategoryRequestDTO);
			return StatusCode(response.StatusCode, response);
		}


		[HttpPut("{id}")]
		public async Task<IActionResult> UpdateProductCategory(int id, [FromBody] ProductCategoryRequestDTO ProductCategoryRequestDTO)
		{
			var response = await _productCategoryService.UpdateProductCategoryAsync(id, ProductCategoryRequestDTO);
			return StatusCode(response.StatusCode, response);
		}


		[HttpPatch("soft/{id}")]
		public async Task<IActionResult> SoftDeleteProductCategory(int id)
		{
			var response = await _productCategoryService.SoftDeleteAsync(id);
			return StatusCode(response.StatusCode, response);
		}


		[HttpDelete("hard/{id}")]
		public async Task<IActionResult> HardDeleteProductCategory(int id)
		{
			await _productCategoryService.HardDeleteAsync(id);
			return NoContent();

		}
	}
}
