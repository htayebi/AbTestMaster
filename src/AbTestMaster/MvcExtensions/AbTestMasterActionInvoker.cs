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
                TargetService.WriteToTargets(selectedSplit);
                HttpHelpers.SaveToCookie(selectedSplit);
                HttpHelpers.SaveToSession(selectedSplit);
            }
            else if (splitGoal != null)
            {
                TargetService.WriteToTargets(splitGoal);
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
            bool cookieValid =
                (cookieSplit != null)
                && eligibleSplitCases.Any(
                    s =>
                        s.SplitViewName == cookieSplit.SplitViewName
                        && s.SplitGroup == cookieSplit.SplitGroup
                        && s.Goal == cookieSplit.Goal
                        && (!s.Ratio.HasValue || s.Ratio > 0));

            return cookieValid ? cookieSplit : PickSplitRandomly(eligibleSplitCases);
        }

        private SplitView PickSplitRandomly(List<SplitView> splits)
        {
            var nonZeroSplits = splits.Where(s => !s.Ratio.HasValue || s.Ratio.Value > 0).ToList();
            double sum = splits.Where(s => s.Ratio.HasValue).Sum(s => s.Ratio.Value);

            //if sum of ratios assigned is greater than one, ignore them and split the ratio evenly
            if (sum > 1)
            {
                var count = (double)nonZeroSplits.Count;
                foreach (var splitView in nonZeroSplits)
                {
                    splitView.Ratio = 1/count;
                }
            }

            //if sum of ratios assigned is smaller than one, some redistribution needs to be done
            if (sum < 1)
            {
                var splitsWithNoRatio = nonZeroSplits.Where(s => !s.Ratio.HasValue).ToList();
                var count = (double)splitsWithNoRatio.Count;

                double ratioLeft = 1 - sum;

                // if there are splitvies with unassigned ratios, distribute the remaining percentage to them
                if (count > 0)
                {
                    foreach (var splitView in nonZeroSplits.Where(n => !n.Ratio.HasValue))
                    {
                        splitView.Ratio = ratioLeft / count;
                    }
                }
                // else, time the current splitview ratios propotionally
                else
                {
                    foreach (var splitView in nonZeroSplits)
                    {
                        splitView.Ratio = splitView.Ratio / sum;
                    }
                }
            }

            double randomNumber = PickRandom();
            int elementIndex = -1;
            double accumulativeRatio = 0;

            do
            {
                elementIndex++;
                accumulativeRatio += nonZeroSplits.ElementAt(elementIndex).Ratio.Value;
            } while (accumulativeRatio < randomNumber);

            return nonZeroSplits.ElementAt(elementIndex);
        }

        private static double PickRandom()
        {
            var rnd = new Random();
            return rnd.NextDouble();
        }
        #endregion
    }
}
