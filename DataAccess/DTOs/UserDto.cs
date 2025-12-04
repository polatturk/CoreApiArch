using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class UserDto
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public string? Email { get; set; }
        public string Password { get; set; }
    }
    public class UserLoginDto
    {
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string Password { get; set; }
    }   
    public class UserListDto
    {
        public int Id { get; set; }
        public DateTime RecordDate { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string Password { get; set; }
    }
    public class UserUpdateDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }

}
