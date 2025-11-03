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
                { "LoginController.Create", "新增Login資料" },
                { "LoginController.GetAll", "取得所有Login資料" },
                { "LoginController.Get", "依據 ID 取得Login資料" },
                { "LoginController.Update", "更新Login資料" },
                { "LoginController.Delete", "刪除Login資料" },
            };

            return keyValuePairs;
        }
    }
}
