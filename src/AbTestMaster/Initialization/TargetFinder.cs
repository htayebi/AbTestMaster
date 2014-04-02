using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using AbTestMaster.Configuration;
using AbTestMaster.Services;
using AbTestMaster.Target;

namespace AbTestMaster.Initialization
{
    internal class TargetFinder
    {
        internal static List<TargetBase> FindTargets()
        {
            var targetsList = new List<TargetBase>();
            var config = TargetService.Config;

            foreach (var objTarget in config.Targets)
            {
                var targetElement = (TargetElement)objTarget;

                if (targetElement.Type.ToLower() == "file")
                {
                    targetsList.Add(CreateFileTarget(targetElement));
                }
                else if (targetElement.Type.ToLower() == "database")
                {
                    targetsList.Add(CreateDatabaseTarget(targetElement));
                }
                else
                {
                    //throw an AbTestMasterException --> create this
                    throw new Exception("Unknown target type " + targetElement.Type);
                }
            }

            return targetsList;
        }

        internal static AbTestMasterSection FindConfig()
        {
            var config = ConfigurationManager.GetSection("AbTestMaster") as AbTestMasterSection ?? new AbTestMasterSection();

            return config;
        }


        private static TargetBase CreateDatabaseTarget(TargetElement targetElement)
        {
            var target = new DatabaseTarget
            {
                CommandText = targetElement.CommandText,
                ConnectionString = ConfigurationManager.ConnectionStrings[targetElement.ConnectionStringName].ConnectionString,
                DataType = GetTargetDataType(targetElement.Data),
                Name = targetElement.Name,
                Type = targetElement.Type,
                Parameters =
                    targetElement.Parameters.Cast<ParameterElement>()
                        .ToDictionary(param => param.Name, param => param.Value)
            };

            return target;
        }

        private static TargetBase CreateFileTarget(TargetElement targetElement)
        {
            var target = new FileTarget
            {
                Path = targetElement.Path,
                DataType = GetTargetDataType(targetElement.Data),
                Name = targetElement.Name,
                Type = targetElement.Type,
                Parameters =
                    targetElement.Parameters.Cast<ParameterElement>()
                        .ToDictionary(param => param.Name, param => param.Value)
            };

            return target;
        }

        private static TargetDataType GetTargetDataType(string input)
        {
            var datatype = TargetDataType.Unknown;

            switch (input.ToLower())
            {
                case "views":
                    datatype = TargetDataType.Views;
                    break;
                case "goals":
                    datatype = TargetDataType.Goals;
                    break;
            }
            return datatype;
        }
    }
}
