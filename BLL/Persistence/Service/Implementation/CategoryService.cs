using AutoMapper;
using BLL.CustomException;
using BLL.Persistence.Service.Abstract;
using DAL.Persistence.Repository.Abstract;
using DAL.Persistence.Repository.Implementation;
using DTO.CategoryDto_s;
using DTO.LocationDto_s;
using Entity.Entities;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Validation.CategoryValidator;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BLL.Persistence.Service.Concrete
{
    public class CategoryService : ICategoryService
    {

        public ICategoryRepository _repository { get; }
        public IMapper _mapper { get; }  //  okey

        public CategoryService(ICategoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task AddAsync(CategoryToAddDto categoryDto)
        {
            List<string> errors = new List<string>();
           

                CategoryToAddValidator validator = new CategoryToAddValidator();
                var model = JsonSerializer.Serialize(categoryDto);
                var result = validator.Validate(categoryDto);
                errors = result.Errors.Select(m => m.ErrorMessage).ToList();
                if (result.IsValid)
                {

                    var CheckParentCategory = await _repository.FindIsItParentCategoryOrNot(categoryDto.ParentCategoryId);
                    if (CheckParentCategory)
                    {
                        
                        Category category = _mapper.Map<Category>(categoryDto);
                        await _repository.AddAsync(category);
                        Log.Information($"{nameof(CategoryService)}.{nameof(AddAsync)} - Category added successfully. Data: {model}");
                    }
                    else
                    {

                        errors.Add("The specified ParentCategoryId is not a parent category.");
                        throw new InvalidOperationException($"{string.Join(", ", errors)}");

                    }
                }
                else
                {

                    throw new ValidationException($"Validation failed-{string.Join(", ", errors)}.");
                }

           
        }

        public async Task Delete(DeleteCategoryDTO entity)
        {
            List<string> errors = new List<string>();
            try
            {
                var model = JsonSerializer.Serialize(entity);

                DeleteCategoryValidator validator = new DeleteCategoryValidator();
                var result = validator.Validate(entity);
                 errors = result.Errors.Select(m => m.ErrorMessage).ToList();
                if (result.IsValid)
                {
                   
                    var existingCategory = await _repository.GetAsync(entity.Id);
                    if(existingCategory == null)
                    {
                        errors.Add("Category not found ");
                        throw new InvalidOperationException($"{string.Join(", ", errors)}");
                    }
                    else
                    {
                        var IsParentCategory = await _repository.FindIsItParentCategoryOrNot(entity.Id);
                        if (IsParentCategory)
                        {
                            errors.Add("You cannot delete ParentCategory ");
                            throw new InvalidOperationException($"{string.Join(", ", errors)}");
                        }
                        else
                        {
                            await _repository.Delete(existingCategory);

                            Log.Information($"{nameof(CategoryService)}.{nameof(Delete)} - Category Deleted successfully. Id: {model}");
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
                if(ex is ValidationException)
                {
                    Log.Error($"{nameof(CategoryService)}.{nameof(Delete)} - Validation failed. Errors: {string.Join(", ", errors)}");
                }
                if(ex is InvalidOperationException)
                {
                    Log.Error($"{nameof(CategoryService)}.{nameof(Delete)} - InvalidOperationException:Errors: {string.Join(", ", errors)}");
                }
                else
                {
                    Log.Error($"{nameof(CategoryService)}.{nameof(Delete)} - {ex.Message}");
                }
              
                throw;
            }
               
            

        }

        public async Task<List<CategoryToListDto>> GetAllAsync()
        {
            
                var result = new List<CategoryToListDto>();
                List<Category> categories = await _repository.GetAllAsync();
                if (categories.Count > 0)
                {



                    var parentCategories = new Dictionary<int, CategoryToListDto>();

                    foreach (var category in categories)
                    {
                        var categoryDto = _mapper.Map<CategoryToListDto>(category);

                        if (categoryDto.ParentCategoryId == null)
                        {

                            parentCategories.Add(categoryDto.Id, categoryDto);
                        }
                        else
                        {

                            if (parentCategories.TryGetValue(categoryDto.ParentCategoryId.Value, out var parentCategoryDto))
                            {
                                parentCategoryDto.ChildCategories ??= new List<CategoryToListDto>();
                                parentCategoryDto.ChildCategories.Add(categoryDto);
                            }
                        }
                    }


                    result.AddRange(parentCategories.Values);
                    Log.Information($"{nameof(CategoryService)}.{nameof(GetAllAsync)} - Categories Get successfully");
                }
                else
                {
                    Log.Error($"{nameof(CategoryService)}.{nameof(GetAllAsync)} - Categories not found");
                }


                return result;
           



        }

        public async Task<CategoryFindIdDTO> GetAsync(int id)
        {
            

                Category category = await _repository.GetAsync(id);
                var result = _mapper.Map<CategoryFindIdDTO>(category);
                if (result != null)
                {
                    Log.Information($"{nameof(CategoryService)}.{nameof(GetAsync)} - Category Get successfully.  ID: {id}");
                }
                else
                {
                    Log.Error($"{nameof(CategoryService)}.{nameof(GetAsync)} - Category not found. ID: {id}");
                }
                return result;


           

        }

        public async Task UpdateAsync(UpdateCategoryDto categoryDto)
        {
            List<string> errors = new List<string>();

           
                if (categoryDto == null)
                {
                    errors.Add("Dto is null");
                    throw new InvalidOperationException($"{string.Join(", ", errors)}");
                }
                else
                {
                    UpdateCategoryValidator validator = new UpdateCategoryValidator();
                    var model = JsonSerializer.Serialize(categoryDto);
                    var result = validator.Validate(categoryDto);
                    errors = result.Errors.Select(m => m.ErrorMessage).ToList();
                    if (result.IsValid)
                    {
                        var existingCategory = await _repository.GetAsync(categoryDto.Id);
                        if (existingCategory == null)
                        {
                            errors.Add("Category not found ");
                            throw new InvalidOperationException($"{string.Join(", ", errors)}");
                        }
                        else
                        {
                            var category = _mapper.Map(categoryDto, existingCategory);
                            var IsParentCategory = await _repository.FindIsItParentCategoryOrNot(categoryDto.ParentCategoryId);
                            if (IsParentCategory)
                            {
                                await _repository.UpdateAsync(category);
                                Log.Information($"{nameof(CategoryService)}.{nameof(UpdateAsync)} -Category updated successfully. Data: {model}");
                            }
                            else
                            {
                                errors.Add("The specified ParentCategoryId is not a parent category.");
                                throw new InvalidOperationException($"{string.Join(", ", errors)}");
                            }

                        }



                    }
                    else
                    {

                        throw new ValidationException($"Validation failed-{string.Join(", ", errors)}.");
                    }
                }





        }

        public async Task<List<FindParentsCategoryDto>> FindParentCategory()
        {
           

                List<Category> categories = await _repository.GetAllAsync();
                var result = new List<FindParentsCategoryDto>();
                if (categories.Count > 0)
                {

                    foreach (var category in categories)
                    {
                        if (category.ParentCategory == null)
                        {
                            var categoryDto = new FindParentsCategoryDto
                            {
                                Id = category.Id,
                                Name = category.Name
                            };

                            result.Add(categoryDto);
                        }
                    }
                    Log.Information($"{nameof(CategoryService)}.{nameof(FindParentCategory)} - ParentCategories Get successfully.");
                }
                else
                {
                    Log.Error($"{nameof(CategoryService)}.{nameof(FindParentCategory)} - ParentCategories not found.");
                }

                return result;
            




        }

        public async Task<List<GetChildCategoryWithParentCategoryId>> GetChildCategoryWithParentCategoryId(int id)
        {
            List<string> errors = new List<string>();
            
                var categories = await _repository.GetAllAsync();
                var result = new List<GetChildCategoryWithParentCategoryId>();

                if (categories.Count > 0)
                {
                    var IsparentCategory = await _repository.FindIsItParentCategoryOrNot(id);
                    if (IsparentCategory)
                    {
                        foreach (var category in categories)
                        {
                            if (category.ParentCategoryId == id)
                            {
                                var categoryDto = new GetChildCategoryWithParentCategoryId
                                {
                                    Id = category.Id,
                                    Name = category.Name
                                };
                                result.Add(categoryDto);
                            }
                        }
                        Log.Information($"{nameof(CategoryService)}.{nameof(GetChildCategoryWithParentCategoryId)} - ChildCategories Get successfully. with this ParentCategory ID: {id}");
                    }
                    else
                    {
                        errors.Add("The specified ParentCategoryId is not a parent category.");


                        throw new InvalidOperationException($"{string.Join(", ", errors)}");
                    }


                }
                else
                {
                    Log.Error($"{nameof(CategoryService)}.{nameof(GetChildCategoryWithParentCategoryId)} - ChildCategories not found.");
                }

                return result;
           


        }

        public async Task<bool> FindIsItParentCategoryOrNot(int categoryId)
        {
            var result = await _repository.FindIsItParentCategoryOrNot(categoryId);
            return result;
        }
    }
}
