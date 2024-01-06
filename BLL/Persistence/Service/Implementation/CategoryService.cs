using AutoMapper;
using BLL.Persistence.Service.Abstract;
using DAL.Persistence.Repository.Abstract;
using DAL.Persistence.Repository.Implementation;
using DTO.CategoryDto_s;
using DTO.LocationDto_s;
using Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Persistence.Service.Concrete
{
    public class CategoryService : ICategoryService
    {

        public ICategoryRepository _repository { get; }
        public IMapper _mapper { get; }

        public CategoryService(ICategoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        public async Task AddAsync(CategoryToAddDto categoryDto)
        {
            Category category = _mapper.Map<Category>(categoryDto);
            await _repository.AddAsync(category);
        }

        public async Task Delete(DeleteCategoryDTO entity)
        {
            var existingLocation = await _repository.GetAsync(entity.Id);
            await _repository.Delete(existingLocation);
        }

        public async Task<List<CategoryToListDto>> GetAllAsync()
        {
            List<Category> categories = await _repository.GetAllAsync();
            var result = new List<CategoryToListDto>();


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

            return result;


        }

        public async Task<CategoryFindIdDTO> GetAsync(int id)
        {
            Category category = await _repository.GetAsync(id);
            var result = _mapper.Map<CategoryFindIdDTO>(category);
            return result;
        }

        public async Task UpdateAsync(UpdateCategoryDto categoryDto)
        {

            var existingCategory = await _repository.GetAsync(categoryDto.Id);

            var category = _mapper.Map(categoryDto, existingCategory);

            if (category != null)
            {

                await _repository.UpdateAsync(category);
            }

        }

        public async Task<List<FindParentsCategoryDto>> FindParentCategory()
        {
            List<Category> categories = await _repository.GetAllAsync();
            var result = new List<FindParentsCategoryDto>();
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

            return result;




        }

        public async Task<List<GetChildCategoryWithParentCategoryId>> GetChildCategoryWithParentCategoryId(int id)
        {
            var categories = await _repository.GetAllAsync();
            var result = new List<GetChildCategoryWithParentCategoryId>();

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

            return result;
        }

        
    }
}
