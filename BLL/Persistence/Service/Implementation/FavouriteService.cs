using AutoMapper;
using BLL.Persistence.Service.Abstraction;
using BLL.ServiceExtensions;
using DAL.Persistence.Repository.Abstraction;
using DTO;
using DTO.ComplaintDto_s;
using DTO.FavouriteDto_s;
using DTO.ProductDto_s;
using Entity.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Persistence.Service.Implementation
{
    public class FavouriteService : IFavouriteService
    {
        public IFavouriteRepository _favouriteRepository { get; }
        public IMapper _mapper { get; }
        public JwtTokenExtractor _jwtTokenExtractor { get; }

        public FavouriteService(IFavouriteRepository favouriteRepository, IMapper mapper, JwtTokenExtractor jwtTokenExtractor)
        {
            _favouriteRepository = favouriteRepository;
            _mapper = mapper;
            _jwtTokenExtractor = jwtTokenExtractor;
        }


        public async Task AddAsync(FavouriteAddDto favouriteAddDto)
        {
            Favorite favorite = _mapper.Map<Favorite>(favouriteAddDto);
            var userId = _jwtTokenExtractor.GetUserIdFromJwtToken();
            var existingFavorite = await _favouriteRepository.GetByProductIdAndUserIdAsync(favorite.ProductId, userId);
            if (existingFavorite != null)
            {

                throw new InvalidOperationException("Favorite already exists.");  //bunu o true false sohbetlerindede duzeldersen
            }
            favorite.UserId = userId;
            await _favouriteRepository.AddAsync(favorite);



        }

        public async Task<List<FavouriteGetProductDto>> GetAllAsync()
        {
            List<Favorite> favorites = await _favouriteRepository.GetAllAsync();
            var Userid = _jwtTokenExtractor.GetUserIdFromJwtToken();
            List<FavouriteGetProductDto> result = _mapper.Map<List<FavouriteGetProductDto>>(
              favorites.Where(comp => comp.UserId == Userid).ToList());
            return result;

        }

        public async Task<FavouriteGetProductDto> GetAsync(int id)
        {
            Favorite favorite = await _favouriteRepository.GetAsync(id);
            var UserId = _jwtTokenExtractor.GetUserIdFromJwtToken();
            if (favorite.UserId == UserId)
            {
                var result = _mapper.Map<FavouriteGetProductDto>(favorite);
                return result;
            }
            throw new InvalidOperationException("Favorite not found or does not belong to the authenticated user.");
        }

        public async Task Delete(DeleteFavouriteDto entity)
        {
            var favourite = await _favouriteRepository.GetAsync(entity.Id);
            if (favourite == null)
            {
                throw new InvalidOperationException("Favourite not found.");
            }


            var UserId = _jwtTokenExtractor.GetUserIdFromJwtToken();
            if (favourite.UserId != UserId)
            {
                throw new InvalidOperationException("Favourite does not belong to the authenticated user.");
            }
            await _favouriteRepository.Delete(favourite);

        }
    }
}
