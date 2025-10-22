using Business.Response;
using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IBookService
    {
        IResponse<IEnumerable<Book>> ListAll();
        IResponse<Book> GetById(int id);
        Task<IResponse<Book>> Create(Book book);
        Task<IResponse<Book>> Update(Book book);
        IResponse<Book> Delete(int id);
        IResponse<IEnumerable<Book>> GetByName(string name);
    }
}
