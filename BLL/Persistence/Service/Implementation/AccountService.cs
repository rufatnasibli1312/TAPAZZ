using AutoMapper;
using Azure;
using BLL.JWT;
using BLL.Persistence.Service.Abstraction;
using BLL.Persistence.Service.Concrete;
using BLL.ServiceExtensions;
using DAL.Persistence.Repository.Abstraction;
using DTO.AccountDto_s;
using DTO.JWTDto_s;
using DTO.ProductDto_s;
using Entity.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Validation.UserValidator;
using static System.Runtime.InteropServices.JavaScript.JSType;
using ValidationException = FluentValidation.ValidationException;

namespace BLL.Persistence.Service.Implementation
{
    public class AccountService : IAccountService
    {
        public IAccountRepository _accountRepository { get; }

        public IMapper _mapper { get; }
        public IConfiguration _configuration { get; }
        public SignInManager<User> _signInManager { get; }
        public JwtTokenExtractor _jwtTokenExtractor { get; }

        public FindUserRole _findUserRole { get; }

        public AccountService(IAccountRepository accountRepository, IMapper mapper, IConfiguration configuration, SignInManager<User> signInManager, JwtTokenExtractor jwtTokenExtractor, FindUserRole findUserRole)
        {
            _accountRepository = accountRepository;

            _mapper = mapper;
            _configuration = configuration;
            _signInManager = signInManager;
            _jwtTokenExtractor = jwtTokenExtractor;

            _findUserRole = findUserRole;
        }


        public async Task<IdentityResult> RegisterUserAsync(RegisterModelDto model, string webRootPath)
        {
            List<string> errors = new List<string>();
            try
            {
                var jsonModel = JsonSerializer.Serialize(model);


                RegisterValidation validator = new RegisterValidation();
                var result = await validator.ValidateAsync(model);
                errors = result.Errors.Select(error => error.ErrorMessage).ToList();
                if (result.IsValid)
                {
                    var user = _mapper.Map<User>(model);
                    string fileName = model.Photo.FileName;
                    string name = $"{Guid.NewGuid()}-{fileName}";
                    string path = Path.Combine(webRootPath, "ProfilePhoto");
                    string filePath = Path.Combine(path, name);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.Photo.CopyToAsync(fileStream);
                    }

                    user.PhotoPath = name;
                    user.PhotoName = fileName;



                    var data = await _accountRepository.RegisterUserAsync(user, model.Password);
                    if (data.Succeeded)
                    {
                        var addToRoleResult = await _accountRepository.AddRoleAsync(user);
                        if (addToRoleResult.Succeeded)
                        {
                            Log.Information($"{nameof(AccountService)}.{nameof(RegisterUserAsync)} - User Registered Succesfully. Data --- {model}");
                            return data;
                        }
                        else
                        {
                            errors.Add("Failed to add role for the user.");


                            throw new InvalidOperationException($"{string.Join(", ", errors)}");
                        }

                    }
                    else
                    {
                        errors.Add("User registration failed");
                        throw new InvalidOperationException($"{string.Join(", ", errors)}");
                    }

                }
                else
                {
                    throw new ValidationException($"Validation failed-{string.Join(", ", errors)}.");
                }

            }
            catch (Exception ex)
            {
                if (ex is ValidationException)
                {
                    Log.Error($"{nameof(AccountService)}.{nameof(RegisterUserAsync)} - Validation failed. Errors: {string.Join(", ", errors)}");
                }
                else if (ex is InvalidOperationException)
                {
                    Log.Error($"{nameof(AccountService)}.{nameof(RegisterUserAsync)} - InvalidOperationException:Errors: {string.Join(", ", errors)}");
                }
                else
                {
                    Log.Error($"{nameof(AccountService)}.{nameof(RegisterUserAsync)} - {ex.Message}");
                }

                throw;
            }


        }

        public async Task<User> FindUserById(string userId)
        {
            List<string> errors = new List<string>();
            try
            {
                var user = await _accountRepository.FindUserById(userId);
                if (user == null)
                {
                    errors.Add("User not  Found");
                    throw new InvalidOperationException($"{string.Join(", ", errors)}");

                }
                Log.Information($"{nameof(AccountService)}.{nameof(FindUserById)} - User Finded Succesfully. Id --- {userId}");
                return user;
            }
            catch (Exception ex)
            {
                if (ex is InvalidOperationException)
                {
                    Log.Error($"{nameof(AccountService)}.{nameof(FindUserById)} - InvalidOperationException:Errors: {string.Join(", ", errors)}");
                }
                else
                {
                    Log.Error($"{nameof(AccountService)}.{nameof(FindUserById)} - {ex.Message}");
                }

                throw;
            }

        }

