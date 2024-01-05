using Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.CategoryDto_s
{
    public class CategoryToListDto
    {
       
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ParentCategoryId { get; set; }
        public Category ParentCategory { get; set; }
        public List<CategoryToListDto> ChildCategories { get; set; }
        public List<Product>? Products { get; set; }
    }
}
