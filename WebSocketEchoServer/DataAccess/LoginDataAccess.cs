using AutoMapper;
using Common.DTO.Login;
using DataAccess.Interface;
using XPlan.DataAccess;

namespace DataAccess
{
    public class LoginDataAccess : MongoEntityDataAccess<LoginEntity, LoginDocument>, ILoginDataAccess
    {
        public LoginDataAccess(IMapper mapper)
            : base(mapper)
        {
        }
    }
}
