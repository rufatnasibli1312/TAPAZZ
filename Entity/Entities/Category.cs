using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Entities
{
    public class Category : BaseEntity
    {

        public string Name { get; set; }
        public int? ParentCategoryId { get; set; }
        public Category ParentCategory { get; set; }
        public List<Product>? Products { get; set; }
    }

}
