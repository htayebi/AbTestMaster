using System.Web.Mvc;
using AbTestMaster.MvcExtensions;

namespace Web.Controllers
{
    public class HomeController : AbTestMasterController
    {
        [SplitView("PositiveVersion", "LandingTest", "ReceiptPageGoal")]
        public ActionResult IndexVariantA()
        {
            return View();
        }

        [SplitView("SmearCampaignVersion", "LandingTest", "ReceiptPageGoal")]
        public ActionResult IndexVariantB()
        {
            return View();
        }

        public ActionResult Payment()
        {
            return View();
        }

        [SplitView("Striped", "TableTest", "ReceiptPageGoal")]
        public PartialViewResult TableVariantA()
        {
            return PartialView("Partials/_TableVariantA");
        }

        [SplitView("PlainWithColor", "TableTest", "ReceiptPageGoal", .6)]
        public PartialViewResult TableVariantB()
        {
            return PartialView("Partials/_TableVariantB");
        }

        [SplitView("RedDanger", "WarningTest", "ReceiptPageGoal", .8)]
        public PartialViewResult WarningVariantA()
        {
            return PartialView("Partials/_WarningVariantA");
        }

        [SplitView("BlueDisclaimer", "WarningTest", "ReceiptPageGoal", .2)]
        public PartialViewResult WarningVariantB()
        {
            return PartialView("Partials/_WarningVariantB");
        }

        [SplitGoal("ReceiptPageGoal")]
        public ActionResult Receipt()
        {
            return View();
        }

        public ViewResult About()
        {
            return View();
        }
    }
}