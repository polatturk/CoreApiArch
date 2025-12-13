using Business.Interfaces;
using Business.Services;
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
    public class UserController : ControllerBase
    {
        public readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [EnableRateLimiting("RateLimiter2")]
        [HttpGet("ListAll")]
        public IActionResult GetAll()
        {
            var users = _userService.ListAll();

            if (!users.IsSuccess)
            {
                return NotFound(users.Message);
            }

            return Ok(users);
        }

        [AllowAnonymous]
        [EnableRateLimiting("RateLimiter2")]
        [HttpGet("GetByName")]
        public IActionResult GetByName(string name)
        {
            var result = _userService.GetByName(name);

            if (!result.IsSuccess)
            {
                return NotFound(result.Message);
            }

            return Ok(result);
        }

        [HttpDelete("Delete")]
        public IActionResult Delete(int id)
        {

            var result = _userService.Delete(id);
            if (!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }

            return Ok(result);
        }

        [HttpPost("Create")]
        public IActionResult Create([FromBody] UserDto user)
        {
            if (user == null)
            {
                return BadRequest("Kullanıcı bilgileri boş olamaz.");
            }

            var result = _userService.Create(user);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }

        [HttpPost("Login")]
        public IActionResult Login([FromBody] UserLoginDto user)
        {
            if (user == null)
            {
                return BadRequest("Kullanıcı bilgileri boş olamaz.");
            }

            var result = _userService.Login(user);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result.Message);
            }
        }

        [HttpPut("Update")]
        public IActionResult Update([FromBody] UserUpdateDto userUpdateDto)
        {
            if (userUpdateDto == null)
            {
                return BadRequest("Kullanıcı bilgileri boş olamaz.");
            }

            var result = _userService.Update(userUpdateDto);

            if (!result.Result.IsSuccess)
            {
                return BadRequest(result.Result.Message);
            }
            return Ok(result);

        }
    }
}
