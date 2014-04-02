using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using AbTestMaster.Domain;

namespace AbTestMaster.Services
{
    internal class LogService
    {
        #region internal methods
        internal void WriteToFile(SplitView split)
        {
            var keyValuePairs = new Dictionary<string, string>
            {
                {Constants.DATE_TIME, DateTime.UtcNow.ToString()},
                {Constants.SPLIT_GROUP, split.SplitGroup},
                {Constants.SPLIT_NAME, split.SplitViewName},
                {Constants.SPLIT_GOAL, split.Goal}
            };

            CheckForFile(Constants.SPLIT_VIEWS_FILE_PATH, new List<string> { Constants.DATE_TIME, Constants.SPLIT_GROUP, Constants.SPLIT_NAME, Constants.SPLIT_GOAL });

            WriteToFile(keyValuePairs, Constants.SPLIT_VIEWS_FILE_PATH);
        }

        internal void WriteToFile(SplitGoal goal)
        {
            var keyValuePairs = new Dictionary<string, string>
            {
                {Constants.DATE_TIME, DateTime.UtcNow.ToString()},
                {Constants.SPLIT_GOAL, goal.Goal},
                {Constants.SPLIT_VIEWS_SEQUENCE_TRAIL, HttpHelpers.GetViewTrail(goal.Goal)}
            };

            CheckForFile(Constants.SPLIT_GOALS_FILE_PATH, new List<string> { Constants.DATE_TIME, Constants.SPLIT_GOAL, Constants.SPLIT_VIEWS_SEQUENCE_TRAIL });

            WriteToFile(keyValuePairs, Constants.SPLIT_GOALS_FILE_PATH);
        }

        internal void WriteToFile(Dictionary<string, string> keyValuePairs, string path)
        {
            var logFilePath = @HttpContext.Current.Server.MapPath("~") + path;
            var logFile = new StreamWriter(logFilePath, true);
            foreach (var keyValuePair in keyValuePairs)
            {
                logFile.Write(keyValuePair.Value + ",");
            }

            logFile.WriteLine();
            logFile.Close();
        }
        #endregion

        #region private methods
        private void CheckForFile(string filePath, IEnumerable<string> columns)
        {
            string fullPath = @HttpContext.Current.Server.MapPath("~") + filePath;

            if (File.Exists(fullPath))
            {
                return;
            }

            using (var writer = new StreamWriter(fullPath, true))
            {
                foreach (string column in columns)
                {
                    writer.Write(column);
                    writer.Write(",");
                }
                writer.WriteLine();
            }
        }
        #endregion
    }
}
