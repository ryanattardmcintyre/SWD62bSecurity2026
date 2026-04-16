using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.CustomExceptions
{
    public class CreateEventException: Exception
    {
        public CreateEventException(): base("An other event with the same name already exists")
        {
        }
        public CreateEventException(string message): base(message)
        {
        }
        public CreateEventException(string message, Exception innerException): base(message, innerException)
        {
        }
    }
}
