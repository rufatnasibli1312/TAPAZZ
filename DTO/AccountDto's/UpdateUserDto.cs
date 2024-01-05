using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.AccountDto_s
{
    public class UpdateUserDto
    {
        public string Id { get; set; }
        public string Fullname { get; set; }
        public string Address { get; set; }
        public IFormFile newProfilePhoto { get; set; }
        
        public string PhoneNumber { get; set; }
    }
}
