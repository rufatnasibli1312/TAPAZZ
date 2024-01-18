using AutoMapper;
using BLL.Persistence.Service.Abstract;
using BLL.Persistence.Service.Abstraction;
using DAL.Persistence.Repository.Abstract;
using DTO.LocationDto_s;
using Entity.Entities;
using Serilog;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Validation.LocationValidator;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BLL.Persistence.Service.Concrete
{
    public class CityService : ICityService

    {
        public ICityRepository _repository { get; } // okey
        public IMapper _mapper { get; }


        public CityService(ICityRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;

        }

        public async Task AddAsync(CitytoAddDTO locationDto)
        {
            List<string> errors = new List<string>();

            var model = JsonSerializer.Serialize(locationDto);

            CitytoAddValidator validator = new CitytoAddValidator();
            var result = validator.Validate(locationDto);
            errors = result.Errors.Select(m => m.ErrorMessage).ToList();
            if (result.IsValid)
            {
                City city = _mapper.Map<City>(locationDto);
                await _repository.AddAsync(city);
                Log.Information($"{nameof(CityService)}.{nameof(AddAsync)} - City added successfully. Data: {model}");
            }
            else
            {
                throw new ValidationException($"Validation failed-{string.Join(", ", errors)}.");

            }





        }

        public async Task Delete(DeleteCityDto entity)
        {
            List<string> errors = new List<string>();

            var model = JsonSerializer.Serialize(entity);

            DeleteCityValidation validator = new DeleteCityValidation();
            var result = validator.Validate(entity);
            errors = result.Errors.Select(m => m.ErrorMessage).ToList();
            if (result.IsValid)
            {
                var existingLocation = await _repository.GetAsync(entity.Id);
                if (existingLocation == null)
                {
                    errors.Add("City not found ");
                    throw new InvalidOperationException($"{string.Join(", ", errors)}");

                }
                else
                {
                    await _repository.Delete(existingLocation);
                    Log.Information($"{nameof(CityService)}.{nameof(Delete)} - City Deleted successfully. Id: {model}");
                }


            }
            else
            {

                throw new ValidationException($"Validation failed-{string.Join(", ", errors)}.");
            }





        }

        public async Task<List<CityToListDto>> GetAllAsync()
        {

            List<City> list = await _repository.GetAllAsync();
            List<CityToListDto> result = _mapper.Map<List<CityToListDto>>(list);
            if (list.Count != 0)
            {
                Log.Error($"{nameof(CityService)}.{nameof(GetAllAsync)} - Cities not found");
            }

            Log.Information($"{nameof(CityService)}.{nameof(GetAllAsync)} - Cities Get successfully");

            return result;



        }

        public async Task<CityFindIdDTO> GetAsync(int id)
        {


            City location = await _repository.GetAsync(id);
            var result = _mapper.Map<CityFindIdDTO>(location);

            if (result == null)
            {
                Log.Error($"{nameof(CityService)}.{nameof(GetAsync)} - City Not Found.  ID: {id}");
            }
            Log.Information($"{nameof(CityService)}.{nameof(GetAsync)} - City Get successfully.  ID: {id}");

            return result;



        }

        public async Task UpdateAsync(CityToUpdateDTO locationToUpdateDTO)
        {
            List<string> errors = new List<string>();

            if (locationToUpdateDTO == null)
            {
                errors.Add("Dto is null");
                throw new InvalidOperationException($"{string.Join(", ", errors)}");
            }
            var model = JsonSerializer.Serialize(locationToUpdateDTO);

            CityToUpdateValidator validator = new CityToUpdateValidator();
            var result = validator.Validate(locationToUpdateDTO);
            errors = result.Errors.Select(m => m.ErrorMessage).ToList();
            if (result.IsValid)
            {
                var existingLocation = await _repository.GetAsync(locationToUpdateDTO.Id);
                if (existingLocation == null)
                {
                    errors.Add("City not found ");
                   
                    throw new InvalidOperationException($"{string.Join(", ", errors)}");

                }
                else
                {
                    var location = _mapper.Map(locationToUpdateDTO, existingLocation);
                    Log.Information($"{nameof(CityService)}.{nameof(UpdateAsync)} - City Updated successfully. Data: {model}");
                    await _repository.UpdateAsync(location);


                }


            }
            else
            {
                
                throw new ValidationException($"Validation failed-{string.Join(", ", errors)}.");

            }






        }


    }
}
