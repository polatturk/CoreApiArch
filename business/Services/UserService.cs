using AutoMapper;
using Business.Interfaces;
using Business.Response;
using Core.Entities;
using DataAccess.DTOs;
using DataAccess.Repository;
using Microsoft.Extensions.Logging;
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
        private readonly ILogger<UserService> _logger;

        public UserService(IGenericRepository<User> userRepository, IMapper mapper, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
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
                _logger.LogInformation("Kullanıcı başarıyla oluşturuldu.", newUser.Name);
                return Task.FromResult<IResponse<User>>(ResponseGeneric<User>.Success(newUser, "Kullanıcı başarıyla oluşturuldu."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı oluşturulurken bir hata oluştu.", null);
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
                _logger.LogInformation("Kullanıcı başarıyla silindi.", user.Name);
                return ResponseGeneric<User>.Success(user, "Kullanıcı başarıyla silindi.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Kullanıcı silinirken bir hata oluştu.", null);
                return ResponseGeneric<User>.Error("Beklenmeyen bir hata oluştu.");
            }
        }

        public IResponse<UserListDto> GetById(int id)
        {
            try
            {
                var user = _userRepository.GetByIdAsync(id).Result;
                var userListDtos = _mapper.Map<UserListDto>(user);

                if (user == null)
                {
                    return ResponseGeneric<UserListDto>.Error("Kullanıcı bulunamadı.");

                }
                return ResponseGeneric<UserListDto>.Success(userListDtos, "Kullanıcı başarıyla bulundu.");
            }
            catch
            {
                return ResponseGeneric<UserListDto>.Error("Beklenmeyen bir hata oluştu.");
            }
        }

        public IResponse<IEnumerable<UserListDto>> GetByName(string name)
        {
            try
            {
                var userList = _userRepository.GetAll().Where(x => x.Name == name).ToList();
                var userListDtos = _mapper.Map<IEnumerable<UserListDto>>(userList);

                if (userList == null || userList.Count == 0)
                {
                    return ResponseGeneric<IEnumerable<UserListDto>>.Error("Kullanıcı bulunamadı.");
                }
                return ResponseGeneric<IEnumerable<UserListDto>>.Success(userListDtos, "Kullanıcı başarıyla bulundu.");
            }
            catch
            {
                return ResponseGeneric<IEnumerable<UserListDto>>.Error("Beklenmeyen bir hata oluştu.");
            }
        }

        public IResponse<IEnumerable<UserListDto>> ListAll()
        {
            try
            {
                var allUser = _userRepository.GetAll().ToList();
                var userListDtos = _mapper.Map<IEnumerable<UserListDto>>(allUser);

                if (allUser == null || allUser.Count == 0)
                {
                    return ResponseGeneric<IEnumerable<UserListDto>>.Error("Kullanıcılar bulunamadı.");
                }
                return ResponseGeneric<IEnumerable<UserListDto>>.Success(userListDtos, "Kullanıcılar başarıyla listelendi.");
            }
            catch
            {
                return ResponseGeneric<IEnumerable<UserListDto>>.Error("Beklenmeyen bir hata oluştu.");
            }
        }

        public Task<IResponse<UserUpdateDto>> Update(UserUpdateDto userUpdateDto)
        {
            try
            {
                var userEntity = _userRepository.GetByIdAsync(userUpdateDto.Id).Result;
                if (userEntity == null)
                {
                    return Task.FromResult<IResponse<UserUpdateDto>>(ResponseGeneric<UserUpdateDto>.Error("Kullanıcı bulunamadı."));

                }

                if (userUpdateDto.Name != null)
                {
                    userEntity.Name = userUpdateDto.Name;
                }
                if (userUpdateDto.Surname != null)
                {
                    userEntity.Surname = userUpdateDto.Surname;
                }
                if (userUpdateDto.Username != null)
                {
                    userEntity.Username = userUpdateDto.Username;
                }

                if (userUpdateDto.Email != null)
                {
                    userEntity.Email = userUpdateDto.Email;
                }

                if (userUpdateDto.Password != null)
                {
                    userEntity.Password = userUpdateDto.Password;
                }
                
                _userRepository.Update(userEntity);
                _logger.LogInformation("Kullanıcı başarıyla güncellendi.", userUpdateDto.Name);
                return Task.FromResult<IResponse<UserUpdateDto>>(ResponseGeneric<UserUpdateDto>.Success(userUpdateDto, "Kullanıcı başarıyla güncellendi."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Beklenmeyen bir hata oluştu.", null);
                return Task.FromResult<IResponse<UserUpdateDto>>(ResponseGeneric<UserUpdateDto>.Error("Beklenmeyen bir hata oluştu."));
            }
        }
    }
}
