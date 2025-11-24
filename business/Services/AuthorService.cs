using Business.Interfaces;
using Business.Response;
using Core.DTOs;
using AutoMapper;
using Core.Entities;
using DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using DataAccess.DTOs;

namespace Business.Services
{
    public class AuthorService : IAuthorService
    {

        // DI ile IGenericRepository alıyoruz.
        private readonly IGenericRepository<Author> _authorRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthorService> _logger;


        public AuthorService(IGenericRepository<Author> authorRepository, IMapper mapper, ILogger<AuthorService> logger)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public Task<IResponse<Author>> Create(AuthorDto authorDto)
        {
            try
            {
                if (authorDto == null)
                {
                    return Task.FromResult<IResponse<Author>>(ResponseGeneric<Author>.Error("Yazar bilgileri boş olamaz."));
                }

                var newAuthor = _mapper.Map<Author>(authorDto);
                newAuthor.RecordDate = DateTime.Now;

                _authorRepository.Create(newAuthor);
                _logger.LogInformation("Yazar başarıyla oluşturuldu.", newAuthor.Name);
                return Task.FromResult<IResponse<Author>>(ResponseGeneric<Author>.Success(newAuthor, "Yazar başarıyla oluşturuldu."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Yazar oluşturulurken bir hata oluştu.", null);
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
                _logger.LogInformation("Yazar başarıyla silindi.", author.Name);
                return ResponseGeneric<Author>.Success(author, "Yazar başarıyla silindi.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Yazar silinirken bir hata oluştu.", null);
                return ResponseGeneric<Author>.Error("Beklenmeyen bir hata oluştu.");
            }
        }

        public IResponse<AuthorListDto> GetById(int id)
        {
            try
            {
                var author = _authorRepository.GetByIdAsync(id).Result;
                var authorListDtos = _mapper.Map<AuthorListDto>(author);

                if (authorListDtos == null)
                {
                    return ResponseGeneric<AuthorListDto>.Error("Yazar bulunamadı.");

                }
                return ResponseGeneric<AuthorListDto>.Success(authorListDtos, "Yazar başarıyla bulundu.");
            }
            catch
            {
                return ResponseGeneric<AuthorListDto>.Error("Beklenmeyen bir hata oluştu.");
            }
        }

        public IResponse<IEnumerable<AuthorListDto>> GetByName(string name)
        {
            try
            {
                var authorList = _authorRepository.GetAll().Where(x => x.Name == name).ToList();
                var authorListDtos = _mapper.Map<IEnumerable<AuthorListDto>>(authorList);

                if (authorListDtos == null || authorListDtos.ToList().Count == 0)
                {
                    return ResponseGeneric<IEnumerable<AuthorListDto>>.Error("Yazar bulunamadı.");

                }
                return ResponseGeneric<IEnumerable<AuthorListDto>>.Success(authorListDtos, "Yazar başarıyla bulundu.");
            }
            catch
            {
                return ResponseGeneric<IEnumerable<AuthorListDto>>.Error("Beklenmeyen bir hata oluştu.");
            }      
        }

        public IResponse<IEnumerable<AuthorListDto>> ListAll()
        {
            try
            {
                var allAuthors = _authorRepository.GetAll().ToList();

                var authorListDtos = _mapper.Map<IEnumerable<AuthorListDto>>(allAuthors);

                if (authorListDtos == null || authorListDtos.ToList().Count == 0)
                {
                    return ResponseGeneric<IEnumerable<AuthorListDto>>.Error("Yazarlar bulunamadı.");
                }

                return ResponseGeneric<IEnumerable<AuthorListDto>>.Success(authorListDtos, "Yazarlar başarıyla listelendi.");

            }
            catch
            {
                return ResponseGeneric<IEnumerable<AuthorListDto>>.Error("Beklenmeyen bir hata oluştu.");
            }
        }

        public Task<IResponse<AuthorUpdateDto>> Update(AuthorUpdateDto authorUpdateDto)
        {
            try
            {
                var authorEntity = _authorRepository.GetByIdAsync(authorUpdateDto.Id).Result;
                if (authorEntity == null)
                {
                    return Task.FromResult<IResponse<AuthorUpdateDto>>(ResponseGeneric<AuthorUpdateDto>.Error("Yazar bulunamadı."));

                }

                if (authorUpdateDto.Name != null)
                {
                    authorEntity.Name = authorUpdateDto.Name;
                }

                if (authorUpdateDto.Surname != null)
                {
                    authorEntity.Surname = authorUpdateDto.Surname;
                }

                if (authorUpdateDto.PlaceOfBirth != null)
                {
                    authorEntity.PlaceOfBirth = authorUpdateDto.PlaceOfBirth;
                }

                if (authorUpdateDto.YearOfBirth != null)
                {
                    authorEntity.YearOfBirth = authorUpdateDto.YearOfBirth ?? authorEntity.YearOfBirth;
                }
                _authorRepository.Update(authorEntity);
                _logger.LogInformation("Yazar başarıyla güncellendi.", authorUpdateDto.Name);
                return Task.FromResult<IResponse<AuthorUpdateDto>>(ResponseGeneric<AuthorUpdateDto>.Success(authorUpdateDto, "Yazar başarıyla güncellendi."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Beklenmeyen bir hata oluştu.", null);
                return Task.FromResult<IResponse<AuthorUpdateDto>>(ResponseGeneric<AuthorUpdateDto>.Error("Beklenmeyen bir hata oluştu."));
            }
        }
    }
}
