using System.Collections.Generic;
using AbTestMaster.Domain;
using AbTestMaster.Initialization;

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
                    _splitViews = SplitFinder.FindSplitViews();
                }

                return _splitViews;
            }
            set
            {
                _splitViews = value;
            }
        }

        internal static List<SplitGoal> SplitGoals
        {
            get
            {
                if (_splitgoals == null)
                {
                    _splitgoals = SplitFinder.FindSplitGoals();
                }

                return _splitgoals;
            }
            set
            {
                _splitgoals = value;
            }
        }

        #endregion
    }
}
