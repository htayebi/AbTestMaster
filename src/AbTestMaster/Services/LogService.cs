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
                {Constants.SPLIT_GROUP, split.SplitGroup},
                {Constants.SPLIT_NAME, split.SplitViewName},
                {Constants.SPLIT_SEQUENCE, split.Sequence}
            };

            CheckForFile(Constants.SPLIT_VIEWS_FILE_PATH, new List<string> { Constants.DATE_TIME, Constants.SPLIT_GROUP, Constants.SPLIT_NAME, Constants.SPLIT_SEQUENCE });

            WriteToFile(keyValuePairs, Constants.SPLIT_VIEWS_FILE_PATH);
        }

        internal void WriteToFile(SplitGoal goal)
        {
            var keyValuePairs = new Dictionary<string, string>
            {
                {Constants.SPLIT_SEQUENCE, goal.Sequence},
                {Constants.SPLIT_VIEWS_SEQUENCE_TRAIL, HttpHelpers.GetViewTrail(goal.Sequence)}
            };

            CheckForFile(Constants.SPLIT_GOALS_FILE_PATH, new List<string> { Constants.DATE_TIME, Constants.SPLIT_SEQUENCE, Constants.SPLIT_VIEWS_SEQUENCE_TRAIL });

            WriteToFile(keyValuePairs, Constants.SPLIT_GOALS_FILE_PATH);
        }

        internal void WriteToFile(Dictionary<string, string> keyValuePairs, string path)
        {
            try
            {
                var logFilePath = @HttpContext.Current.Server.MapPath("~") + path;
                var logFile = new StreamWriter(logFilePath, true);
                logFile.Write(DateTime.Now.ToLocalTime());
                foreach (var keyValuePair in keyValuePairs)
                {
                    logFile.Write("," + keyValuePair.Value);
                }

                logFile.WriteLine();
                logFile.Close();
            }
            catch (Exception)
            { }
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
