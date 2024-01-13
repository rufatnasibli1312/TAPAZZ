using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Entity.Interface;

namespace Entity.Entities
{
    public class Product : BaseEntity, IAuditEntity
    {
        public Product()
        {
            PhotoPath = new HashSet<ImagesPath>();
        }
        [MaxLength(200)]
        public string Name { get; set; }
        [MaxLength(200)]
        public string Description { get; set; }
        public bool IsNew { get; set; }
        public bool IsDelivered { get; set; }
        public decimal Price { get; set; }
        public Category Category { get; set; }
        public int CategoryID { get; set; }
        public User User { get; set; }
        public string UserID { get; set; }


        public DateTime ExpireDate { get; set; }
        public bool IsActive { get; set; }
        public ICollection<ImagesPath> PhotoPath { get; set; }
        public DateTime CreateDate { get; set; }
        public int CityId { get; set; }
        public City City { get; set; }



    }


}
