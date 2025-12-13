using Business.Interfaces;
using Core.DTOs;
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
    public class BookController : ControllerBase
    {
        public readonly IBookService _bookService;
        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [AllowAnonymous]
        [EnableRateLimiting("RateLimiter2")]
        [HttpGet("ListAll")]
        public IActionResult GetAll()
        {
            var books = _bookService.ListAll();

            if (!books.IsSuccess)
            {
                return NotFound(books.Message);
            }

            return Ok(books);
        }


        [AllowAnonymous]
        [EnableRateLimiting("RateLimiter2")]
        [HttpGet("GetByName")]
        public IActionResult GetByName(string name)
        {
            var result = _bookService.GetByName(name);

            if (!result.IsSuccess)
            {
                return NotFound(result.Message);
            }

            return Ok(result);
        }

        [HttpDelete("Delete")]
        public IActionResult Delete(int id)
        {

            var result = _bookService.Delete(id);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
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
                return BadRequest(result.Result.Message);
            }
            return Ok(result);

        }

        [HttpPut("Update")]
        public IActionResult Update([FromBody] BookUpdateDto bookUpdateDto)
        {
            if (bookUpdateDto == null)
            {
                return BadRequest("Kitap bilgileri boş olamaz.");

            }

            var result = _bookService.Update(bookUpdateDto);

            if (!result.Result.IsSuccess)
            {
                return BadRequest(result.Result.Message);
            }
            return Ok(result);

        }

        [AllowAnonymous]
        [HttpGet("GetBooksByCategoryId")]
        public IActionResult GetBooksByCategoryId(int categoryId) 
        {
            var result = _bookService.GetBooksByCategoryId(categoryId);

            if (!result.IsSuccess)
            {
                return NotFound(result.Message);
            }

            return Ok(result);
        }

        [AllowAnonymous]
        [HttpGet("GetBooksByAuthorId")]
        public IActionResult GetBooksByAuthorId(int authorId)
        {
            var result = _bookService.GetBooksByAuthorId(authorId);

            if (!result.IsSuccess)
            {
                return NotFound(result.Message);
            }

            return Ok(result);
        }

    }
}
