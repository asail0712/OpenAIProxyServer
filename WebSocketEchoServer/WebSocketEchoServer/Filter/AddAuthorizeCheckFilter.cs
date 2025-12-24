using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPlan.Utility.Filter;

namespace OpenAIProxyService.Controllers
{
    public class AddAuthorizeCheckFilter : AuthorizeCheckFilter
    {
        protected override HashSet<string> GetAuthorizedApi()
        {
            HashSet<string> hiddenSet = new HashSet<string>()
            {

            };

            return hiddenSet;
        }
    }
}
