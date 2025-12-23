using XPlan.Entities;

namespace Common.DTO.Login
{
    public class AuthEntity : IDBEntity
    {        
        public string Id { get; set; }              = "";
        public DateTime CreatedAt { get; set; }             // 建立時間
        public DateTime UpdatedAt { get; set; }             // 更新時間

        public AuthEntity() 
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
