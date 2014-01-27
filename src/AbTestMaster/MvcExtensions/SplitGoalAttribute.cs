using System.Web.Mvc;
using AbTestMaster.Domain;

namespace AbTestMaster.MvcExtensions
{
    public class SplitGoalAttribute : ActionFilterAttribute
    {
        public SplitGoalAttribute(string sequence)
        {
            SplitGoal = new SplitGoal { Sequence = sequence };
        }

        internal SplitGoal SplitGoal { get; set; }
    }
}
