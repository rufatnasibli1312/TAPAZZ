using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.ProductDto_s
{
    public class UpdateProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsNew { get; set; }
        public bool IsDelivered { get; set; }
        public int Price { get; set; }
        public List<IFormFile> newPhotos { get; set; }
        public int CityId { get; set; }
        public int CategoryId { get; set; }
    }
}
