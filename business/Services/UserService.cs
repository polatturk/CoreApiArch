using AutoMapper;
using Business.Interfaces;
using Business.Response;
using Core.Entities;
using DataAccess.DTOs;
using DataAccess.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class UserService : IUserService
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;


        public UserService(IGenericRepository<User> userRepository, IConfiguration configuration, IMapper mapper)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _mapper = mapper;   
        }

        public IResponse<UserDto> Create(UserDto user)
        {
            if (user == null) return ResponseGeneric<UserDto>.Error("Kullanıcı bilgileri boş olamaz.");

            if (string.IsNullOrEmpty(user.Username) && string.IsNullOrEmpty(user.Email))
                return ResponseGeneric<UserDto>.Error("Kullanıcı adı veya e-posta adresi boş olamaz.");

            var existingUser = _userRepository.GetAll().FirstOrDefault(x => x.Username == user.Username || x.Email == user.Email);
            if (existingUser != null)
                return ResponseGeneric<UserDto>.Error("Bu kullanıcı adı veya e-posta adresi zaten kullanılıyor.");

            var newUser = _mapper.Map<User>(user);

            newUser.Password = HashPasword(user.Password);
            newUser.RecordDate = DateTime.Now;

            _userRepository.Create(newUser);

            return ResponseGeneric<UserDto>.Success(null, "Kullanıcı kaydı oluşturuldu.");
        }

        public IResponse<UserResponseDto> Login(UserLoginDto user)
        {
            if (string.IsNullOrWhiteSpace(user.Username) && string.IsNullOrWhiteSpace(user.Email))
            {
                return ResponseGeneric<UserResponseDto>.Error("Kullanıcı adı veya e-posta adresi boş olamaz.");
            }

            var hashedPassword = HashPasword(user.Password);
            var checkUser = _userRepository.GetAll().FirstOrDefault(x =>
                (x.Username == user.Username || x.Email == user.Email) && x.Password == hashedPassword);

            if (checkUser == null)
            {
                return ResponseGeneric<UserResponseDto>.Error("Kullanıcı adı veya şifre hatalı.");
            }

            var loginResponse = _mapper.Map<UserResponseDto>(checkUser);

            loginResponse.Token = GenerateJwtToken(checkUser);

            return ResponseGeneric<UserResponseDto>.Success(loginResponse, "Giriş başarılı.");
        }

        private string HashPasword(string password)
        {
            //TODO: SecretKey'i config dosyasından al
            string secretKey = "";


            using (var sha256 = SHA256.Create())
            {
                var combinedPassword = password + secretKey;
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combinedPassword));
                var hashedPassword = Convert.ToBase64String(bytes);
                return hashedPassword;
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

        private string GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Sid, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                               issuer: _configuration["Jwt:Issuer"],
                                              audience: _configuration["Jwt:Audience"],
                                                             claims: claims,
                                                                            expires: DateTime.Now.AddMinutes(30),
                                                                                           signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);

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
                return Task.FromResult<IResponse<UserUpdateDto>>(ResponseGeneric<UserUpdateDto>.Success(userUpdateDto, "Kullanıcı başarıyla güncellendi."));
            }
            catch (Exception ex)
            {
                return Task.FromResult<IResponse<UserUpdateDto>>(ResponseGeneric<UserUpdateDto>.Error("Beklenmeyen bir hata oluştu."));
            }
        }
    }
}
