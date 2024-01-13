using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.Entities
{
    public class User : IdentityUser
    {

        public string Fullname { get; set; }
        public string Address { get; set; }
        public string? PhotoPath { get; set; }
        public string? PhotoName { get; set; }
        public List<Product>? Products { get; set; }

        public ICollection<Complaint> Complaints { get; set; }

        public ICollection<Favorite> Favorites { get; set; }



    }

}
