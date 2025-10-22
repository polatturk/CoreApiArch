using Business.Response;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IAuthorService
    {
        IResponse<IEnumerable<Author>> ListAll();
        IResponse<Author> GetById(int id);
        Task<IResponse<Author>> Create(Author author);
        Task<IResponse<Author>> Update(Author author);
        IResponse<Author> Delete(int id);
        IResponse<IEnumerable<Author>> GetByName(string name);
    }
}
   