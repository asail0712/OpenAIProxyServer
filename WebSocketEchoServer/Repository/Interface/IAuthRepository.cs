using Common.DTO.Auth;

using XPlan.Repository;

namespace Repository.Interface
{
    public interface IAuthRepository : IRepository<AuthEntity>
    {
        Task<bool> ChangePassword(string key, string pwHash);
    }
}
