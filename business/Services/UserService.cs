using AutoMapper;
using Business.Interfaces;
using Business.Response;
using Core.Entities;
using DataAccess.DTOs;
using DataAccess.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class UserService : IUserService
    {
        // DI ile IGenericRepository alıyoruz.
        private readonly IGenericRepository<User> _userRepository;
        private readonly IMapper _mapper;

        public UserService(IGenericRepository<User> userRepository, Mapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public Task<IResponse<User>> Create(UserDto userDto)
        {
            try
            {
                if (userDto == null)
                {
                    return Task.FromResult<IResponse<User>>(ResponseGeneric<User>.Error("Kullanıcı bilgileri boş olamaz."));
                }

                var newUser = _mapper.Map<User>(userDto);
                newUser.RecordDate = DateTime.Now;

                _userRepository.Create(newUser);
                return Task.FromResult<IResponse<User>>(ResponseGeneric<User>.Success(newUser, "Kullanıcı başarıyla oluşturuldu."));
            }
            catch
            {
                return Task.FromResult<IResponse<User>>(ResponseGeneric<User>.Error("Beklenmeyen bir hata oluştu."));
            }
        }

        public IResponse<User> Delete(int id)
        {
            try
            {
                var user = _userRepository.GetByIdAsync(id).Result;

                if (user == null)
                {
                    return ResponseGeneric<User>.Error("Kullanıcı bulunamadı.");

                }
                _userRepository.Delete(user);
                return ResponseGeneric<User>.Success(user, "Kullanıcı başarıyla silindi.");
            }
            catch
            {
                return ResponseGeneric<User>.Error("Beklenmeyen bir hata oluştu.");
            }
        }

        public IResponse<User> GetById(int id)
        {
            try
            {
                var user = _userRepository.GetByIdAsync(id).Result;

                if (user == null)
                {
                    return ResponseGeneric<User>.Error("Kullanıcı bulunamadı.");

                }
                return ResponseGeneric<User>.Success(user, "Kullanıcı başarıyla bulundu.");
            }
            catch
            {
                return ResponseGeneric<User>.Error("Beklenmeyen bir hata oluştu.");
            }
        }

        public IResponse<IEnumerable<User>> GetByName(string name)
        {
            try
            {
                var userList = _userRepository.GetAll().Where(x => x.Name == name).ToList();

                if (userList == null || userList.Count == 0)
                {
                    return ResponseGeneric<IEnumerable<User>>.Error("Kullanıcı bulunamadı.");
                }
                return ResponseGeneric<IEnumerable<User>>.Success(userList, "Kullanıcı başarıyla bulundu.");
            }
            catch
            {
                return ResponseGeneric<IEnumerable<User>>.Error("Beklenmeyen bir hata oluştu.");
            }
        }

        public IResponse<IEnumerable<User>> ListAll()
        {
            try
            {
                var allUser = _userRepository.GetAll().ToList();

                if (allUser == null || allUser.Count == 0)
                {
                    return ResponseGeneric<IEnumerable<User>>.Error("Kullanıcılar bulunamadı.");
                }
                return ResponseGeneric<IEnumerable<User>>.Success(allUser, "Kullanıcıllar başarıyla listelendi.");
            }
            catch
            {
                return ResponseGeneric<IEnumerable<User>>.Error("Beklenmeyen bir hata oluştu.");
            }
        }

        public Task<IResponse<User>> Update(User user)
        {
            try
            {
                if (user == null)
                {
                    return Task.FromResult<IResponse<User>>(ResponseGeneric<User>.Error("Kullanıcı bulunamadı."));

                }
                _userRepository.Update(user);
                return Task.FromResult<IResponse<User>>(ResponseGeneric<User>.Success(user, "Kullanıcı başarıyla güncellendi."));
            }
            catch 
            {
                return Task.FromResult<IResponse<User>>(ResponseGeneric<User>.Error("Beklenmeyen bir hata oluştu."));
            }
        }
    }
}
