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
    public interface IBookService
    {
        IResponse<IEnumerable<BookListDto>> ListAll();
        IResponse<BookListDto> GetById(int id);
        Task<IResponse<Book>> Create(BookDto bookDto);
        Task<IResponse<Book>> Update(Book book);
        IResponse<Book> Delete(int id);
        IResponse<IEnumerable<BookListDto>> GetByName(string name);
        IResponse<IEnumerable<BookListDto>> GetBooksByCategoryId(int categoryId);
        IResponse<IEnumerable<BookListDto>> GetBooksByAuthorId(int authorId);
    }
}
