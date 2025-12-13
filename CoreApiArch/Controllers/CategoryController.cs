using Business.Interfaces;
using Business.Services;
using DataAccess.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace CoreApiArch.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        public readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [AllowAnonymous]
        [EnableRateLimiting("RateLimiter2")]
        [HttpGet("ListAll")]
        public IActionResult GetAll()
        {
            var categories = _categoryService.ListAll();

            if (!categories.IsSuccess)
            {
                return NotFound(categories.Message);
            }

            return Ok(categories);
        }

        [AllowAnonymous]
        [EnableRateLimiting("RateLimiter2")]
        [HttpGet("GetByName")]
        public IActionResult GetByName(string name)
        {
            var result = _categoryService.GetByName(name);

            if (!result.IsSuccess)
            {
                return NotFound(result.Message);
            }

            return Ok(result);
        }

        [HttpDelete("Delete")]
        public IActionResult Delete(int id)
        {

            var result = _categoryService.Delete(id);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);
        }

        [HttpPost("Create")]
        public IActionResult Create([FromBody] CategoryDto categoryDto)
        {
            if (categoryDto == null)
            {
                return BadRequest("Kategori bilgileri boş olamaz.");
            }

            var result = _categoryService.Create(categoryDto);

            if (!result.Result.IsSuccess)
            {
                return BadRequest(result.Result.Message);
            }
            return Ok(result);

        }

        [HttpPut("Update")]
        public IActionResult Update([FromBody] CategoryUpdateDto categoryUpdateDto)
        {
            if (categoryUpdateDto == null)
            {
                return BadRequest("Kategori bilgileri boş olamaz.");

            }

            var result = _categoryService.Update(categoryUpdateDto);

            if (!result.Result.IsSuccess)
            {
                return BadRequest(result.Result.Message);
            }
            return Ok(result);

        }
    }
}
