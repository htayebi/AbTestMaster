using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AbTestMaster.Domain;
using AbTestMaster.Services;

namespace AbTestMaster.MvcExtensions
{
    internal class AbTestMasterActionInvoker : ControllerActionInvoker
    {
        public override bool InvokeAction(ControllerContext controllerContext, string actionName)
        {
            SplitGoal splitGoal = HandleGoalCall(controllerContext, actionName);

            List<SplitView> splitViews = SplitServices.SplitViews;
            SplitView splitView = GetCurrentSplit(controllerContext, actionName, splitViews);

            SplitView selectedSplit = null;

            if (splitView != null && controllerContext.RouteData.Values[Constants.ACTION].ToString() == actionName)
            {
                List<SplitView> all = splitViews.FindAll(s => s.SplitGroup == splitView.SplitGroup);
                selectedSplit = ChooseSplit(all, splitView.SplitGroup);

                controllerContext.RouteData.Values[Constants.ACTION] = selectedSplit.Action;
                controllerContext.RouteData.Values[Constants.CONTROLLER] = selectedSplit.Controller;

                AddRemoveArea(controllerContext, selectedSplit);

                actionName = selectedSplit.Action;
            }

            bool success = base.InvokeAction(controllerContext, actionName);

            if (!success || !HttpHelpers.IsIn200Family(controllerContext.HttpContext.Response.StatusDescription))
            {
                return success;
            }

            if (selectedSplit != null)
            {
                var logservice = new LogService();
                logservice.WriteToFile(selectedSplit);
                HttpHelpers.SaveToCookie(selectedSplit);
                HttpHelpers.SaveToSession(selectedSplit);
            }
            else if (splitGoal != null)
            {
                var logservice = new LogService();
                logservice.WriteToFile(splitGoal);
                HttpHelpers.RemoveFromSession(splitGoal);
            }

            return true;
        }

        #region private methods
        private void AddRemoveArea(ControllerContext controllerContext, SplitView selectedSplit)
        {
            if (String.IsNullOrEmpty(selectedSplit.Area))
            {
                if (controllerContext.RouteData.Values.ContainsKey(Constants.AREA))
                {
                    controllerContext.RouteData.Values.Remove(Constants.AREA);
                }
            }
            else
            {
                if (controllerContext.RouteData.Values.ContainsKey(Constants.AREA))
                {
                    controllerContext.RouteData.Values[Constants.AREA] = selectedSplit.Area;
                }
                else
                {
                    controllerContext.RouteData.Values.Add(Constants.AREA, selectedSplit.Area);
                }
            }

        }

        private SplitGoal HandleGoalCall(ControllerContext controllerContext, string actionName)
        {
            List<SplitGoal> splitGoals = SplitServices.SplitGoals;
            string controllerName = controllerContext.RouteData.Values[Constants.CONTROLLER].ToString();
            string areaName = GetAreaName(controllerContext);

            SplitGoal splitGoal = splitGoals.SingleOrDefault(s =>
                String.Equals(s.Action, actionName, StringComparison.InvariantCultureIgnoreCase)
                && String.Equals(s.Controller, controllerName, StringComparison.InvariantCultureIgnoreCase)
                && String.Equals(s.Area, areaName, StringComparison.InvariantCultureIgnoreCase));

            return splitGoal;
        }

        private SplitView GetCurrentSplit(ControllerContext controllerContext, string actionName, IEnumerable<SplitView> splitViews)
        {
            string controllerName = controllerContext.RouteData.Values[Constants.CONTROLLER].ToString();
            string areaName = GetAreaName(controllerContext);

            return splitViews.SingleOrDefault(s =>
                String.Equals(s.Action, actionName, StringComparison.InvariantCultureIgnoreCase)
                && String.Equals(s.Controller, controllerName, StringComparison.InvariantCultureIgnoreCase)
                && String.Equals(s.Area, areaName, StringComparison.InvariantCultureIgnoreCase));
        }

        private string GetAreaName(ControllerContext controllerContext)
        {
            string areaName = null;

            if (controllerContext.RouteData.Values.ContainsKey(Constants.AREA))
            {
                areaName = controllerContext.RouteData.Values[Constants.AREA].ToString();
            }

            return areaName;
        }

        private SplitView ChooseSplit(List<SplitView> eligibleSplitCases, string splitGroup)
        {
            SplitView cookieSplit = HttpHelpers.ReadFromCookie(splitGroup);

            //make sure splitview in cookie is still in use
            bool cookieValid = ( cookieSplit != null ) && eligibleSplitCases.Any(
                s => s.SplitViewName == cookieSplit.SplitViewName && s.SplitGroup == cookieSplit.SplitGroup);

            return cookieValid ? cookieSplit : PickSplitRandomly(eligibleSplitCases);
        }

        private SplitView PickSplitRandomly(List<SplitView> splits)
        {
            var randomIndex = PickRandom(0, splits.Count);
            return splits.ElementAt(randomIndex);
        }

        private static int PickRandom(int min, int max)
        {
            var rnd = new Random();
            return rnd.Next(min, max);
        }
        #endregion
    }
}
