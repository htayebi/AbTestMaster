using System;
using System.Data.SqlClient;
using AbTestMaster.Domain;
using AbTestMaster.Services;

namespace AbTestMaster.Target
{
    internal class DatabaseTarget : TargetBase
    {
        internal string ConnectionString { get; set; }
        internal string CommandText { get; set; }

        internal override void Write(SplitView split)
        {
            try
            {
                using (var conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();

                    var cmd = new SqlCommand(CommandText, conn);

                    foreach (var parameter in Parameters)
                    {
                        if (CommandText.Contains(parameter.Key))
                        {
                            cmd.Parameters.AddWithValue(parameter.Key, ParameterRetriever.RetireveValue(split, parameter.Value));
                        }
                    }

                    cmd.ExecuteNonQuery();

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                if (TargetService.Config.Targets.ThrowExceptions)
                {
                    throw new Exception("Exception occurred in AbTestMaster when writing to database. Error Message: " + ex.Message);
                }
            }
        }

        internal override void Write(SplitGoal goal)
        {
            try
            {
                using (var conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();

                    var cmd = new SqlCommand(CommandText, conn);

                    foreach (var parameter in Parameters)
                    {
                        if (CommandText.Contains(parameter.Key))
                        {
                            cmd.Parameters.AddWithValue(parameter.Key, ParameterRetriever.RetireveValue(goal, parameter.Value));
                        }
                    }

                    cmd.ExecuteNonQuery();

                    conn.Close();
                }
            }
            catch (Exception ex)
            {
                if (TargetService.Config.Targets.ThrowExceptions)
                {
                    throw new Exception("Exception occurred in AbTestMaster when writing to database. Error Message: " + ex.Message);

                }
            }
        }
    }
}
