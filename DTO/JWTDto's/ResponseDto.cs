using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.JWTDto_s
{
    public  class ResponseDto
    {
        public  string AccessToken { get; set; }
        public  string RefreshToken { get; set; }
    }
}
