using AutoMapper;
using Common.DTO.Login;
using DataAccess.Interface;
using XPlan.DataAccess;

namespace DataAccess
{
    public class AuthDataAccess : MongoEntityDataAccess<AuthEntity, AuthDocument>, IAuthDataAccess
    {
        public AuthDataAccess(IMapper mapper)
            : base(mapper)
        {
            EnsureIndexCreated("Account");
            AddNoUpdateKey("PasswordHash");
        }
    }
}
