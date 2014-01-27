namespace AbTestMaster.Initialization
{
    public class AbTestMasterBootstrapper
    {
        internal static string AssmeblyName;

        public static void Initialize(string assemblyName)
        {
            AssmeblyName = assemblyName;
        }
    }
}
