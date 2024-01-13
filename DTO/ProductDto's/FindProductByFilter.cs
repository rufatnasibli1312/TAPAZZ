using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.ProductDto_s
{
    public class FindProductByFilter
    {
       
        public string? Name { get; set; }

       
        public string? Description { get; set; }
       

        
        public int CityId { get; set; }
        
    }
}
