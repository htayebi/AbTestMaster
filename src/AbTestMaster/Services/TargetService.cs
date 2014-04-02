using System.Collections.Generic;
using System.Linq;
using AbTestMaster.Configuration;
using AbTestMaster.Domain;
using AbTestMaster.Initialization;
using AbTestMaster.Target;

namespace AbTestMaster.Services
{
    internal class TargetService
    {
        private static List<TargetBase> _targets;
        private static AbTestMasterSection _config;

        internal static List<TargetBase> Targets
        {
            get
            {
                if (_targets == null)
                {
                    _targets = TargetFinder.FindTargets();
                }

                return _targets;
            }
            set
            {
                _targets = value;
            }
        }

        internal static void WriteToTargets(SplitView split)
        {
            foreach (var target in Targets.Where(t => t.DataType == TargetDataType.Views))
            {
                target.Write(split);
            }
        }

        internal static void WriteToTargets(SplitGoal split)
        {
            foreach (var target in Targets.Where(t => t.DataType == TargetDataType.Goals))
            {
                target.Write(split);
            }
        }

        internal static AbTestMasterSection Config
        {
            get
            {
                if (_config == null)
                {
                    _config = TargetFinder.FindConfig();
                }

                return _config;
            }

            set
            {
                _config = value;
            }
        }
    }
}
