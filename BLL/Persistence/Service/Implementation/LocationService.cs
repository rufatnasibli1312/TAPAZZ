using AutoMapper;
using BLL.Persistence.Service.Abstract;
using BLL.Persistence.Service.Abstraction;
using DAL.Persistence.Repository.Abstract;
using DTO.LocationDto_s;
using Entity.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Persistence.Service.Concrete
{
    public class LocationService : ILocationService

    {
        public ILocationRepository _repository { get; }
        public IMapper _mapper { get; }


        public LocationService(ILocationRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;

        }

        public async Task AddAsync(LocationtoAddDTO locationDto)
        {
            Location location = _mapper.Map<Location>(locationDto);
            await _repository.AddAsync(location);

        }

        public async Task Delete(DeleteLocationDto entity)
        {
            var existingLocation = await _repository.GetAsync(entity.Id);

            await _repository.Delete(existingLocation);
        }

        public async Task<List<LocationToListDto>> GetAllAsync()
        {
            List<Location> list = await _repository.GetAllAsync();
            List<LocationToListDto> result = _mapper.Map<List<LocationToListDto>>(list);
            return result;
        }

        public async Task<LocationFindIdDTO> GetAsync(int id)
        {
            Location location = await _repository.GetAsync(id);

            

            var result = _mapper.Map<LocationFindIdDTO>(location);

            return result;

        }

        public async Task UpdateAsync(LocationToUpdateDTO locationToUpdateDTO)
        {
            var existingLocation = await _repository.GetAsync(locationToUpdateDTO.Id);

            var location = _mapper.Map(locationToUpdateDTO, existingLocation);

            if (location != null)
            {

                await _repository.UpdateAsync(location);
            }

        }

        public async Task<List<LocationAddProductsGetDTO>> GetProductsWithLocationId(int id)
        {
            var productsModel = await _repository.GetProductsWithLocationId(id);
            var product = _mapper.Map <List<LocationAddProductsGetDTO>>(productsModel);
            return product;
        }
    }
}
