using System;
using System.Linq;
using AbTestMaster.Domain;
using AbTestMaster.Services;

namespace AbTestMaster.Target
{
    internal class FileTarget : TargetBase
    {
        internal string Path { get; set; }
        internal override void Write(SplitView split)
        {
            try
            {
                var logservice = new LogService();

                var values = Parameters.ToDictionary(parameter => parameter.Key, parameter => ParameterRetriever.RetireveValue(split, parameter.Value).ToString());

                logservice.WriteToFile(values, Path);
            }
            catch (Exception ex)
            {
                if (TargetService.Config.Targets.ThrowExceptions)
                {
                    throw new Exception("Exception occurred in AbTestMaster when writing to file. Error Message: " + ex.Message);
                }
            }

        }

        internal override void Write(SplitGoal goal)
        {
            try
            {
                var logservice = new LogService();

                var values = Parameters.ToDictionary(parameter => parameter.Key, parameter => ParameterRetriever.RetireveValue(goal, parameter.Value).ToString());

                logservice.WriteToFile(values, Path);
            }
            catch (Exception ex)
            {
                if (TargetService.Config.Targets.ThrowExceptions)
                {
                    throw new Exception("Exception occurred in AbTestMaster when writing to file. Error Message: " + ex.Message);
                }
            }

        }
    }
}
