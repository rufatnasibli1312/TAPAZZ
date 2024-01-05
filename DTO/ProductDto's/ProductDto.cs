using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.LocationDto_s
{
    public class ProductDto
    {
        public bool Isnew { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsDelivered { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}
