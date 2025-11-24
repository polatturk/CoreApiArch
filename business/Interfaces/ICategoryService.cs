using Business.Response;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.DTOs;

namespace Business.Interfaces
{
    public interface ICategoryService
    {
        IResponse<IEnumerable<CategoryListDto>> ListAll();
        IResponse<CategoryListDto> GetById(int id);
        Task<IResponse<Category>> Create(CategoryDto categoryDto);
        Task<IResponse<CategoryUpdateDto>> Update(CategoryUpdateDto categoryUpdateDto);
        IResponse<Category> Delete(int id);
        IResponse<IEnumerable<CategoryListDto>> GetByName(string name);
    }
}
