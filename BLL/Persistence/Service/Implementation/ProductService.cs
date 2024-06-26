﻿using AutoMapper;
using BLL.Persistence.Service.Abstraction;
using BLL.ServiceExtensions;
using DAL.Data;
using DAL.Persistence.Repository.Abstraction;
using DTO;
using DTO.LocationDto_s;
using DTO.ProductDto_s;
using Entity.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Validation.ProductValidator;
using static System.Runtime.InteropServices.JavaScript.JSType;
using ValidationException = FluentValidation.ValidationException;

namespace BLL.Persistence.Service.Implementation
{
    public class ProductService : IProductService
    {


        public IProductRepository _productRepository { get; }
        public IMapper _mapper { get; }
        public JwtTokenExtractor _jwtTokenExtractor { get; }
        public IAccountService _accountService { get; }   //okey
        public FindUserRole _findUserRole { get; }
        public MyDbContext _dbContext { get; }
        public ProductAddValidator _addValidator { get; }
        public UpdateProductValidator _updateValidator { get; }

        public ProductService(IProductRepository productRepository, IMapper mapper, JwtTokenExtractor jwtTokenExtractor, IAccountService accountService, FindUserRole findUserRole, MyDbContext dbContext, ProductAddValidator Addvalidator, UpdateProductValidator  updateValidator)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _jwtTokenExtractor = jwtTokenExtractor;
            _accountService = accountService;
            _findUserRole = findUserRole;
            _dbContext = dbContext;
            _addValidator = Addvalidator;
            _updateValidator = updateValidator;
        }


