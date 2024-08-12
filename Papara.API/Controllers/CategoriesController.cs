using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Papara.Core.DTOs.Request;
using Papara.Service.Services.Abstract;

namespace Papara.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CategoriesController : ControllerBase
	{

		private readonly ICategoryService _categoryService;

		public CategoriesController(ICategoryService categoryService)
		{ 
			_categoryService = categoryService;
		}

		[HttpGet("{id}/details")]
		public async Task<IActionResult> GetCategoryWithDetails(int id)
		{
			var result = await _categoryService.GetCategoryWithDetailAsync(c => c.Id == id);
			return Ok(result);
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetCategory(int id)
		{
			var result = await _categoryService.GetAsync(x => x.Id == id);

			return Ok(result);
		}


		[HttpGet("details")]
		public async Task<IActionResult> GetAllCategoriesWithDetails()
		{
			var result = await _categoryService.GetAllCategoriesWithDetailAsync();
			return Ok(result);
		}

		[HttpGet]
		public async Task<IActionResult> GetAllCategories()
		{
			var result = await _categoryService.GetListAsync();

			return Ok(result);

		}
			
		[HttpPost]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> AddCategory([FromBody] CategoryRequestDTO categoryDto)
		{
			var result = await _categoryService.AddAsync(categoryDto);
			return Ok(result);
		}


		[HttpPut("{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryRequestDTO categoryDto)
		{
			var result = await _categoryService.UpdateAsync(id, categoryDto);

			return Ok(result);

		}

		[HttpPatch("soft/{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> SoftDeleteCategory(int id)
		{
			var response = await _categoryService.SoftDeleteAsync(id);
			return StatusCode(response.StatusCode, response);
		}


		[HttpDelete("hard/{id}")]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> HardDeleteCategory(int id)
		{
			var response = await _categoryService.HardDeleteAsync(id);

			return StatusCode(response.StatusCode, response);
		}
	}


}
