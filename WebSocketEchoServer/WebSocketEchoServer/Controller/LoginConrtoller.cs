using Common.DTO.Login;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPlan.Controller;

namespace OpenAIProxyService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : GenericController<LoginRequest, LoginResponse, ILoginService>
    {
        public LoginController(ILoginService service)
            : base(service)
        {

        }
    }
}
