using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.CustomException
{
    public class ValidationException : Exception
    {
        public ValidationException() { }

        public ValidationException(string message) : base(message) { }

        public ValidationException(string message,List<string> error ) : base(message) { }

        public ValidationException(string message, Exception innerException) : base(message, innerException) { }
    }
}
