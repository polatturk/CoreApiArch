using Business.Response;
using Core.Entities;
using Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IAuthorService
    {
        IResponse<IEnumerable<AuthorListDto>> ListAll();
        IResponse<AuthorListDto> GetById(int id);
        Task<IResponse<Author>> Create(AuthorDto author);
        Task<IResponse<Author>> Update(Author author);
        IResponse<Author> Delete(int id);
        IResponse<IEnumerable<AuthorListDto>> GetByName(string name);
    }
}
   