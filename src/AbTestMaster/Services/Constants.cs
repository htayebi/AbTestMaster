namespace AbTestMaster.Services
{
    internal class Constants
    {
        internal const string AB_TEST_MASTER_SESSION = "AB_TEST_MASTER_SESSION";
        internal const string AB_TEST_MASTER_COOKIE = "AB_TEST_MASTER_COOKIE";

        internal const string ACTION = "action";
        internal const string CONTROLLER = "controller";
        internal const string AREA = "area";
        internal const string NAMESPACES = "Namespaces";

        internal const string SPLIT_GROUP = "SPLIT_GROUP";
        internal const string SPLIT_NAME = "SPLIT_NAME";
        internal const string SPLIT_SEQUENCE = "SPLIT_SEQUENCE";
        internal const string SPLIT_VIEWS_SEQUENCE_TRAIL = "SPLIT_VIEWS_SEQUENCE_TRAIL";
        internal const string SPLIT_GOAL = "SPLIT_GOAL";

        internal const string DATE_TIME = "DATE_TIME";

        internal const string SPLIT_GROUP_VARIABLE = "$splitgroup";
        internal const string SPLIT_NAME_VARIABLE = "$splitname";
        internal const string SPLIT_VIEWS_SEQUENCE_TRAIL_VARIABLE = "$splitsequencetrail";
        internal const string SPLIT_GOAL_VARIABLE = "$splitgoal";
        internal const string DATE_TIME_VARIABLE = "$currentdatetimeutc";
        internal const string IP_ADDRESS_VARIABLE = "$ipaddress";
        internal const string BROWSER_VARIABLE = "$browser";
        internal const string USER_AGENT_VARIABLE = "$useragent";

        internal const string SPLIT_GOALS_FILE_PATH = @"\App_Data\AB_TEST_MASTER_SplitGoals.csv";
        internal const string SPLIT_VIEWS_FILE_PATH = @"\App_Data\AB_TEST_MASTER_SplitViews.csv";
    }
}
