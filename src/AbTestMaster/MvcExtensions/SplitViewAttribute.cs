using System.Web.Mvc;
using AbTestMaster.Domain;

namespace AbTestMaster.MvcExtensions
{
    public class SplitViewAttribute : ActionFilterAttribute
    {
        public SplitViewAttribute(string splitName, string splitGroup)
        {
            SplitView = new SplitView { SplitGroup = splitGroup, SplitViewName = splitName };
        }

        public SplitViewAttribute(string splitName, string splitGroup, double ratio)
        {
            SplitView = new SplitView { SplitGroup = splitGroup, SplitViewName = splitName, Ratio = ratio };
        }

        public SplitViewAttribute(string splitName, string splitGroup, string goal)
        {
            SplitView = new SplitView { SplitGroup = splitGroup, SplitViewName = splitName, Goal = goal };
        }

        public SplitViewAttribute(string splitName, string splitGroup, string goal, double ratio)
        {
            SplitView = new SplitView { SplitGroup = splitGroup, SplitViewName = splitName, Goal = goal, Ratio = ratio };
        }

        internal SplitView SplitView { get; set; }
    }
}
