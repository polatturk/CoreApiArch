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
        Task<IResponse<User>> Create(UserDto userDto);
        Task<IResponse<User>> Update(User user);
        IResponse<User> Delete(int id);
        IResponse<IEnumerable<UserListDto>> GetByName(string name);
    }
}
