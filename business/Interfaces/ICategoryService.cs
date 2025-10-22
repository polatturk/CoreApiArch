using Business.Response;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface ICategoryService
    {
        IResponse<IEnumerable<Category>> ListAll();
        IResponse<Category> GetById(int id);
        Task<IResponse<Category>> Create(Category category);
        Task<IResponse<Category>> Update(Category category);
        IResponse<Category> Delete(int id);
        IResponse<IEnumerable<Category>> GetByName(string name);
    }
}
