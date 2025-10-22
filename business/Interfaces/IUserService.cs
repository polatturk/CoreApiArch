using Business.Response;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IUserService
    {
        IResponse<IEnumerable<User>> ListAll();
        IResponse<User> GetById(int id);
        Task<IResponse<User>> Create(User user);
        Task<IResponse<User>> Update(User user);
        IResponse<User> Delete(int id);
        IResponse<IEnumerable<User>> GetByName(string name);
    }
}
