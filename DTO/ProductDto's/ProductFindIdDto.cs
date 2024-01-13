using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.ProductDto_s
{

    public class ProductFindIdDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsNew { get; set; }
        public bool IsDelivered { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }


    }
}
