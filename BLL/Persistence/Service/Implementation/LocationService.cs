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
    public class LocationService : ILocationService

    {
        public ILocationRepository _repository { get; } // loglama isleri burda hell edilib
        public IMapper _mapper { get; }


        public LocationService(ILocationRepository repository, IMapper mapper)
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

                LocationtoAddValidator validator = new LocationtoAddValidator();
                var result = validator.Validate(locationDto);
                errors = result.Errors.Select(m => m.ErrorMessage).ToList();
                if (result.IsValid)
                {
                    Location location = _mapper.Map<Location>(locationDto);
                    await _repository.AddAsync(location);
                    Log.Information($"{nameof(LocationService)}.{nameof(AddAsync)} - Location added successfully. Data: {model}");
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
                    Log.Error($"{nameof(LocationService)}.{nameof(AddAsync)} - Validation failed. Errors: {string.Join(", ", errors)}");
                }
                else
                {
                    Log.Error($"{nameof(LocationService)}.{nameof(AddAsync)} - {ex.Message}");

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

                DeleteLocationValidation validator = new DeleteLocationValidation();
                var result = validator.Validate(entity);
                errors = result.Errors.Select(m => m.ErrorMessage).ToList();
                if (result.IsValid)
                {
                    var existingLocation = await _repository.GetAsync(entity.Id);
                    if (existingLocation == null)
                    {
                        errors.Add("Location not found ");
                        throw new InvalidOperationException($"{string.Join(", ", errors)}");
                    }
                    else
                    {
                        await _repository.Delete(existingLocation);
                        Log.Information($"{nameof(LocationService)}.{nameof(Delete)} - Location Deleted successfully. Id: {model}");
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
                    Log.Error($"{nameof(LocationService)}.{nameof(Delete)} - InvalidOperationException:Errors: {string.Join(", ", errors)}");
                }
                if (ex is ValidationException)
                {
                    Log.Error($"{nameof(CategoryService)}.{nameof(Delete)} - Validation failed. Errors: {string.Join(", ", errors)}");
                }
                else
                {
                    Log.Error($"{nameof(LocationService)}.{nameof(Delete)} - {ex.Message}");
                }

                throw;
            }

        }

        public async Task<List<LocationToListDto>> GetAllAsync()
        {
            try
            {
                List<Location> list = await _repository.GetAllAsync();
                List<LocationToListDto> result = _mapper.Map<List<LocationToListDto>>(list);
                if (list.Count > 0)
                {
                    Log.Information($"{nameof(LocationService)}.{nameof(GetAllAsync)} - Locations Get successfully");
                }
                else
                {
                    Log.Error($"{nameof(LocationService)}.{nameof(GetAllAsync)} - Locations not found");
                }
                return result;

            }
            catch (Exception ex)
            {
                Log.Error($"{nameof(LocationService)}.{nameof(GetAllAsync)} - {ex.Message}");
                throw;
            }

        }

        public async Task<LocationFindIdDTO> GetAsync(int id)
        {
            List<string> errors = new List<string>();
            try
            {
                Location location = await _repository.GetAsync(id);
                var result = _mapper.Map<LocationFindIdDTO>(location);
                if (result != null)
                {
                    Log.Information($"{nameof(LocationService)}.{nameof(GetAsync)} - Location Get successfully.  ID: {id}");
                }
                else
                {
                    errors.Add("Location not found ");
                    throw new InvalidOperationException($"{string.Join(", ", errors)}");

                }
                return result;
            }
            catch (Exception ex)
            {
                if (ex is InvalidOperationException)
                {
                    Log.Error($"{nameof(LocationService)}.{nameof(GetAsync)} - Location not found. ID: {id}");
                }
                else
                {
                    Log.Error($"{nameof(LocationService)}.{nameof(GetAsync)} - {ex.Message}");
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

                LocationToUpdateValidator validator = new LocationToUpdateValidator();
                var result = validator.Validate(locationToUpdateDTO);
                errors = result.Errors.Select(m => m.ErrorMessage).ToList();
                if (result.IsValid)
                {
                    var existingLocation = await _repository.GetAsync(locationToUpdateDTO.Id);
                    if (existingLocation == null)
                    {
                        errors.Add("Location not found ");
                        throw new InvalidOperationException($"{string.Join(", ", errors)}");
                    }
                    else
                    {
                        var location = _mapper.Map(locationToUpdateDTO, existingLocation);

                        await _repository.UpdateAsync(location);
                        Log.Information($"{nameof(LocationService)}.{nameof(UpdateAsync)} - Location Updated successfully. Data: {model}");

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
                    Log.Error($"{nameof(LocationService)}.{nameof(UpdateAsync)} - InvalidOperationException:Errors: {string.Join(", ", errors)}");
                }
                if (ex is ValidationException)
                {
                    Log.Error($"{nameof(LocationService)}.{nameof(UpdateAsync)} - Validation failed. Errors: {string.Join(", ", errors)}");
                }
                else
                {
                    Log.Error($"{nameof(LocationService)}.{nameof(UpdateAsync)} - {ex.Message}");
                }

                throw;
            }


        }

        public async Task<List<LocationAddProductsGetDTO>> GetProductsWithLocationId(int id)
        {
            List<string> errors = new List<string>();
            try
            {
                var existingLocation = await _repository.GetAsync(id);
                if (existingLocation != null)
                {
                    var productsModel = await _repository.GetProductsWithLocationId(id);
                    var product = _mapper.Map<List<LocationAddProductsGetDTO>>(productsModel);
                    if (product.Any(location => location.Products.Count > 0))
                    {
                        Log.Information($"{nameof(LocationService)}.{nameof(GetProductsWithLocationId)} - Products Get successfully with this {id}");
                    }
                    else
                    {
                        Log.Error($"{nameof(LocationService)}.{nameof(GetProductsWithLocationId)} - Products not found");
                    }

                    return product;
                }
                else
                {
                    errors.Add("Location not found ");
                    throw new InvalidOperationException($"{string.Join(", ", errors)}");
                }

            }
            catch (Exception ex)
            {

                if (ex is ValidationException)
                {
                    Log.Error($"{nameof(LocationService)}.{nameof(GetProductsWithLocationId)} - Validation failed. Errors: {string.Join(", ", errors)}");
                }
                else
                {
                    Log.Error($"{nameof(LocationService)}.{nameof(GetProductsWithLocationId)} - {ex.Message}");

                }
                throw;
            }

        }
    }
}
