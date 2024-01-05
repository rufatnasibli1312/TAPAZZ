using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.AccountDto_s
{
    public class RegisterModelDto
    {

        public string Email { get; set; }


        public string Password { get; set; }


        public string Fullname { get; set; }


        public string Address { get; set; }



        public IFormFile Photo { get; set; }

        public string PhoneNumber { get; set; }




    }
}
