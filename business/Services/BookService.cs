using Business.Interfaces;
using Core.Entities;
using Business.Response;
using DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccess.DTOs;
using AutoMapper;
using Core.DTOs;
using Microsoft.Extensions.Logging;

namespace Business.Services
{
    public class BookService : IBookService
    {
        // DI ile IGenericRepository alıyoruz.
        private readonly IGenericRepository<Book> _bookRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<BookService> _logger;

        public BookService(IGenericRepository<Book> bookRepository, IMapper mapper, ILogger<BookService> logger)
        {
            _bookRepository = bookRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public Task<IResponse<Book>> Create(BookDto bookDto)
        {
            try
            {
                if (bookDto == null)
                {
                    return Task.FromResult<IResponse<Book>>(ResponseGeneric<Book>.Error("Kitap bilgileri boş olamaz."));
                }

                var newBook = _mapper.Map<Book>(bookDto);
                newBook.RecordDate = DateTime.Now;

                _bookRepository.Create(newBook);
                _logger.LogInformation("Kitap başarıyla oluşturuldu.", newBook.Title);
                return Task.FromResult<IResponse<Book>>(ResponseGeneric<Book>.Success(newBook, "Kitap başarıyla oluşturuldu."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kitap oluşturulurken bir hata oluştu.", null);
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
                _logger.LogInformation("Kitap başarıyla silindi.", book.Title);
                return ResponseGeneric<Book>.Success(book, "Kitap başarıyla silindi.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kitap silinirken bir hata oluştu.", null);
                return ResponseGeneric<Book>.Error("Beklenmeyen bir hata oluştu.");
            }
        }

        public IResponse<BookListDto> GetById(int id)
        {
            try
            {
                var book = _bookRepository.GetByIdAsync(id).Result;
                var bookListDtos = _mapper.Map<BookListDto>(book);

                if (bookListDtos == null)
                {
                    return ResponseGeneric<BookListDto>.Error("Kitap bulunamadı.");

                }
                return ResponseGeneric<BookListDto>.Success(bookListDtos, "Kitap başarıyla bulundu.");
            }
            catch
            {
                return ResponseGeneric<BookListDto>.Error("Beklenmeyen bir hata oluştu.");
            }
        }

        public IResponse<IEnumerable<BookListDto>> GetByName(string title)
        {
            try
            {
                var bookList = _bookRepository.GetAll().Where(x => x.Title == title).ToList();
                var bookListDtos = _mapper.Map<IEnumerable<BookListDto>>(bookList);

                if (bookListDtos == null || bookListDtos.ToList().Count == 0)
                {
                    return ResponseGeneric<IEnumerable<BookListDto>>.Error("Kitap bulunamadı.");

                }
                return ResponseGeneric<IEnumerable<BookListDto>>.Success(bookListDtos, "Kitap başarıyla bulundu.");
            }
            catch
            {
                return ResponseGeneric<IEnumerable<BookListDto>>.Error("Beklenmeyen bir hata oluştu.");
            }
        }

        public IResponse<IEnumerable<BookListDto>> ListAll()
        {
            try
            {
                var allBooks = _bookRepository.GetAll().ToList();
                var bookListDtos = _mapper.Map<IEnumerable<BookListDto>>(allBooks);

                if (bookListDtos == null || bookListDtos.ToList().Count == 0)
                {
                    return ResponseGeneric<IEnumerable<BookListDto>>.Error("Kitaplar bulunamadı.");
                }
                return ResponseGeneric<IEnumerable<BookListDto>>.Success(bookListDtos, "Kitaplar başarıyla listelendi.");
            }
            catch
            {
                return ResponseGeneric<IEnumerable<BookListDto>>.Error("Beklenmeyen bir hata oluştu.");
            }
        }

        public IResponse<IEnumerable<BookListDto>> GetBooksByCategoryId(int categoryId)
        {
            try
            {
                var booksInCategory = _bookRepository.GetAll().Where(x => x.CategoryId == categoryId).ToList();

                var bookDtos = _mapper.Map<IEnumerable<BookListDto>>(booksInCategory);

                if (bookDtos == null || bookDtos.ToList().Count == 0)
                {
                    return ResponseGeneric<IEnumerable<BookListDto>>.Error("Kategori bulunamadı.");
                }
                return ResponseGeneric<IEnumerable<BookListDto>>.Success(bookDtos, "Kategoriler başarıyla listelendi.");
            }
            catch
            {

                return ResponseGeneric<IEnumerable<BookListDto>>.Error("Bir hata oluştu.");
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
                _logger.LogInformation("Kitap başarıyla güncellendi.", book.Title);
                return Task.FromResult<IResponse<Book>>(ResponseGeneric<Book>.Success(book, "Kitap başarıyla güncellendi."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Beklenmeyen bir hata oluştu.", null);
                return Task.FromResult<IResponse<Book>>(ResponseGeneric<Book>.Error("Beklenmeyen bir hata oluştu."));
            }
        }
    }
}
