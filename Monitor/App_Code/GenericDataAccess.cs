using System.Data;
using System.Data.Common;

namespace Monitor.App_Code
{
    /// <summary>
    /// GenericDataAccess 的摘要说明
    /// </summary>
    public static class GenericDataAccess
    {
        //public GenericDataAccess()
        //{
        //    //
        //    // TODO: 在此处添加构造函数逻辑
        //    //
        //}
        public static DataTable ExecuteSelectCommand(DbCommand command)
        {
            // The DataTable to be returned 
            DataTable table;
            // Execute the command making sure the connection gets closed in the end
            // Open the data connection 
            command.Connection.Open();
            // Execute the command and save the results in a DataTable
            DbDataReader reader = command.ExecuteReader();
            table = new DataTable();
            table.Load(reader);
            // Close the reader 
            reader.Close();
            return table;
        }

        public static DbCommand CreateCommand()
        {
            // Obtain the database provider name
            string dataProviderName = MonitoringSystemConfiguration.DbProviderName;
            // Obtain the database connection string
            string connectionString = MonitoringSystemConfiguration.DbConnectionString;
            // Create a new data provider factory
            DbProviderFactory factory = DbProviderFactories.GetFactory(dataProviderName);
            // Obtain a database specific connection object
            DbConnection conn = factory.CreateConnection();
            // Set the connection string
            conn.ConnectionString = connectionString;
            // Create a database specific command object
            DbCommand comm = conn.CreateCommand();
            // Set the command type to stored procedure
            comm.CommandType = CommandType.StoredProcedure;
            // Return the initialized command object
            return comm;
        }

    }
}