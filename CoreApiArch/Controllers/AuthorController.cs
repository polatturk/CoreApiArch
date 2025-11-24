using Business.Interfaces;
using Core.DTOs;
using Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace CoreApiArch.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
       
        public readonly IAuthorService _authorService;

        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet("ListAll")]
        public IActionResult GetAll()
        {
            var authors = _authorService.ListAll();

            if (!authors.IsSuccess)
            {
                return NotFound("Yazar bulunamadı.");
            }

            return Ok(authors);
        }

        [HttpGet("GetByName")]
        public IActionResult GetByName(string name)
        {
            var result = _authorService.GetByName(name);

            if (!result.IsSuccess)
            {
                return NotFound("Yazar bulunamadı.");
            }

            return Ok(result);
        }

        [HttpDelete("Delete")]
        public IActionResult Delete(int id)
        {

            var result = _authorService.Delete(id);
            if(!result.IsSuccess)
            {
                return BadRequest("Silme işlemi başarısız oldu.");
            }

            return Ok(result);
        }

        [HttpPost("Create")]
        public IActionResult Create([FromBody] AuthorDto author)
        {
            if (author == null)
            {
                return BadRequest("Yazar bilgileri boş olamaz.");

            }

            var result = _authorService.Create(author);

            if (!result.Result.IsSuccess)
            {
                return BadRequest("Yazar oluşturulamadı.");
            }
            return Ok(result);

        }

        [HttpPut("Update")]
        public IActionResult Update([FromBody] AuthorUpdateDto authorUpdateDto)
        {
            if (authorUpdateDto == null)
            {
                return BadRequest("Yazar bilgileri boş olamaz.");
            }

            var result = _authorService.Update(authorUpdateDto);

            if (!result.Result.IsSuccess)
            {
                return BadRequest("Yazar güncellenemedi.");
            }
            return Ok(result);

        }
    }
}
