using Business.Interfaces;
using Core.DTOs;
using DataAccess.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreApiArch.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        public readonly IBookService _bookService;
        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet("ListAll")]
        public IActionResult GetAll()
        {
            var books = _bookService.ListAll();

            if (!books.IsSuccess)
            {
                return NotFound("Kitap bulunamadı.");
            }

            return Ok(books);
        }

        [HttpGet("GetByName")]
        public IActionResult GetByName(string name)
        {
            var result = _bookService.GetByName(name);

            if (!result.IsSuccess)
            {
                return NotFound("Kitap bulunamadı.");
            }

            return Ok(result);
        }

        [HttpDelete("Delete")]
        public IActionResult Delete(int id)
        {

            var result = _bookService.Delete(id);
            if (!result.IsSuccess)
            {
                return BadRequest("Silme işlemi başarısız oldu.");
            }

            return Ok(result);
        }

        [HttpPost("Create")]
        public IActionResult Create([FromBody] BookDto book)
        {
            if (book == null)
            {
                return BadRequest("Kitap bilgileri boş olamaz.");

            }

            var result = _bookService.Create(book);

            if (!result.Result.IsSuccess)
            {
                return BadRequest("Kitap oluşturulamadı.");
            }
            return Ok(result);

        }

        [HttpGet("GetBooksByCategoryId")]
        public IActionResult GetBooksByCategoryId(int categoryId) 
        {
            var result = _bookService.GetBooksByCategoryId(categoryId);

            if (!result.IsSuccess)
            {
                return NotFound("Kitap Bulunamadı.");
            }

            return Ok(result);
        }

    }
}
