using XPlan.Utility.Filter;

namespace Common.Filter
{
    public class ApiHiddenFilter : HiddenApiDocumentFilter
    {
        protected override HashSet<string> GetHiddenList()
        {
            HashSet<string> hiddenSet = new HashSet<string>()
            {                
                "AuthController.Create",
                "AuthController.GetAll",
                "AuthController.Get",
                "AuthController.Update",
                "AuthController.Delete"
            };

            return hiddenSet;
        }
    }
}
