using AbTestMaster.Initialization;
 
[assembly: WebActivator.PostApplicationStartMethod(typeof(Web.AbTestMasterInitializer), "AfterStart")]
 
namespace Web {
    public static class AbTestMasterInitializer {
        public static void AfterStart() {
            AbTestMasterBootstrapper.Initialize("Web");
        }
    }
}