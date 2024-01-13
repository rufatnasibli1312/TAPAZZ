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
        public ICityRepository _repository { get; } // loglama isleri burda hell edilib
        public IMapper _mapper { get; }


        public CityService(ICityRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;

        }

        public async Task AddAsync(LocationtoAddDTO locationDto)
        {
            List<string> errors = new List<string>();
            try
            {
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

            catch (Exception ex)
            {

                if (ex is ValidationException)
                {
                    Log.Error($"{nameof(CityService)}.{nameof(AddAsync)} - Validation failed. Errors: {string.Join(", ", errors)}");
                }
                else
                {
                    Log.Error($"{nameof(CityService)}.{nameof(AddAsync)} - {ex.Message}");

                }
                throw;
            }


        }

        public async Task Delete(DeleteLocationDto entity)
        {
            List<string> errors = new List<string>();
            try
            {
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
            catch (Exception ex)
            {
                if (ex is InvalidOperationException)
                {
                    Log.Error($"{nameof(CityService)}.{nameof(Delete)} - InvalidOperationException:Errors: {string.Join(", ", errors)}");
                }
                if (ex is ValidationException)
                {
                    Log.Error($"{nameof(CategoryService)}.{nameof(Delete)} - Validation failed. Errors: {string.Join(", ", errors)}");
                }
                else
                {
                    Log.Error($"{nameof(CityService)}.{nameof(Delete)} - {ex.Message}");
                }

                throw;
            }

        }

        public async Task<List<LocationToListDto>> GetAllAsync()
        {
            try
            {
                List<City> list = await _repository.GetAllAsync();
                List<LocationToListDto> result = _mapper.Map<List<LocationToListDto>>(list);
                if (list.Count > 0)
                {
                    Log.Information($"{nameof(CityService)}.{nameof(GetAllAsync)} - Cities Get successfully");
                }
                else
                {
                    Log.Error($"{nameof(CityService)}.{nameof(GetAllAsync)} - Cities not found");
                }
                return result;

            }
            catch (Exception ex)
            {
                Log.Error($"{nameof(CityService)}.{nameof(GetAllAsync)} - {ex.Message}");
                throw;
            }

        }

        public async Task<LocationFindIdDTO> GetAsync(int id)
        {
            List<string> errors = new List<string>();
            try
            {
                City location = await _repository.GetAsync(id);
                var result = _mapper.Map<LocationFindIdDTO>(location);
                if (result != null)
                {
                    Log.Information($"{nameof(CityService)}.{nameof(GetAsync)} - City Get successfully.  ID: {id}");
                }
                else
                {
                    errors.Add("City not found ");
                    throw new InvalidOperationException($"{string.Join(", ", errors)}");

                }
                return result;
            }
            catch (Exception ex)
            {
                if (ex is InvalidOperationException)
                {
                    Log.Error($"{nameof(CityService)}.{nameof(GetAsync)} - City not found. ID: {id}");
                }
                else
                {
                    Log.Error($"{nameof(CityService)}.{nameof(GetAsync)} - {ex.Message}");
                }
                throw;
            }


        }

        public async Task UpdateAsync(LocationToUpdateDTO locationToUpdateDTO)
        {
            List<string> errors = new List<string>();
            try
            {
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

                        await _repository.UpdateAsync(location);
                        Log.Information($"{nameof(CityService)}.{nameof(UpdateAsync)} - City Updated successfully. Data: {model}");

                    }


                }
                else
                {
                    throw new ValidationException($"Validation failed-{string.Join(", ", errors)}.");
                }


            }
            catch (Exception ex)
            {
                if (ex is InvalidOperationException)
                {
                    Log.Error($"{nameof(CityService)}.{nameof(UpdateAsync)} - InvalidOperationException:Errors: {string.Join(", ", errors)}");
                }
                if (ex is ValidationException)
                {
                    Log.Error($"{nameof(CityService)}.{nameof(UpdateAsync)} - Validation failed. Errors: {string.Join(", ", errors)}");
                }
                else
                {
                    Log.Error($"{nameof(CityService)}.{nameof(UpdateAsync)} - {ex.Message}");
                }

                throw;
            }


        }

      
    }
}
