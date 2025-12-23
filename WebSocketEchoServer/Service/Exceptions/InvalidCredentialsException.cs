using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPlan.Utility.Exceptions;

namespace Service.Exceptions
{
    public class InvalidCredentialsException : CustomException
    {
        public InvalidCredentialsException()
            : base($"Invalid account or password.") { }
    }
}
