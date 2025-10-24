using Business.Interfaces;
using Business.Response;
using Core.Entities;
using DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class CategoryService : ICategoryService
    {

        // DI ile IGenericRepository alıyoruz.
        private readonly IGenericRepository<Category> _categoryRepository;

        public CategoryService(IGenericRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public Task<IResponse<Category>> Create(Category category)
        {
            if (category == null)
            {
                return Task.FromResult<IResponse<Category>>(ResponseGeneric<Category>.Error("Kategori bilgileri boş olamaz."));
            }
            _categoryRepository.Create(category);
            return Task.FromResult<IResponse<Category>>(ResponseGeneric<Category>.Success(category, "Kategori başarıyla oluşturuldu."));
        }

        public IResponse<Category> Delete(int id)
        {
            var category = _categoryRepository.GetByIdAsync(id).Result;

            if (category == null)
            {
                return ResponseGeneric<Category>.Error("Kategori bulunamadı.");

            }
            _categoryRepository.Delete(category);
            return ResponseGeneric<Category>.Success(category, "Kategori başarıyla silindi.");
        }

        public IResponse<Category> GetById(int id)
        {
            var category = _categoryRepository.GetByIdAsync(id).Result;

            if (category == null)
            {
                return ResponseGeneric<Category>.Error("Kategori bulunamadı.");

            }
            return ResponseGeneric<Category>.Success(category, "Kategori başarıyla bulundu.");
        }

        public IResponse<IEnumerable<Category>> GetByName(string name)
        {
            var categoryList = _categoryRepository.GetAll().Where(x => x.Name == name).ToList();

            if (categoryList == null || categoryList.Count == 0)
            {
                return ResponseGeneric<IEnumerable<Category>>.Error("Kategori bulunamadı.");

            }
            return ResponseGeneric<IEnumerable<Category>>.Success(categoryList, "Kategori başarıyla bulundu.");
        }

        public IResponse<IEnumerable<Category>> ListAll()
        {
            var allCategories = _categoryRepository.GetAll().ToList();

            if (allCategories == null || allCategories.Count == 0)
            {
                return ResponseGeneric<IEnumerable<Category>>.Error("Kategoriler bulunamadı.");
            }
            return ResponseGeneric<IEnumerable<Category>>.Success(allCategories, "Kategoriler başarıyla listelendi.");
        }

        public Task<IResponse<Category>> Update(Category category)
        {
            if (category == null)
            {
                return Task.FromResult<IResponse<Category>>(ResponseGeneric<Category>.Error("Kategori bulunamadı"));

            }
            _categoryRepository.Update(category);
            return Task.FromResult<IResponse<Category>>(ResponseGeneric<Category>.Success(category, "Kategori başarıyla güncellendi"));
        }
    }
}
