using Business.Interfaces;
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

            if (authors == null)
            {
                return BadRequest("Yazar bulunamadı.");
            }

            return Ok(authors);
        }

        [HttpGet("GetByName")]

        public IActionResult GetByName(string name)
        {
            var result = _authorService.GetByName(name);

            if (result == null)
            {
                return BadRequest("Yazar bulunamadı.");
            }

            return Ok(result);
        }

        [HttpDelete("Delete")]
        public IActionResult Delete(int id)
        {

            var result = _authorService.Delete(id);
            if(result == null)
            {
                return BadRequest("Silme işlemi başarısız oldu.");
            }

            return Ok(result);
        }

        [HttpPost("Create")]
        public IActionResult Create([FromBody] Author author)
        {
            if (author == null)
            {
                return BadRequest("Yazar bilgileri boş olamaz.");

            }

            var result = _authorService.Create(author);

            if (result == null)
            {
                return BadRequest("Yazar oluşturulamadı.");
            }
            return Ok(result);

        }
    }
}
