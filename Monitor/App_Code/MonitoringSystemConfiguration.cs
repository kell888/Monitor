using System.Configuration;

namespace Monitor.App_Code
{
    /// <summary>
    /// MonitoringSystemConfiguration 的摘要说明
    /// </summary>
    public static class MonitoringSystemConfiguration
    {
        //public MonitoringSystemConfiguration()
        //{
        //    //
        //    // TODO: 在此处添加构造函数逻辑
        //    //
        //}
        private static string dbConnectionString;
        private static string dbProviderName;
        static MonitoringSystemConfiguration()
        {
            dbConnectionString = ConfigurationManager.ConnectionStrings["MonitoringSystem"].ConnectionString;
            dbProviderName = ConfigurationManager.ConnectionStrings["MonitoringSystem"].ProviderName;
        }
        public static string DbConnectionString
        {
            get
            {
                return dbConnectionString;
            }
        }

        public static string DbProviderName
        {
            get
            {
                return dbProviderName;
            }
        }
    }
}