using Business.Interfaces;
using Business.Services;
using DataAccess.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoreApiArch.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("ListAll")]
        public IActionResult GetAll()
        {
            var users = _userService.ListAll();

            if (!users.IsSuccess)
            {
                return NotFound("Kullanıcı bulunamadı.");
            }

            return Ok(users);
        }

        [HttpGet("GetByName")]
        public IActionResult GetByName(string name)
        {
            var result = _userService.GetByName(name);

            if (!result.IsSuccess)
            {
                return NotFound("Kullanıcı bulunamadı.");
            }

            return Ok(result);
        }

        [HttpDelete("Delete")]
        public IActionResult Delete(int id)
        {

            var result = _userService.Delete(id);
            if (!result.IsSuccess)
            {
                return BadRequest("Silme işlemi başarısız oldu.");
            }

            return Ok(result);
        }

        [HttpPost("Create")]
        public IActionResult Create([FromBody] UserDto userDto)
        {
            if (userDto == null)
            {
                return BadRequest("Kullanıcı bilgileri boş olamaz.");
            }

            var result = _userService.Create(userDto);

            if (!result.Result.IsSuccess)
            {
                return BadRequest("Kullanıcı oluşturulamadı.");
            }
            return Ok(result);

        }
    }
}
