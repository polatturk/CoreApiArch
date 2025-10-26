using Business.Interfaces;
using Business.Response;
using Core.Entities;
using DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Business.Services
{
    public class AuthorService : IAuthorService
    {

        // DI ile IGenericRepository alıyoruz.
        private readonly IGenericRepository<Author> _authorRepository;

        public AuthorService(IGenericRepository<Author> authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public Task<IResponse<Author>> Create(Author author)
        {
            try
            {
                if (author == null)
                {
                    return Task.FromResult<IResponse<Author>>(ResponseGeneric<Author>.Error("Yazar bilgileri boş olamaz."));
                }
                _authorRepository.Create(author);
                return Task.FromResult<IResponse<Author>>(ResponseGeneric<Author>.Success(author, "Yazar başarıyla oluşturuldu."));
            }
            catch
            {
                return Task.FromResult<IResponse<Author>>(ResponseGeneric<Author>.Error("Beklenmeyen bir hata oluştu."));
            }
        }

        public IResponse<Author> Delete(int id) 
        {
            try
            {
                var author = _authorRepository.GetByIdAsync(id).Result;

                if (author == null)
                {
                    return ResponseGeneric<Author>.Error("Yazar bulunamadı.");

                }
                _authorRepository.Delete(author);
                return ResponseGeneric<Author>.Success(author, "Yazar başarıyla silindi.");
            }
            catch
            {
                return ResponseGeneric<Author>.Error("Beklenmeyen bir hata oluştu.");
            }
        }

        public IResponse<Author> GetById(int id)
        {
            try
            {
                var author = _authorRepository.GetByIdAsync(id).Result;

                if (author == null)
                {
                    return ResponseGeneric<Author>.Error("Yazar bulunamadı.");

                }
                return ResponseGeneric<Author>.Success(author, "Yazar başarıyla bulundu.");
            }
            catch
            {
                return ResponseGeneric<Author>.Error("Beklenmeyen bir hata oluştu.");
            }
        }

        public IResponse<IEnumerable<Author>> GetByName(string name)
        {
            try
            {
                var authorList = _authorRepository.GetAll().Where(x => x.Name == name).ToList();

                if (authorList == null || authorList.Count == 0)
                {
                    return ResponseGeneric<IEnumerable<Author>>.Error("Yazar bulunamadı.");

                }
                return ResponseGeneric<IEnumerable<Author>>.Success(authorList, "Yazar başarıyla bulundu.");
            }
            catch
            {
                return ResponseGeneric<IEnumerable<Author>>.Error("Beklenmeyen bir hata oluştu.");
            }      
        }

        public IResponse<IEnumerable<Author>> ListAll()
        {
            try
            {
                var allAuthors = _authorRepository.GetAll().ToList();

                if (allAuthors == null || allAuthors.Count == 0)
                {
                    return ResponseGeneric<IEnumerable<Author>>.Error("Yazarlar bulunamadı.");
                }
                return ResponseGeneric<IEnumerable<Author>>.Success(allAuthors, "Yazarlar başarıyla listelendi.");
            }
            catch
            {
                return ResponseGeneric<IEnumerable<Author>>.Error("Beklenmeyen bir hata oluştu.");
            }
        }

        public Task<IResponse<Author>> Update(Author author)
        {
            try
            {
                if (author == null)
                {
                    return Task.FromResult<IResponse<Author>>(ResponseGeneric<Author>.Error("Yazar bulunamadı."));

                }
                _authorRepository.Update(author);
                return Task.FromResult<IResponse<Author>>(ResponseGeneric<Author>.Success(author, "Yazar başarıyla güncellendi."));
            }
            catch
            {
                return Task.FromResult<IResponse<Author>>(ResponseGeneric<Author>.Error("Beklenmeyen bir hata oluştu."));
            }
        }
    }
}
