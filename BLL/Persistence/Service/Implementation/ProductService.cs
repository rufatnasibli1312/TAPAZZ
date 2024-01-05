using AutoMapper;
using BLL.Persistence.Service.Abstraction;
using BLL.ServiceExtensions;
using DAL.Persistence.Repository.Abstraction;
using DTO.LocationDto_s;
using DTO.ProductDto_s;
using Entity.Entities;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Persistence.Service.Implementation
{
    public class ProductService : IProductService
    {


        public IProductRepository _productRepository { get; }
        public IMapper _mapper { get; }
        public JwtTokenExtractor _jwtTokenExtractor { get; }

        public ProductService(IProductRepository productRepository, IMapper mapper, JwtTokenExtractor jwtTokenExtractor)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _jwtTokenExtractor = jwtTokenExtractor;
        }


        public async Task AddAsync(ProductAddDto productDto, string webRootPath)
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
            await _productRepository.AddAsync(product);

        }

        public async Task<List<ProductToListDto>> GetAllAsync()
        {
            List<Product> products = await _productRepository.GetAllAsync();
            List<ProductToListDto> result = _mapper.Map<List<ProductToListDto>>(products);
            return result;

        }

        public async Task<ProductFindIdDto> GetAsync(int id)
        {

            Product product = await _productRepository.GetAsync(id);
            var result = _mapper.Map<ProductFindIdDto>(product);

            return result;
        }

        public async Task<List<ProductToListDto>> GetMyProductsAsync()
        {
            List<Product> products = await _productRepository.GetAllAsync();
            var userId = _jwtTokenExtractor.GetUserIdFromJwtToken();

            List<ProductToListDto> result = _mapper.Map<List<ProductToListDto>>(
       products.Where(product => product.UserID == userId).ToList());
            return result;
        }

        public async Task<bool> Delete(DeleteProductDto entity)
        {
            var product = await _productRepository.GetAsync(entity.Id);
            if (product != null)
            {
                var userId = _jwtTokenExtractor.GetUserIdFromJwtToken();
                if (product.UserID == userId)
                {
                    await _productRepository.Delete(product);
                    return true;
                }
                return false;

            }
            return false;
        }

        public async Task UpdateAsync(UpdateProductDto updateProductDto, string webRootPath)
        {
            var existingProduct = await _productRepository.GetProductsWithPhotoPath(updateProductDto.Id);
            if (existingProduct != null)
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
                await _productRepository.UpdateAsync(existingProduct);
            }
        }
       
    }
}
