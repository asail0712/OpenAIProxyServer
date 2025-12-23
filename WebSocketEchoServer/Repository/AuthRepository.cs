using Common.DTO.Auth;
using DataAccess.Interface;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Repository.Interface;
using XPlan.DataAccess;
using XPlan.DTO;
using XPlan.Repository;
using XPlan.Utility.Caches;

namespace Repository
{
    public class AuthRepository : GenericRepository<AuthEntity, IAuthDataAccess>, IAuthRepository
    {
        public AuthRepository(IAuthDataAccess dataAccess, IMemoryCache memoryCache, IOptions<CacheSettings> cacheSettings) 
            : base(dataAccess, memoryCache, cacheSettings)
        {

        }

        public async Task<bool> ChangePassword(string account, string newPasswordHash)
        {
            var definition = new FindUpdateDefinition<AuthEntity>()
                .Where(_ => Builders<AuthEntity>.Filter.Eq(x => x.Account, account))
                .UpdateDef(_ => Builders<AuthEntity>.Update
                    .Set(x => x.PasswordHash, newPasswordHash)
                    .Set(x => x.UpdatedAt, DateTime.UtcNow)
                );

            var updated = await _dataAccess.FindOneAndUpdateAsync(definition);
            return updated != null;
        }
    }
}
