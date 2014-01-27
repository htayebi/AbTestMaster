using System.Web.Mvc;

namespace AbTestMaster.MvcExtensions
{
    public class AbTestMasterController : Controller
    {
        protected override IActionInvoker CreateActionInvoker()
        {
            return new AbTestMasterActionInvoker();
        }
    }
}

