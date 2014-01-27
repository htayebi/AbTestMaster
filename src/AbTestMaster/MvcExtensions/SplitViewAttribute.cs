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

        public SplitViewAttribute(string splitName, string splitGroup, string sequence)
        {
            SplitView = new SplitView { SplitGroup = splitGroup, SplitViewName = splitName, Sequence = sequence };
        }

        internal SplitView SplitView { get; set; }
    }
}
