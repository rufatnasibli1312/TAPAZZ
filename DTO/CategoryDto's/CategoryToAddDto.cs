﻿using Entity.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.CategoryDto_s
{
    public class CategoryToAddDto
    {
        public string Name { get; set; }
        public int ParentCategoryId { get; set; }
        

    }
}
