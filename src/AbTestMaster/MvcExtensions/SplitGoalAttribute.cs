using System.Web.Mvc;
using AbTestMaster.Domain;

namespace AbTestMaster.MvcExtensions
{
    public class SplitGoalAttribute : ActionFilterAttribute
    {
        public SplitGoalAttribute(string goalName)
        {
            SplitGoal = new SplitGoal { Goal = goalName };
        }

        internal SplitGoal SplitGoal { get; set; }
    }
}
