﻿using Business.Interfaces;
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

namespace Business.Services
{
    public class AuthorService : IAuthorService
    {

        // DI ile IGenericRepository alıyoruz.
        private readonly IGenericRepository<Author> _authorRepository;
        private readonly IMapper _mapper;

        public AuthorService(IGenericRepository<Author> authorRepository, IMapper mapper)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
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
                return Task.FromResult<IResponse<Author>>(ResponseGeneric<Author>.Success(newAuthor, "Yazar başarıyla oluşturuldu."));
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
