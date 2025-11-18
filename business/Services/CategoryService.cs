using Business.Interfaces;
using Business.Response;
using Core.Entities;
using DataAccess.Repository;
using DataAccess.DTOs;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Business.Services
{
    public class CategoryService : ICategoryService
    {

        // DI ile IGenericRepository alıyoruz.
        private readonly IGenericRepository<Category> _categoryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(IGenericRepository<Category> categoryRepository, IMapper mapper, ILogger<CategoryService> logger)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public Task<IResponse<Category>> Create(CategoryDto categoryDto)
        {
            try
            {
                if (categoryDto == null)
                {
                    return Task.FromResult<IResponse<Category>>(ResponseGeneric<Category>.Error("Kategori bilgileri boş olamaz."));
                }

                var newCategory = _mapper.Map<Category>(categoryDto);
                newCategory.RecordDate = DateTime.Now;

                _categoryRepository.Create(newCategory);
                _logger.LogInformation("Kategori başarıyla oluşturuldu.", newCategory.Name);

                return Task.FromResult<IResponse<Category>>(ResponseGeneric<Category>.Success(newCategory, "Kategori başarıyla oluşturuldu."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kategori oluşturulurken bir hata oluştu.", categoryDto.Name);
                return Task.FromResult<IResponse<Category>>(ResponseGeneric<Category>.Error("Beklenmeyen bir hata oluştu."));

            }
        }

        public IResponse<Category> Delete(int id)
        {
            try
            {
                var category = _categoryRepository.GetByIdAsync(id).Result;

                if (category == null)
                {
                    return ResponseGeneric<Category>.Error("Kategori bulunamadı.");

                }
                _categoryRepository.Delete(category);
                return ResponseGeneric<Category>.Success(category, "Kategori başarıyla silindi.");
            }
            catch
            {
                return ResponseGeneric<Category>.Error("Beklenmeyen bir hata oluştu.");
            }
        }

        public IResponse<CategoryListDto> GetById(int id)
        {
            try
            {
                var category = _categoryRepository.GetByIdAsync(id).Result;
                var categoryListDtos = _mapper.Map < CategoryListDto>(category);

                if (category == null)
                {
                    return ResponseGeneric<CategoryListDto>.Error("Kategori bulunamadı.");

                }
                return ResponseGeneric<CategoryListDto>.Success(categoryListDtos, "Kategori başarıyla bulundu.");
            }
            catch
            {
                return ResponseGeneric<CategoryListDto>.Error("Beklenmeyen bir hata oluştu.");
            }
        }

        public IResponse<IEnumerable<CategoryListDto>> GetByName(string name)
        {
            try
            {
                var categoryList = _categoryRepository.GetAll().Where(x => x.Name == name).ToList();
                var categoryListDtos = _mapper.Map<IEnumerable<CategoryListDto>>(categoryList);

                if (categoryList == null || categoryList.Count == 0)
                {
                    return ResponseGeneric<IEnumerable<CategoryListDto>>.Error("Kategori bulunamadı.");

                }
                return ResponseGeneric<IEnumerable<CategoryListDto>>.Success(categoryListDtos, "Kategori başarıyla bulundu.");
            }
            catch 
            {
                return ResponseGeneric<IEnumerable<CategoryListDto>>.Error("Beklenmeyen bir hata oluştu.");
            }
        }

        public IResponse<IEnumerable<CategoryListDto>> ListAll()
        {
            try
            {
                var allCategories = _categoryRepository.GetAll().ToList();
                var categoryListDtos = _mapper.Map<IEnumerable<CategoryListDto>>(allCategories);

                if (allCategories == null || allCategories.Count == 0)
                {
                    return ResponseGeneric<IEnumerable<CategoryListDto>>.Error("Kategoriler bulunamadı.");
                }
                return ResponseGeneric<IEnumerable<CategoryListDto>>.Success(categoryListDtos, "Kategoriler başarıyla listelendi.");
            }
            catch
            {
                return ResponseGeneric<IEnumerable<CategoryListDto>>.Error("Beklenmeyen bir hata oluştu.");
            }
        }

        public Task<IResponse<Category>> Update(Category category)
        {
            try
            {
                if (category == null)
                {
                    return Task.FromResult<IResponse<Category>>(ResponseGeneric<Category>.Error("Kategori bulunamadı."));

                }
                _categoryRepository.Update(category);
                return Task.FromResult<IResponse<Category>>(ResponseGeneric<Category>.Success(category, "Kategori başarıyla güncellendi."));
            }
            catch
            {
                return Task.FromResult<IResponse<Category>>(ResponseGeneric<Category>.Error("Kategori bulunamadı."));
            }
        }
    }
}
