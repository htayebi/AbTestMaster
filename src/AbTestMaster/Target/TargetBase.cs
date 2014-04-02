using System.Collections.Generic;
using AbTestMaster.Domain;

namespace AbTestMaster.Target
{
    internal abstract class TargetBase
    {
        internal string Name { get; set; }
        internal string Type { get; set; }
        internal Dictionary<string, string> Parameters { get; set; }
        internal TargetDataType DataType { get; set; }
        internal abstract void Write(SplitView split);
        internal abstract void Write(SplitGoal goal);
    }
}