        public async Task<ResponseDto> Login(LoginDto loginDto)
        {
            List<string> errors = new List<string>();
            try
            {
                var jsonModel = JsonSerializer.Serialize(loginDto);

                LoginValidator validator = new LoginValidator();
                var result = validator.Validate(loginDto);
                errors = result.Errors.Select(m => m.ErrorMessage).ToList();
                if (result.IsValid)
                {
                    var user = await _accountRepository.FindUserByEmail(loginDto.Email);
                    if (user == null)
                    {
                        errors.Add("User not found");
                        throw new InvalidOperationException($"{string.Join(", ", errors)}");
                    }
                    else
                    {
                        var checkPassword = _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

                        if (checkPassword.Result.Succeeded)
                        {
                            var role = await _findUserRole.GetUserRole(user);
                            if (role == null)
                            {
                                errors.Add("Role not found");
                                throw new InvalidOperationException($"{string.Join(", ", errors)}");
                            }
                            else
                            {
                                var tokens = JwtHelper.GenerateToken(_configuration, new List<System.Security.Claims.Claim>
                                {
                                     new System.Security.Claims.Claim("Email", loginDto.Email),
                                           new System.Security.Claims.Claim("Id", user.Id),
                                         new System.Security.Claims.Claim("Fullname", user.Fullname),
                                         new System.Security.Claims.Claim(ClaimTypes.Role,role)

                                  });
                                Log.Information($"{nameof(ComplaintService)}.{nameof(Login)} - User  Logged in Succesfully.");
                                return tokens;



                            }


                        }
                        else
                        {
                            errors.Add("Password  is not true");
                            throw new InvalidOperationException($"{string.Join(", ", errors)}");
                        }

                    }


                }
                else
                {
                    throw new ValidationException($"Validation failed-{string.Join(", ", errors)}.");
                }

            }
            catch (Exception ex)
            {
               
                    Log.Error($"{nameof(AccountService)}.{nameof(Login)} - {ex.Message}");
               

                throw;
            }

        }

        public async Task<User> FindUserByEmail(string email)
        {
            List<string> errors = new List<string>();
            try
            {
                var user = await _accountRepository.FindUserByEmail(email);
                if (user == null)
                {
                    errors.Add("User not  Found");
                    throw new InvalidOperationException($"{string.Join(", ", errors)}");

                }
                Log.Information($"{nameof(AccountService)}.{nameof(FindUserByEmail)} - User Finded Succesfully. Email --- {email}");
                return user;
            }
            catch (Exception ex)
            {
                if (ex is InvalidOperationException)
                {
                    Log.Error($"{nameof(AccountService)}.{nameof(FindUserByEmail)} - InvalidOperationException:Errors: {string.Join(", ", errors)}");
                }
                else
                {
                    Log.Error($"{nameof(AccountService)}.{nameof(FindUserByEmail)} - {ex.Message}");
                }

                throw;
            }

        }

