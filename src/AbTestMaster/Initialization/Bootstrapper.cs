using AbTestMaster.Services;

namespace AbTestMaster.Initialization
{
    public class AbTestMasterBootstrapper
    {
        internal static string AssmeblyName;

        public static void Initialize(string assemblyName)
        {
            AssmeblyName = assemblyName;

            //on application initialise, load all views, goals, configuration & targets
            SplitServices.SplitViews = SplitFinder.FindSplitViews();
            SplitServices.SplitGoals = SplitFinder.FindSplitGoals();
            TargetService.Config = TargetFinder.FindConfig();
            TargetService.Targets = TargetFinder.FindTargets();
        }
    }
}
