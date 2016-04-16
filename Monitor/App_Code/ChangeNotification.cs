using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using MergeQueryUtil;

namespace Monitor.App_Code
{
    /// <summary>
    /// 数据库变化通知类
    /// </summary>
    public class ChangeNotification
    {
        bool isStart;
        SqlHelper sqlHelper;
        SqlCommand cmd;
        public bool IsStart
        {
            get { return isStart; }
        }
        public event EventHandler<DependencyArgs> DataChanged;
        string connString;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connString">数据库连接字符串</param>
        public ChangeNotification(string connString)
        {
            if (string.IsNullOrEmpty(connString))
                throw new ArgumentNullException("connString can not be null!");
            sqlHelper = new SqlHelper();
            this.connString = connString;
        }
        /// <summary>
        /// 开启指定数据库的通知服务(启用Broker)，只需要执行一次，一般在Application_Start函数里
        /// </summary>
        /// <returns></returns>
        public bool Start()
        {
            SqlCommand init = new SqlCommand("if DatabasePropertyEx('" + sqlHelper.Conn.Database + "','IsBrokerEnabled')=0 begin ALTER DATABASE " + sqlHelper.Conn.Database + " SET NEW_BROKER WITH ROLLBACK IMMEDIATE; ALTER DATABASE " + sqlHelper.Conn.Database + " SET ENABLE_BROKER; ALTER DATABASE " + sqlHelper.Conn.Database + " SET TRUSTWORTHY ON; GRANT SEND ON SERVICE::[http://schemas.microsoft.com/SQL/Notifications/QueryNotificationService] TO GUEST; Use Master; Exec sp_configure 'clr enabled',1; Reconfigure end", new SqlConnection(sqlHelper.Conn.ConnectionString));
            try
            {
                if (init.Connection.State != ConnectionState.Open)
                    init.Connection.Open();
                int i = init.ExecuteNonQuery();
                isStart = SqlDependency.Start(sqlHelper.Conn.ConnectionString);
                return isStart;
            }
            catch (Exception e) { Logs.Error("ChangeNotification.Start():" + e.Message); return false; }
            finally
            {
                if (init != null)
                {
                    init.Dispose();
                    if (init.Connection.State != ConnectionState.Closed)
                        init.Connection.Close();
                }
            }
        }

        /// <summary>
        /// 停止指定数据库的通知服务(关闭Broker)，只需要执行一次，一般在Application_End函数里
        /// </summary>
        /// <returns></returns>
        public bool Stop()
        {
            bool f = SqlDependency.Stop(sqlHelper.Conn.ConnectionString);
            if (cmd != null)
                cmd.Dispose();
            sqlHelper.Dispose();
            return f;
        }
        /// <summary>
        /// 订阅变化事件
        /// </summary>
        /// <param name="simpleSQL">简单查询字符串，而且要带上类似dbo.的数据库所有者标识</param>
        public SqlCommand ChangeEvent(string simpleSQL)
        {
            cmd = new SqlCommand(simpleSQL, new SqlConnection(connString));
            if (cmd.Connection.State != ConnectionState.Open)
                cmd.Connection.Open();
            SqlDependency sqldpd = new SqlDependency();
            sqldpd.AddCommandDependency(cmd);
            sqldpd.OnChange += new OnChangeEventHandler(sqldpd_OnChange);
            return cmd;
        }

        void sqldpd_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (DataChanged != null)
            {
                DependencyArgs darg = new DependencyArgs(e.Type, e.Info, e.Source);
                if (cmd != null)
                {
                    darg.TriggerResult = ConvertDataReaderToDataSet(cmd.ExecuteReader());
                }
                DataChanged(this, darg);
            }
        }

        /// <summary>
        /// 将指定的数据读取器转化为数据集
        /// </summary>
        /// <param name="reader">数据读取器</param>
        /// <returns></returns>
        public static DataSet ConvertDataReaderToDataSet(DbDataReader reader)
        {
            DataSet dataSet = new DataSet();
            do
            {
                // Create new data table
                DataTable schemaTable = reader.GetSchemaTable();
                DataTable dataTable = new DataTable();
                if (schemaTable != null)
                {
                    // A query returning records was executed
                    for (int i = 0; i < schemaTable.Rows.Count; i++)
                    {
                        DataRow dataRow = schemaTable.Rows[i];
                        // Create a column name that is unique in the data table
                        string columnName = (string)dataRow["ColumnName"]; //+ "<C" + i + "/>";
                        // Add the column definition to the data table
                        DataColumn column = new DataColumn(columnName, (Type)dataRow["DataType"]);
                        dataTable.Columns.Add(column);
                    }
                    dataSet.Tables.Add(dataTable);
                    // Fill the data table we just created
                    while (reader.Read())
                    {
                        DataRow dataRow = dataTable.NewRow();
                        for (int i = 0; i < reader.FieldCount; i++)
                            dataRow[i] = reader.GetValue(i);
                        dataTable.Rows.Add(dataRow);
                    }
                }
                else
                {
                    // No records were returned
                    DataColumn column = new DataColumn("RowsAffected");
                    dataTable.Columns.Add(column);
                    dataSet.Tables.Add(dataTable);
                    DataRow dataRow = dataTable.NewRow();
                    dataRow[0] = reader.RecordsAffected;
                    dataTable.Rows.Add(dataRow);
                }
            }
            while (reader.NextResult());
            reader.Close();
            return dataSet;
        }
    }

    /// <summary>
    /// 数据库变化依赖参数类
    /// </summary>
    public class DependencyArgs : SqlNotificationEventArgs
    {
        DataSet triggerResult;

        /// <summary>
        /// 触发结果
        /// </summary>
        public DataSet TriggerResult
        {
            get { return triggerResult; }
            internal set { triggerResult = value; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="type"></param>
        /// <param name="info"></param>
        /// <param name="source"></param>
        public DependencyArgs(SqlNotificationType type, SqlNotificationInfo info, SqlNotificationSource source)
            : base(type, info, source)
        {

        }
    }
}