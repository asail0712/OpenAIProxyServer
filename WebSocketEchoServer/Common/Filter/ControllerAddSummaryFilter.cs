using XPlan.Utility.Filter;

namespace Common.Filter
{
    public class ControllerAddSummaryFilter : AddSummaryFilter
    {
        protected override Dictionary<string, string> GetSummaryInfo()
        {
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>()
            {               
                // 基本
                { "AuthController.Login", "使用者登入" },
                { "AuthController.CreateIdentity", "創建新帳號" },
                { "AuthController.ChangePassword", "改變密碼" },
            };

            return keyValuePairs;
        }
    }
}
