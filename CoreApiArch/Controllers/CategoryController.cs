using Business.Interfaces;
using Business.Services;
using DataAccess.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreApiArch.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        public readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("ListAll")]
        public IActionResult GetAll()
        {
            var categories = _categoryService.ListAll();

            if (!categories.IsSuccess)
            {
                return NotFound("Kategori bulunamadı.");
            }

            return Ok(categories);
        }

        [HttpGet("GetByName")]
        public IActionResult GetByName(string name)
        {
            var result = _categoryService.GetByName(name);

            if (!result.IsSuccess)
            {
                return NotFound("Kategori bulunamadı.");
            }

            return Ok(result);
        }

        [HttpDelete("Delete")]
        public IActionResult Delete(int id)
        {

            var result = _categoryService.Delete(id);
            if (!result.IsSuccess)
            {
                return BadRequest("Silme işlemi başarısız oldu.");
            }

            return Ok(result);
        }

        [HttpPost("Create")]
        public IActionResult Create([FromBody] CategoryDto categoryDto)
        {
            if (categoryDto == null)
            {
                return BadRequest("Kategori tap bilgileri boş olamaz.");
            }

            var result = _categoryService.Create(categoryDto);

            if (!result.Result.IsSuccess)
            {
                return BadRequest("Kategori oluşturulamadı.");
            }
            return Ok(result);

        }
    }
}
