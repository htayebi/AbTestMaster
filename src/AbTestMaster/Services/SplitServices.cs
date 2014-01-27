using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using AbTestMaster.Domain;
using AbTestMaster.Initialization;
using AbTestMaster.MvcExtensions;

namespace AbTestMaster.Services
{
    internal class SplitServices
    {
        private static List<SplitView> _splitViews;
        private static List<SplitGoal> _splitgoals;

        #region internal properties
        internal static List<SplitView> SplitViews
        {
            get
            {
                if (_splitViews == null)
                {
                    _splitViews = FindSplitViews();
                }

                return _splitViews;
            }
        }

        internal static List<SplitGoal> SplitGoals
        {
            get
            {
                if (_splitgoals == null)
                {
                    _splitgoals = FindSplitGoals();
                }

                return _splitgoals;
            }
        }

        #endregion

        #region private methods
        private static List<SplitView> FindSplitViews()
        {
            var groups = new List<SplitView>();

            List<Type> allValidControllers =
                GetAllControllers<AbTestMasterController>(GetAssemblyByName(AbTestMasterBootstrapper.AssmeblyName));

            foreach (Type controllerType in allValidControllers)
            {
                var currentControllerDescriptor = new ReflectedControllerDescriptor(controllerType);
                var actions = currentControllerDescriptor.GetCanonicalActions();
                string area = GetAreaName(currentControllerDescriptor.ControllerType.FullName);

                foreach (ActionDescriptor action in actions)
                {
                    var splitViewAttributes = action.GetCustomAttributes(typeof(SplitViewAttribute), false) as SplitViewAttribute[];

                    if (splitViewAttributes != null)
                    {
                        foreach (var splitViewAttribute in splitViewAttributes)
                        {
                            groups.Add(new SplitView
                            {
                                SplitViewName = splitViewAttribute.SplitView.SplitViewName,
                                SplitGroup = splitViewAttribute.SplitView.SplitGroup,
                                Sequence = splitViewAttribute.SplitView.Sequence,
                                Action = action.ActionName,
                                Controller = currentControllerDescriptor.ControllerName,
                                Area = area
                            });
                        }
                    }
                }
            }

            return groups;
        }

        private static List<SplitGoal> FindSplitGoals()
        {
            var groups = new List<SplitGoal>();

            List<Type> allValidControllers =
                GetAllControllers<AbTestMasterController>(GetAssemblyByName(AbTestMasterBootstrapper.AssmeblyName));

            foreach (var controllerType in allValidControllers)
            {
                var currentControllerDescriptor = new ReflectedControllerDescriptor(controllerType);
                ActionDescriptor[] actions = currentControllerDescriptor.GetCanonicalActions();
                string area = GetAreaName(currentControllerDescriptor.ControllerType.FullName);

                foreach (ActionDescriptor action in actions)
                {
                    var splitGoalAttributes = action.GetCustomAttributes(typeof(SplitGoalAttribute), false) as SplitGoalAttribute[];

                    if (splitGoalAttributes != null)
                    {
                        foreach (var splitGoalAttribute in splitGoalAttributes)
                        {
                            groups.Add(new SplitGoal
                            {
                                GoalName = splitGoalAttribute.SplitGoal.GoalName,
                                Sequence = splitGoalAttribute.SplitGoal.Sequence,
                                Action = action.ActionName,
                                Controller = currentControllerDescriptor.ControllerName,
                                Area = area
                            });
                        }
                    }
                }
            }

            return groups;
        }

        private static string GetAreaName(string fullName)
        {
            string area = null;

            string[] namepieces = fullName.Split('.');
            int? areasIndex = null, controllerIndex = null;

            for (int i = 0; i < namepieces.Length; i++)
            {
                if (namepieces[i].ToLower() == "areas" && !areasIndex.HasValue)
                {
                    areasIndex = i;
                }

                if (namepieces[i].ToLower() == "controllers")
                {
                    controllerIndex = i;
                }
            }

            if (!controllerIndex.HasValue)
            {
                throw new Exception("Controller name was not found in the controller's full name");
            }

            if (areasIndex.HasValue)
            {
                var sb = new StringBuilder();
                for (int i = (areasIndex.Value + 1); i < controllerIndex.Value; i++)
                {
                    sb.Append(namepieces[i]);
                }
                area = sb.ToString();
            }

            return area;
        }

        private static Assembly GetAssemblyByName(string assmeblyName)
        {
            return Assembly.Load(assmeblyName);
        }

        private static List<Type> GetAllControllers<T>(Assembly assembly)
        {
            var derivedType = typeof(T);

            return
                assembly
                .GetTypes()
                .Where(t => t != derivedType && t.IsSubclassOf(derivedType)).ToList();
        }

        #endregion
    }
}
