using Business.Response;
using Core.Entities;
using DataAccess.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IUserService
    {
        IResponse<IEnumerable<UserListDto>> ListAll();
        IResponse<UserListDto> GetById(int id);
        IResponse<UserDto> Create(UserDto userDto);
        public IResponse<string> Login(UserLoginDto user);
        Task<IResponse<UserUpdateDto>> Update(UserUpdateDto userUpdateDto);
        IResponse<User> Delete(int id);
        IResponse<IEnumerable<UserListDto>> GetByName(string name);
    }
}
