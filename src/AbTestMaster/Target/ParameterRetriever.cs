using System;
using System.Web;
using AbTestMaster.Domain;
using AbTestMaster.Services;

namespace AbTestMaster.Target
{
    internal class ParameterRetriever
    {
        internal static object RetireveValue(string parameterName)
        {
            object value;

            switch (parameterName)
            {
                case Constants.DATE_TIME_VARIABLE:
                    value = DateTime.UtcNow;
                    break;
                case Constants.IP_ADDRESS_VARIABLE:
                    value = HttpContext.Current.Request.UserHostAddress;
                    break;
                case Constants.BROWSER_VARIABLE:
                    value = HttpContext.Current.Request.Browser.Browser;
                    break;
                case Constants.USER_AGENT_VARIABLE:
                    value = HttpContext.Current.Request.UserAgent;
                    break;
                default:
                    throw new Exception(string.Format("invalid parameter name {0}", parameterName));
            }

            return value;
        }
        internal static object RetireveValue(SplitView view, string parameterName)
        {
            object value;

            switch (parameterName)
            {
                case Constants.SPLIT_GROUP_VARIABLE:
                    value = view.SplitGroup;
                    break;
                case Constants.SPLIT_NAME_VARIABLE:
                    value = view.SplitViewName;
                    break;
                case Constants.SPLIT_GOAL_VARIABLE:
                    value = view.Goal;
                    break;
                default:
                    value = RetireveValue(parameterName);
                    break;
            }

            return value;
        }

        internal static object RetireveValue(SplitGoal goal, string parameterName)
        {
            object value;

            switch (parameterName)
            {
                case Constants.SPLIT_VIEWS_SEQUENCE_TRAIL_VARIABLE:
                    value = HttpHelpers.GetViewTrail(goal.Goal);
                    break;
                case Constants.SPLIT_GOAL_VARIABLE:
                    value = goal.Goal;
                    break;
                default:
                    value = RetireveValue(parameterName);
                    break;
            }

            return value;
        }

    }
}
