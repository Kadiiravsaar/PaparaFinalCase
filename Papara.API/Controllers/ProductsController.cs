using Microsoft.AspNetCore.Authentication.JwtBearer;
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
	public class ProductsController : ControllerBase
	{
		private readonly IProductService _productService;

		public ProductsController(IProductService productService)
		{
			_productService = productService;
		}
		

		[HttpGet("{id}/details")]
		public async Task<IActionResult> GetProductWithDetails(int id)
		{
			var result = await _productService.GetProductWithDetailAsync(c => c.Id == id);
			return Ok(result);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetProduct(int id)
		{
			var response = await _productService.GetAsync(p => p.Id == id);
			return Ok(response);
		}

		[HttpGet("search")]
		public async Task<IActionResult> GetProductsByName([FromQuery] string name)
		{
			var products = await _productService.GetProductsByNameAsync(name);
			return Ok(products);
		}

		[HttpGet("details")]
		public async Task<IActionResult> GetAllProductsWithDetails()
		{
			var result = await _productService.GetAllProductsWithDetailAsync();
			return Ok(result);
		}

		[HttpGet]
		public async Task<IActionResult> GetAllProducts()
		{
			var response = await _productService.GetListAsync();
			return Ok(response);
		}


		[HttpPost]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> AddProduct([FromBody] ProductRequestDTO productRequestDTO)
		{
			var response = await _productService.AddProductAsync(productRequestDTO);
			return StatusCode(response.StatusCode, response);
		}

		[HttpDelete("removeCategory")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> RemoveCategoryFromProduct(int productId, int categoryId)
		{
			var result = await _productService.RemoveCategoryFromProduct(productId, categoryId);
			return Ok(result); 
		}


		[HttpPut("{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> UpdateProductAsync(int id, [FromBody] ProductRequestDTO productRequestDTO)
		{
			var response = await _productService.UpdateProductAsync(id, productRequestDTO);
			return StatusCode(response.StatusCode, response);
		}


		[HttpPatch("soft/{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> SoftDeleteProduct(int id)
		{
			var response = await _productService.SoftDeleteAsync(id);
			return StatusCode(response.StatusCode, response);
		}


		[HttpDelete("hard/{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> HardDeleteProduct(int id)
		{

			await _productService.HardDeleteAsync(id);
			return NoContent();
		}
	}
}
