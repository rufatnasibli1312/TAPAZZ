﻿using AutoMapper;
using Azure;
using BLL.JWT;
using BLL.Persistence.Service.Abstraction;
using BLL.ServiceExtensions;
using DAL.Persistence.Repository.Abstraction;
using DTO.AccountDto_s;
using DTO.JWTDto_s;
using DTO.ProductDto_s;
using Entity.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Persistence.Service.Implementation
{
    public class AccountService : IAccountService
    {
        public IAccountRepository _accountRepository { get; }

        public IMapper _mapper { get; }
        public IConfiguration _configuration { get; }
        public SignInManager<User> _signInManager { get; }
        public JwtTokenExtractor _jwtTokenExtractor { get; }

        public AccountService(IAccountRepository accountRepository, IMapper mapper, IConfiguration configuration, SignInManager<User> signInManager, JwtTokenExtractor jwtTokenExtractor)
        {
            _accountRepository = accountRepository;

            _mapper = mapper;
            _configuration = configuration;
            _signInManager = signInManager;
            _jwtTokenExtractor = jwtTokenExtractor;
        }


        public async Task<IdentityResult> RegisterUserAsync(RegisterModelDto model, string webRootPath)
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
            }
            return data;

        }

        public async Task<User> FindUserById(string userId)
        {
            return await _accountRepository.FindUserById(userId);
        }

        public async Task<ResponseDto> Login(LoginDto loginDto)
        {

            var user = await _accountRepository.FindUserByEmail(loginDto.Email);

            var checkPassword = _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            var tokens = JwtHelper.GenerateToken(_configuration, new List<System.Security.Claims.Claim>
            {
                new System.Security.Claims.Claim("Email", loginDto.Email),
                new System.Security.Claims.Claim("Id", user.Id),
                new System.Security.Claims.Claim("Fullname", user.Fullname)
                


            });


            return tokens;
        }

        public async Task<User> FindUserByEmail(string email)
        {
            return await _accountRepository.FindUserByEmail(email);
        }

        public async Task UpdateAsync(UpdateUserDto updateUserDto, string webRootPath)
        {
            var user = await _accountRepository.FindUserById(updateUserDto.Id);
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
                }
                await _accountRepository.SaveChanges();
            }



        }

    }
}