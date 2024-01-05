using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Entities
{
    public class ImagesPath :BaseEntity
    {
        public int ProductId { get; set; }
        public string Path { get; set; }
        
    }
}
