﻿using AutoMapper;
using BLL.Persistence.Service.Abstraction;
using BLL.Persistence.Service.Concrete;
using BLL.ServiceExtensions;
using DAL.Persistence.Repository.Abstraction;
using DTO;
using DTO.ComplaintDto_s;
using DTO.FavouriteDto_s;
using DTO.ProductDto_s;
using Entity.Entities;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Validation.FavouriteValidator;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BLL.Persistence.Service.Implementation
{
    public class FavouriteService : IFavouriteService
    {
        public IFavouriteRepository _favouriteRepository { get; }
        public IMapper _mapper { get; }
        public JwtTokenExtractor _jwtTokenExtractor { get; } //loglama edilib
        public IProductService _productService { get; }

        public FavouriteService(IFavouriteRepository favouriteRepository, IMapper mapper, JwtTokenExtractor jwtTokenExtractor, IProductService productService)
        {
            _favouriteRepository = favouriteRepository;
            _mapper = mapper;
            _jwtTokenExtractor = jwtTokenExtractor;
            _productService = productService;
        }


        public async Task AddAsync(FavouriteAddDto favouriteAddDto)
        {
            List<string> errors = new List<string>();
            try
            {
                var model = JsonSerializer.Serialize(favouriteAddDto);

                FavouriteAddValidator validator = new FavouriteAddValidator();
                var result = validator.Validate(favouriteAddDto);
                errors = result.Errors.Select(m => m.ErrorMessage).ToList();
                if (result.IsValid)
                {
                    Favorite favorite = _mapper.Map<Favorite>(favouriteAddDto);
                    var userId = _jwtTokenExtractor.GetUserIdFromJwtToken();

                    var product = _productService.GetAsync(favouriteAddDto.ProductId);

                    if (product != null)
                    {

                        var existingFavorite = await _favouriteRepository.GetByProductIdAndUserIdAsync(favorite.ProductId, userId);
                        if (existingFavorite != null)
                        {
                            errors.Add("Favorite already exists.");
                            throw new InvalidOperationException($"{string.Join(", ", errors)}");

                        }
                        favorite.UserId = userId;
                        Log.Information($"{nameof(FavouriteService)}.{nameof(AddAsync)} - Favourite Added Succesfully. Data: {favouriteAddDto}");
                        await _favouriteRepository.AddAsync(favorite);

                    }
                    else
                    {
                        errors.Add("Product not found");
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
                    Log.Error($"{nameof(FavouriteAddDto)}.{nameof(AddAsync)} - Validation failed. Errors: {string.Join(", ", errors)}");
                }

                else if (ex is InvalidOperationException)
                {
                    Log.Error($"{nameof(FavouriteAddDto)}.{nameof(AddAsync)} - InvalidOperationException:Errors: {string.Join(", ", errors)}");
                }
                else
                {
                    Log.Error($"{nameof(FavouriteAddDto)}.{nameof(AddAsync)} - {ex.Message}");

                }
                throw;
            }




        }

        public async Task<List<FavouriteGetProductDto>> GetAllAsync()
        {
            List<string> errors = new List<string>();
            try
            {
                List<Favorite> favorites = await _favouriteRepository.GetAllAsync();
                if (favorites != null)
                {
                    var Userid = _jwtTokenExtractor.GetUserIdFromJwtToken();
                    List<FavouriteGetProductDto> result = _mapper.Map<List<FavouriteGetProductDto>>(
                      favorites.Where(comp => comp.UserId == Userid).ToList());
                    Log.Information($"{nameof(FavouriteService)}.{nameof(GetAllAsync)} - Favourite Getted All Succesfully.");
                    return result;
                }
                else
                {
                    errors.Add("Favourites not found");
                    throw new InvalidOperationException($"{string.Join(", ", errors)}");
                }

            }
            catch (Exception ex)
            {

                if (ex is InvalidOperationException)
                {
                    Log.Error($"{nameof(FavouriteAddDto)}.{nameof(GetAllAsync)} - InvalidOperationException:Errors: {string.Join(", ", errors)}");
                }
                else
                {
                    Log.Error($"{nameof(FavouriteAddDto)}.{nameof(GetAllAsync)} - {ex.Message}");

                }
                throw;
            }


        }

        public async Task<FavouriteGetProductDto> GetAsync(int id)
        {
            List<string> errors = new List<string>();
            try
            {
                Favorite favorite = await _favouriteRepository.GetAsync(id);
                if (favorite != null)
                {
                    var UserId = _jwtTokenExtractor.GetUserIdFromJwtToken();
                    if (favorite.UserId == UserId)
                    {
                        var result = _mapper.Map<FavouriteGetProductDto>(favorite);
                        Log.Information($"{nameof(FavouriteService)}.{nameof(GetAsync)} - Favourite Getted All Succesfully. Id = {id}");
                        return result;
                    }
                    else
                    {
                        errors.Add("Favorite does not belong to the authenticated user.");
                        throw new InvalidOperationException($"{string.Join(", ", errors)}");

                    }

                }
                else
                {
                    errors.Add("Favorite not found");
                    throw new InvalidOperationException($"{string.Join(", ", errors)}");
                }

            }
            catch (Exception ex)
            {
                if (ex is InvalidOperationException)
                {
                    Log.Error($"{nameof(FavouriteAddDto)}.{nameof(GetAsync)} - InvalidOperationException:Errors: {string.Join(", ", errors)}");
                }
                else
                {
                    Log.Error($"{nameof(FavouriteAddDto)}.{nameof(GetAsync)} - {ex.Message}");

                }
                throw;
            }

        }

        public async Task Delete(DeleteFavouriteDto entity)
        {
            List<string> errors = new List<string>();
            try
            {
                var model = JsonSerializer.Serialize(entity);

                DeleteFavouriteValidator validator = new DeleteFavouriteValidator();
                var result = validator.Validate(entity);
                errors = result.Errors.Select(m => m.ErrorMessage).ToList();
                if (result.IsValid)
                {
                    var favourite = await _favouriteRepository.GetAsync(entity.Id);
                    if (favourite == null)
                    {
                        errors.Add("Favourite not found.");
                        throw new InvalidOperationException($"{string.Join(", ", errors)}");
                    }
                    else
                    {
                        var UserId = _jwtTokenExtractor.GetUserIdFromJwtToken();
                        if (favourite.UserId != UserId)
                        {
                            errors.Add("Favourite does not belong to the authenticated user.");
                            throw new InvalidOperationException($"{string.Join(", ", errors)}");
                        }
                        Log.Information($"{nameof(FavouriteService)}.{nameof(Delete)} - Favourite Deleted  Succesfully. Data = {model}");
                        await _favouriteRepository.Delete(favourite);
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
                    Log.Error($"{nameof(FavouriteAddDto)}.{nameof(Delete)} - Validation failed. Errors: {string.Join(", ", errors)}");
                }

                else if (ex is InvalidOperationException)
                {
                    Log.Error($"{nameof(FavouriteAddDto)}.{nameof(Delete)} - InvalidOperationException:Errors: {string.Join(", ", errors)}");
                }
                else
                {
                    Log.Error($"{nameof(FavouriteAddDto)}.{nameof(Delete)} - {ex.Message}");

                }
                throw;
            }


        }
    }
}
