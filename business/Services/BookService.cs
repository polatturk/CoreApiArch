using Business.Interfaces;
using Core.Entities;
using Business.Response;
using DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class BookService : IBookService
    {
        // DI ile IGenericRepository alıyoruz.
        private readonly IGenericRepository<Book> _bookRepository;

        public BookService(IGenericRepository<Book> bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public Task<IResponse<Book>> Create(Book book)
        {
            try
            {
                if (book == null)
                {
                    return Task.FromResult<IResponse<Book>>(ResponseGeneric<Book>.Error("Kitap bilgileri boş olamaz."));
                }
                _bookRepository.Create(book);
                return Task.FromResult<IResponse<Book>>(ResponseGeneric<Book>.Success(book, "Kitap başarıyla oluşturuldu."));
            }
            catch
            {
                return Task.FromResult<IResponse<Book>>(ResponseGeneric<Book>.Error("Beklenmeyen bir hata oluştu."));
            }
        }

        public IResponse<Book> Delete(int id)
        {
            try
            {
                var book = _bookRepository.GetByIdAsync(id).Result;

                if (book == null)
                {
                    return ResponseGeneric<Book>.Error("Kitap bulunamadı.");

                }
                _bookRepository.Delete(book);
                return ResponseGeneric<Book>.Success(book, "Kitap başarıyla silindi.");
            }
            catch 
            {
                return ResponseGeneric<Book>.Error("Beklenmeyen bir hata oluştu.");
            }
        }

        public IResponse<Book> GetById(int id)
        {
            try
            {
                var book = _bookRepository.GetByIdAsync(id).Result;

                if (book == null)
                {
                    return ResponseGeneric<Book>.Error("Kitap bulunamadı.");

                }
                return ResponseGeneric<Book>.Success(book, "Kitap başarıyla bulundu.");
            }
            catch
            {
                return ResponseGeneric<Book>.Error("Beklenmeyen bir hata oluştu.");
            }
        }

        public IResponse<IEnumerable<Book>> GetByName(string title)
        {
            try
            {
                var bookList = _bookRepository.GetAll().Where(x => x.Title == title).ToList();

                if (bookList == null || bookList.Count == 0)
                {
                    return ResponseGeneric<IEnumerable<Book>>.Error("Kitap bulunamadı.");

                }
                return ResponseGeneric<IEnumerable<Book>>.Success(bookList, "Kitap başarıyla bulundu.");
            }
            catch
            {
                return ResponseGeneric<IEnumerable<Book>>.Error("Beklenmeyen bir hata oluştu.");
            }
        }

        public IResponse<IEnumerable<Book>> ListAll()
        {
            try
            {
                var allBooks = _bookRepository.GetAll().ToList();

                if (allBooks == null || allBooks.Count == 0)
                {
                    return ResponseGeneric<IEnumerable<Book>>.Error("Kitaplar bulunamadı.");
                }
                return ResponseGeneric<IEnumerable<Book>>.Success(allBooks, "Kitaplar başarıyla listelendi.");
            }
            catch
            {
                return ResponseGeneric<IEnumerable<Book>>.Error("Beklenmeyen bir hata oluştu.");
            }
        }

        public Task<IResponse<Book>> Update(Book book)
        {
            try
            {
                if (book == null)
                {
                    return Task.FromResult<IResponse<Book>>(ResponseGeneric<Book>.Error("Kitap bulunamadı."));

                }
                _bookRepository.Update(book);
                return Task.FromResult<IResponse<Book>>(ResponseGeneric<Book>.Success(book, "Kitap başarıyla güncellendi."));
            }
            catch
            {
                return Task.FromResult<IResponse<Book>>(ResponseGeneric<Book>.Error("Beklenmeyen bir hata oluştu."));
            }
        }
    }
}