        public async Task AddAsync(ProductAddDto productDto, string webRootPath)
        {
            List<string> errors = new List<string>();
            
                var model = JsonSerializer.Serialize(productDto);

                var result = _addValidator.Validate(productDto);

                errors = result.Errors.Select(m => m.ErrorMessage).ToList();
                if (result.IsValid)
                {
                    Product product = _mapper.Map<Product>(productDto);
                    var userId = _jwtTokenExtractor.GetUserIdFromJwtToken();
                    product.UserID = userId;
                    foreach (var photo in productDto.Photos)
                    {
                        string fileName = photo.FileName;
                        string name = $"{Guid.NewGuid()}-{fileName}";
                        string path = Path.Combine(webRootPath, "ProductPhoto");
                        string filePath = Path.Combine(path, name);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await photo.CopyToAsync(fileStream);
                        }
                        product.PhotoPath.Add(new ImagesPath { Path = name });
                    }
                    Log.Information($"{nameof(ProductService)}.{nameof(AddAsync)} - Product Added Succesfully. Data: {model}");
                    await _productRepository.AddAsync(product);
                }
                else
                {
                    throw new ValidationException($"Validation failed-{string.Join(", ", errors)}.");
                }

            



        }

        public async Task<List<ProductToListDto>> GetAllAsync()
        {
            List<string> errors = new List<string>();
           
                List<Product> products = await _productRepository.GetAllAsync();
                if (products.Count == 0)
                {
                    errors.Add("Products not found");
                    throw new InvalidOperationException($"{string.Join(", ", errors)}");
                }
                else
                {
                    var result = new List<ProductToListDto>();

                    foreach (var product in products)
                    {
                        if (product.ExpireDate > DateTime.Now)
                        {
                            product.IsActive = true;
                        }
                        else
                        {
                            product.IsActive = false;
                        }
                        var productDto = _mapper.Map<ProductToListDto>(product);
                        result.Add(productDto);



                    }

                    await _dbContext.SaveChangesAsync();
                    Log.Information($"{nameof(ProductService)}.{nameof(GetAllAsync)} - Products Get Succesfully.");
                    return result;
                }


        }

        public async Task<ProductFindIdDto> GetAsync(int id)
        {
            List<string> errors = new List<string>();
           
                Product product = await _productRepository.GetAsync(id);
                if (product == null)
                {
                    errors.Add("Product not found");
                    throw new InvalidOperationException($"{string.Join(", ", errors)}");
                }
                var result = _mapper.Map<ProductFindIdDto>(product);
                Log.Information($"{nameof(ProductService)}.{nameof(GetAsync)} - Product Gets Succesfully. Id = {id}");
                return result;
           

        }

        public async Task<List<ProductToListDto>> GetMyProductsAsync()
        {
            List<string> errors = new List<string>();
            
                List<Product> products = await _productRepository.GetAllAsync();
                if (products == null)
                {
                    errors.Add("Product not found");
                    throw new InvalidOperationException($"{string.Join(", ", errors)}");
                }
                else
                {
                    var userId = _jwtTokenExtractor.GetUserIdFromJwtToken();

                    List<ProductToListDto> result = _mapper.Map<List<ProductToListDto>>(
               products.Where(product => product.UserID == userId).ToList());
                    Log.Information($"{nameof(ProductService)}.{nameof(GetMyProductsAsync)} - Your Products Gets Succesfully.");
                    return result;
                }

            
        }

        public async Task Delete(DeleteProductDto entity)
        {
            List<string> errors = new List<string>();
            
                var model = JsonSerializer.Serialize(entity);

                DeleteProductValidator validator = new DeleteProductValidator();
                var result = validator.Validate(entity);
                errors = result.Errors.Select(m => m.ErrorMessage).ToList();
                if (result.IsValid)
                {
                    var product = await _productRepository.GetAsync(entity.Id);
                    if (product != null)
                    {
                        var userId = _jwtTokenExtractor.GetUserIdFromJwtToken();
                        var user = await _accountService.FindUserById(userId);
                        if (user == null)
                        {
                            errors.Add("User not found");
                            throw new InvalidOperationException($"{string.Join(", ", errors)}");
                        }
                        else
                        {
                            var userRole = await _findUserRole.GetUserRole(user);
                            if (userRole == null)
                            {
                                errors.Add("UserRole not found");
                                throw new InvalidOperationException($"{string.Join(", ", errors)}");
                            }
                            else
                            {
                                if (product.UserID == userId || userRole == "Admin")
                                {
                                    Log.Information($"{nameof(ProductService)}.{nameof(Delete)} -  Product Deleted Succesfully.");
                                    await _productRepository.Delete(product);

                                }
                                else
                                {
                                    errors.Add("Invalid user or role for deleting the product");
                                    throw new InvalidOperationException($"{string.Join(", ", errors)}");
                                }
                            }

                        }



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

        public async Task UpdateAsync(UpdateProductDto updateProductDto, string webRootPath)
        {
            List<string> errors = new List<string>();
           
                var model = JsonSerializer.Serialize(updateProductDto);
                
                
                var result = _updateValidator.Validate(updateProductDto);
                var error = result.Errors.Select(m => m.ErrorMessage).ToList();
                if (result.IsValid)
                {
                    var existingProduct = await _productRepository.GetProductsWithPhotoPath(updateProductDto.Id);
                    var userId = _jwtTokenExtractor.GetUserIdFromJwtToken();


                    if (existingProduct != null)
                    {

                        if (userId == existingProduct.UserID)
                        {
                            _mapper.Map(updateProductDto, existingProduct);


                            foreach (var imagePath in existingProduct.PhotoPath)
                            {
                                existingProduct.PhotoPath.Remove(imagePath);

                            }


                            foreach (var newPhoto in updateProductDto.newPhotos)
                            {
                                string fileName = newPhoto.FileName;
                                string name = $"{Guid.NewGuid()}-{fileName}";
                                string path = Path.Combine(webRootPath, "ProductPhoto");
                                string filePath = Path.Combine(path, name);


                                using (var fileStream = new FileStream(filePath, FileMode.Create))
                                {
                                    await newPhoto.CopyToAsync(fileStream);
                                }


                                existingProduct.PhotoPath.Add(new ImagesPath { Path = name });
                            }
                            Log.Information($"{nameof(ProductService)}.{nameof(UpdateAsync)} -  Product Updated Succesfully.");
                            await _productRepository.UpdateAsync(existingProduct);
                        }
                        else
                        {
                            errors.Add("Invalid user  for updated the product");
                            throw new InvalidOperationException($"{string.Join(", ", errors)}");
                        }


                    }
                    else
                    {
                        error.Add("Product is null");
                    }

                }
                else
                {
                    throw new ValidationException($"Validation failed-{string.Join(", ", errors)}.");
                }
            

        }







        public async Task<List<FindProductByFilter>> GetByFilter(FindProductByFilter filter)
        {
            List<string> errors = new List<string>();

           

                // Retrieve the product from the repository based on your filter criteria
                List<Product> products = await _productRepository.GetByFilterAsync(filter);

                // Check if the product is not found
                if (products.Count == 0)
                {
                    errors.Add("Product not found");
                    throw new InvalidOperationException($"{string.Join(", ", errors)}");
                }
                var filteredProducts = new List<FindProductByFilter>();
                foreach (var product in products)
                {
                    if (!string.IsNullOrEmpty(filter.Name) && !product.Name.Contains(filter.Name, StringComparison.OrdinalIgnoreCase))
                    {
                        errors.Add($"Product '{product.Name}' does not match the specified Name");
                    }

                    if (!string.IsNullOrEmpty(filter.Description) && !product.Description.Contains(filter.Description, StringComparison.OrdinalIgnoreCase))
                    {
                        errors.Add($"Product '{product.Name}' does not match the specified Description");
                    }

                    if (filter.CityId != 0 && product.CityId != filter.CityId)
                    {
                        errors.Add($"Product '{product.Name}' does not match the specified CityId");
                    }
                    var mappedProduct = _mapper.Map<FindProductByFilter>(product);
                    filteredProducts.Add(mappedProduct);
                }

                // Check for errors before mapping
                if (errors.Count > 0)
                {
                    throw new InvalidOperationException($"{string.Join(", ", errors)}");
                }

                // Map the result to ProductFindIdDto
                

                Log.Information($"{nameof(ProductService)}.{nameof(GetByFilter)} - Product Gets Successfully.");
                return filteredProducts;
           
        }








    }
}
