using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPlan.Utility.Exceptions;

namespace XPlan.Exceptions
{
    public class ServiceUnavailableException : CustomException
    {
        public ServiceUnavailableException(string reason)
            : base("Service is temporarily unavailable. Please try again later.")
        {
            // reason 只用來 log，不顯示給 client
            Reason = reason;
        }
        public string Reason { get; }
    }
}