        public async Task UpdateAsync(UpdateUserDto updateUserDto, string webRootPath)
        {
            List<string> errors = new List<string>();
            try
            {
                var jsonModel = JsonSerializer.Serialize(updateUserDto);

                UpdateUserValidator validator = new UpdateUserValidator();
                var result = validator.Validate(updateUserDto);
                errors = result.Errors.Select(m => m.ErrorMessage).ToList();

                if (result.IsValid)
                {
                    var user = await _accountRepository.FindUserById(updateUserDto.Id);
                    if (user == null)
                    {
                        errors.Add("User not  Found");
                        throw new InvalidOperationException($"{string.Join(", ", errors)}");
                    }
                    else
                    {
                        var UserId = _jwtTokenExtractor.GetUserIdFromJwtToken();
                        if (updateUserDto.Id == UserId)
                        {
                            _mapper.Map(updateUserDto, user);
                            if (updateUserDto.newProfilePhoto != null)
                            {
                                string fileName = updateUserDto.newProfilePhoto.FileName;
                                string name = $"{Guid.NewGuid()}-{fileName}";
                                string path = Path.Combine(webRootPath, "ProfilePhoto");
                                string filePath = Path.Combine(path, name);

                                using (var fileStream = new FileStream(filePath, FileMode.Create))
                                {
                                    updateUserDto.newProfilePhoto.CopyTo(fileStream);
                                }

                                user.PhotoPath = name;
                                user.PhotoName = fileName;

                                Log.Information($"{nameof(AccountService)}.{nameof(UpdateAsync)} - User Updated Succesfully.");
                                await _accountRepository.SaveChanges();
                            }
                            else
                            {
                                errors.Add("Unauthorized operation: User is not allowed to update this profile.");
                                throw new InvalidOperationException($"{string.Join(", ", errors)}");
                            }
                        }

                    }
                }
                else
                {
                    throw new ValidationException($"Validation failed-{string.Join(", ", errors)}.");
                }




            }
            catch (Exception ex)
            {
                if (ex is ValidationException)
                {
                    Log.Error($"{nameof(AccountService)}.{nameof(UpdateAsync)} - Validation failed. Errors: {string.Join(", ", errors)}");
                }
                else if (ex is InvalidOperationException)
                {
                    Log.Error($"{nameof(AccountService)}.{nameof(UpdateAsync)} - InvalidOperationException:Errors: {string.Join(", ", errors)}");
                }
                else
                {
                    Log.Error($"{nameof(AccountService)}.{nameof(UpdateAsync)} - {ex.Message}");
                }

                throw;
            }







        }

        public async Task DeleteAsync(DeleteUserDto deleteUserDto)
        {
            List<string> errors = new List<string>();
            try
            {
                var jsonModel = JsonSerializer.Serialize(deleteUserDto);

                DeleteUserValidator validator = new DeleteUserValidator();
                var result = validator.Validate(deleteUserDto);
                errors = result.Errors.Select(m => m.ErrorMessage).ToList();
                if (result.IsValid)
                {
                    var UserId = _jwtTokenExtractor.GetUserIdFromJwtToken();

                    var LoginUser = await _accountRepository.FindUserById(UserId);
                    if (LoginUser == null)
                    {
                        errors.Add(" Logining User not  Found");
                        throw new InvalidOperationException($"{string.Join(", ", errors)}");
                    }
                    else
                    {
                        var userRole = await _findUserRole.GetUserRole(LoginUser);

                        if (userRole == "User")
                        {
                            if (UserId == deleteUserDto.Id)
                            {

                                await _accountRepository.RemoveUser(LoginUser);
                                Log.Information($"{nameof(AccountService)}.{nameof(DeleteAsync)} - User Deleted Succesfully.");
                                await _accountRepository.SaveChanges();

                            }
                        }
                        else if (userRole == "Admin")
                        {
                            if (UserId != deleteUserDto.Id)
                            {
                                var user = await _accountRepository.FindUserById(deleteUserDto.Id);
                                if (user != null)
                                {
                                    await _accountRepository.RemoveUser(user);
                                    Log.Information($"{nameof(AccountService)}.{nameof(DeleteAsync)} - User Deleted By Admin Succesfully.");
                                    await _accountRepository.SaveChanges();

                                }
                                else
                                {
                                    errors.Add("User not  Found");
                                    throw new InvalidOperationException($"{string.Join(", ", errors)}");
                                }


                            }
                        }
                        else
                        {
                            errors.Add("Role not  Found");
                            throw new InvalidOperationException($"{string.Join(", ", errors)}");
                        }

                    }
                }

                else
                {
                    throw new ValidationException($"Validation failed-{string.Join(", ", errors)}.");
                }

            }
            catch (Exception ex)
            {
                if (ex is ValidationException)
                {
                    Log.Error($"{nameof(AccountService)}.{nameof(UpdateAsync)} - Validation failed. Errors: {string.Join(", ", errors)}");
                }
                else if (ex is InvalidOperationException)
                {
                    Log.Error($"{nameof(AccountService)}.{nameof(UpdateAsync)} - InvalidOperationException:Errors: {string.Join(", ", errors)}");
                }
                else
                {
                    Log.Error($"{nameof(AccountService)}.{nameof(UpdateAsync)} - {ex.Message}");
                }

                throw;
            }





        }
    }
}
