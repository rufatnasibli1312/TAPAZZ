using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.ResponseDto_s
{
    public class ErrorResponseDto
    {
        public List<string> Errors { get; set; }

        public ErrorResponseDto(List<string> errors)
        {
            Errors = errors;
        }

        public ErrorResponseDto(string error)
        {
            Errors = new List<string> { error };
        }
    }
}
