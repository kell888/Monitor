using System;
using System.Data.Common;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using Monitor.App_Code;
using System.Diagnostics;

namespace MergeQueryUtil
{
    /// <summary>
    /// 数据库操作类
    /// </summary>
    public class SqlHelper : IDisposable
    {
        private string connStr;
        private string providerName;
        private string connStringConfigName = "MonitoringSystem";
        private DbConnection conn;
        /// <summary>
        /// 数据库连接字符串的配置名称，为空则默认为【MonitoringSystem】
        /// </summary>
        /// <param name="connStringConfigName"></param>
        public SqlHelper(string connStringConfigName = null)
        {
            if (!string.IsNullOrEmpty(connStringConfigName))
            {
                SetConnConfig(connStringConfigName);
            }
            else
            {
                connStr = ConfigurationManager.ConnectionStrings[this.connStringConfigName].ConnectionString;
                providerName = ConfigurationManager.ConnectionStrings[this.connStringConfigName].ProviderName;
            }
        }
        ///// <summary>
        ///// 根据新的连接字符串更改当前的数据库，更改后会使得Conn重新构造！
        ///// </summary>
        ///// <param name="connString"></param>
        ///// <returns></returns>
        //public void ChangeDatabase(string connString)
        //{
        //    conn = null;
        //    connStr = connString;
        //}
        /// <summary>
        /// 当前数据库是否能连接上
        /// </summary>
        public bool CanConnect
        {
            get
            {
                bool isClosed = Conn.State == ConnectionState.Closed;
                try
                {
                    if (isClosed)
                        Conn.Open();
                    return Conn.State == ConnectionState.Open;
                }
                catch (Exception ex)
                {
                    StackTrace st = new StackTrace(true);
                    StackFrame sf = st.GetFrame(0);
                    string fileName = sf.GetFileName();
                    Type type = sf.GetMethod().ReflectedType;
                    string assName = type.Assembly.FullName;
                    string typeName = type.FullName;
                    string methodName = sf.GetMethod().Name;
                    int lineNo = sf.GetFileLineNumber();
                    int colNo = sf.GetFileColumnNumber();
                    Logs.LogError(ErrorLevel.DBError, fileName + " : " + assName + "." + typeName + "." + methodName + "(" + lineNo + "行" + colNo + "列)", ex.Message);
                
                return false;
                }
                finally
                {
                    if (isClosed)
                        Conn.Close();
                }
            }
        }
        /// <summary>
        /// 当前数据库是否已经打开
        /// </summary>
        public bool IsOpened
        {
            get
            {
                return Conn.State == ConnectionState.Open;
            }
        }
        /// <summary>
        /// 设置数据库连接字符串的配置名称，默认为【MonitoringSystem】，如果要赋的值与当前值相同则立即返回
        /// </summary>
        /// <param name="connStringConfigName"></param>
        public void SetConnConfig(string connStringConfigName)
        {
            if (connStringConfigName.Equals(this.connStringConfigName, StringComparison.InvariantCultureIgnoreCase))
                return;

            ConnectionStringConfigName = connStringConfigName;
        }
        /// <summary>
        /// 设置或获取数据库连接字符串的配置名称，设置时会使得Conn重新构造！
        /// </summary>
        public string ConnectionStringConfigName
        {
            get
            {
                if (ConfigurationManager.ConnectionStrings != null && ConfigurationManager.ConnectionStrings.Count > 0)
                {
                    bool find = false;
                    for (int i = 0; i < ConfigurationManager.ConnectionStrings.Count; i++)
                    {
                        if (ConfigurationManager.ConnectionStrings[i].Name.Equals(connStringConfigName, StringComparison.InvariantCultureIgnoreCase))
                        {
                            find = true;
                            break;
                        }
                    }
                    if (!find)
                    {
                        connStringConfigName = ConfigurationManager.ConnectionStrings[0].Name;//找不到时取第一个连接串
                    }
                    return connStringConfigName;
                }
                else
                {
                        StackTrace st = new StackTrace(true);
                        StackFrame sf = st.GetFrame(0);
                        string fileName = sf.GetFileName();
                        Type type = sf.GetMethod().ReflectedType;
                        string assName = type.Assembly.FullName;
                        string typeName = type.FullName;
                        string methodName = sf.GetMethod().Name;
                        int lineNo = sf.GetFileLineNumber();
                        int colNo = sf.GetFileColumnNumber();
                        Logs.LogError(ErrorLevel.DBError, fileName + " : " + assName + "." + typeName + "." + methodName + "(" + lineNo + "行" + colNo + "列)", "应用程序配置中缺少ConnectionStrings节！请配置好再运行程序");
                        Logs.Error("应用程序配置中缺少ConnectionStrings节！请配置好再运行程序");
                    throw new Exception("应用程序配置中缺少ConnectionStrings节！请配置好再运行程序");
                }
            }
            set
            {
                conn = null;
                connStringConfigName = value;
                connStr = ConfigurationManager.ConnectionStrings[connStringConfigName].ConnectionString;
                providerName = ConfigurationManager.ConnectionStrings[connStringConfigName].ProviderName;
            }
        }
        /// <summary>
        /// 获取数据库连接对象
        /// </summary>
        public DbConnection Conn
        {
            get
            {
                if (conn == null)
                {
                    DbProviderFactory factory = DbProviderFactories.GetFactory(providerName);
                    conn = factory.CreateConnection();
                    conn.ConnectionString = connStr;
                    return conn;
                }
                return conn;
            }
        }

        //准备Command
        private void PrepareCommand(DbCommand cmd, string cmdText, CommandType type, DbParameter[] param)
        {
            cmd.CommandText = cmdText;
            cmd.CommandType = type;
            if (param != null && param.Length > 0)
            {
                cmd.Parameters.Clear();
                cmd.Parameters.AddRange(param);
            }
        }

        /// <summary>
        /// 执行查询操作
        /// </summary>
        /// <param name="cmdText">命令文本</param>
        /// <param name="type">命令类型</param>
        /// <param name="param">文本参数数组</param>
        /// <returns></returns>
        public DbDataReader ExecuteQueryReader(string cmdText, CommandType type, DbParameter[] param)
        {
            string logSql = ConfigurationManager.AppSettings["logSql"];
            if (!string.IsNullOrEmpty(logSql) && logSql == "1")
                Logs.Trace("ExecuteQueryReader:" + cmdText);
            DbCommand cmd = new SqlCommand("", Conn as SqlConnection);//Conn.CreateCommand();
            PrepareCommand(cmd, cmdText, type, param);
            if (Conn.State != ConnectionState.Open)
                Conn.Open();
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }

        /// <summary>
        /// 执行查询操作
        /// </summary>
        /// <param name="sql">命令文本</param>
        /// <returns></returns>
        public DbDataReader ExecuteQueryReader(string sql)
        {
            string logSql = ConfigurationManager.AppSettings["logSql"];
            if (!string.IsNullOrEmpty(logSql) && logSql == "1")
                Logs.Trace("执行查询操作ExecuteQueryReader:" + sql);
            DbCommand cmd = new SqlCommand("", Conn as SqlConnection);//Conn.CreateCommand();
            PrepareCommand(cmd, sql, CommandType.Text, null);
            if (Conn.State != ConnectionState.Open)
                Conn.Open();
            return cmd.ExecuteReader(CommandBehavior.CloseConnection);
        }

        /// <summary>
        /// 执行查询操作
        /// </summary>
        /// <param name="cmdText">SQL命令</param>
        /// <param name="type"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public DataTable ExecuteQueryDataTable(string cmdText, CommandType type, DbParameter[] param)
        {
            DataTable dt = new DataTable();
            try
            {
                string logSql = ConfigurationManager.AppSettings["logSql"];
                if (!string.IsNullOrEmpty(logSql) && logSql == "1")
                    Logs.Trace("执行查询操作ExecuteQueryDataTable:" + cmdText);
                DbCommand cmd = Conn.CreateCommand();
                PrepareCommand(cmd, cmdText, type, param);
                if (Conn.State != ConnectionState.Open)
                    Conn.Open();
                DbDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                dt.Load(reader);
                reader.Close();
            }
            catch (Exception ex)
            {
                StackTrace st = new StackTrace(true);
                StackFrame sf = st.GetFrame(0);
                string fileName = sf.GetFileName();
                Type typ = sf.GetMethod().ReflectedType;
                string assName = typ.Assembly.FullName;
                string typeName = typ.FullName;
                string methodName = sf.GetMethod().Name;
                int lineNo = sf.GetFileLineNumber();
                int colNo = sf.GetFileColumnNumber();
                Logs.LogError(ErrorLevel.DBError, fileName + " : " + assName + "." + typeName + "." + methodName + "(" + lineNo + "行" + colNo + "列)", ex.Message);
                Logs.Error("ExecuteQueryDataTable：" + ex.Message);
            }
            finally
            {

            }
            return dt;
        }

        /// <summary>
        /// 执行查询操作
        /// </summary>
        /// <param name="sql">SQL命令</param>
        /// <returns></returns>
        public DataTable ExecuteQueryDataTable(string sql)
        {
            DataTable dt = new DataTable();
            try
            {
                string logSql = ConfigurationManager.AppSettings["logSql"];
                if (!string.IsNullOrEmpty(logSql) && logSql == "1")
                    Logs.Trace("执行查询操作ExecuteQueryDataTable:" + sql);
                DbProviderFactory factory = DbProviderFactories.GetFactory(providerName);
                DbDataAdapter adapter = factory.CreateDataAdapter();
                SqlCommand cmd = new SqlCommand(sql, new SqlConnection(Conn.ConnectionString));
                adapter.SelectCommand = cmd;
                PrepareCommand(cmd, cmd.CommandText, CommandType.Text, null);
                if (Conn.State != ConnectionState.Open)
                    Conn.Open();
                adapter.Fill(dt);
            }
            catch (Exception ex)
                {
                StackTrace st = new StackTrace(true);
                StackFrame sf = st.GetFrame(0);
                string fileName = sf.GetFileName();
                Type type = sf.GetMethod().ReflectedType;
                string assName = type.Assembly.FullName;
                string typeName = type.FullName;
                string methodName = sf.GetMethod().Name;
                int lineNo = sf.GetFileLineNumber();
                int colNo = sf.GetFileColumnNumber();
                Logs.LogError(ErrorLevel.DBError, fileName + " : " + assName + "." + typeName + "." + methodName + "(" + lineNo + "行" + colNo + "列)", ex.Message);
                Logs.Error("ExecuteQueryDataTable：" + ex.Message);
            }
            finally
            {

            }
            return dt;
        }

        /// <summary>
        /// 执行查询操作
        /// </summary>
        /// <param name="sql">SQL命令</param>
        /// <returns></returns>
        public DbDataAdapter GetDataAdapter(string sql)
        {
            try
            {
                string logSql = ConfigurationManager.AppSettings["logSql"];
                if (!string.IsNullOrEmpty(logSql) && logSql == "1")
                    Logs.Trace("执行查询操作GetDataAdapter:" + sql);
                DbProviderFactory factory = DbProviderFactories.GetFactory(providerName);
                DbDataAdapter adapter = factory.CreateDataAdapter();
                SqlCommand cmd = new SqlCommand(sql, new SqlConnection(Conn.ConnectionString));
                adapter.SelectCommand = cmd;
                PrepareCommand(cmd, cmd.CommandText, CommandType.Text, null);
                if (Conn.State != ConnectionState.Open)
                    Conn.Open();
                return adapter;
            }
            catch (Exception ex)
            {
                StackTrace st = new StackTrace(true);
                StackFrame sf = st.GetFrame(0);
                string fileName = sf.GetFileName();
                Type type = sf.GetMethod().ReflectedType;
                string assName = type.Assembly.FullName;
                string typeName = type.FullName;
                string methodName = sf.GetMethod().Name;
                int lineNo = sf.GetFileLineNumber();
                int colNo = sf.GetFileColumnNumber();
                Logs.LogError(ErrorLevel.DBError, fileName + " : " + assName + "." + typeName + "." + methodName + "(" + lineNo + "行" + colNo + "列)", ex.Message);
                Logs.Error("GetDataAdapter：" + ex.Message);
            }
            finally
            {

            }
            return null;
        }

        /// <summary>
        /// 执行查询操作
        /// </summary>
        /// <param name="cmd">命令对象</param>
        /// <param name="type">命令类型</param>
        /// <param name="param">文本参数数组</param>
        /// <returns></returns>
        public DataTable ExecuteQueryDataTable(DbCommand cmd, CommandType type, DbParameter[] param)
        {
            DataTable dt = new DataTable();
            try
            {
                string logSql = ConfigurationManager.AppSettings["logSql"];
                if (!string.IsNullOrEmpty(logSql) && logSql == "1")
                    Logs.Trace("执行查询操作ExecuteQueryDataTable(DbCommand cmd, CommandType type, DbParameter[] param):" + cmd.CommandText);
                DbProviderFactory factory = DbProviderFactories.GetFactory(providerName);
                DbDataAdapter adapter = factory.CreateDataAdapter();
                adapter.SelectCommand = cmd;
                PrepareCommand(cmd, cmd.CommandText, type, param);
                if (Conn.State != ConnectionState.Open)
                    Conn.Open();
                adapter.Fill(dt);
            }
            catch (Exception ex)
            {
                StackTrace st = new StackTrace(true);
                StackFrame sf = st.GetFrame(0);
                string fileName = sf.GetFileName();
                Type typ = sf.GetMethod().ReflectedType;
                string assName = typ.Assembly.FullName;
                string typeName = typ.FullName;
                string methodName = sf.GetMethod().Name;
                int lineNo = sf.GetFileLineNumber();
                int colNo = sf.GetFileColumnNumber();
                Logs.LogError(ErrorLevel.DBError, fileName + " : " + assName + "." + typeName + "." + methodName + "(" + lineNo + "行" + colNo + "列)", ex.Message);
                Logs.Error("ExecuteQueryDataTable：" + ex.Message);
            }
            finally
            {

            }
            return dt;
        }

        /// <summary>
        /// 执行查询操作返回第一行第一列对象
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="type"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public object ExecuteScalar(string cmdText, CommandType type, DbParameter[] param)
        {
            object obj = null;
            string logSql = ConfigurationManager.AppSettings["logSql"];
            if (!string.IsNullOrEmpty(logSql) && logSql == "1")
                Logs.Trace("执行查询操作返回第一行第一列对象ExecuteScalar(string cmdText, CommandType type, DbParameter[] param):" + cmdText);
            using (DbCommand cmd = new SqlCommand("", Conn as SqlConnection))//Conn.CreateCommand())
            {
                try
                {
                    PrepareCommand(cmd, cmdText, type, param);
                    if (Conn.State != ConnectionState.Open)
                        Conn.Open();
                    obj = cmd.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    StackTrace st = new StackTrace(true);
                    StackFrame sf = st.GetFrame(0);
                    string fileName = sf.GetFileName();
                    Type typ = sf.GetMethod().ReflectedType;
                    string assName = typ.Assembly.FullName;
                    string typeName = typ.FullName;
                    string methodName = sf.GetMethod().Name;
                    int lineNo = sf.GetFileLineNumber();
                    int colNo = sf.GetFileColumnNumber();
                    Logs.LogError(ErrorLevel.DBError, fileName + " : " + assName + "." + typeName + "." + methodName + "(" + lineNo + "行" + colNo + "列)", ex.Message);
                    Logs.Error("ExecuteScalar：" + ex.Message);
                }
                finally
                {
                    Conn.Close();
                }
            }
            return obj;
        }

        /// <summary>
        /// 执行查询操作返回第一行第一列对象
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public object ExecuteScalar(string sql)
        {
            object obj = null;
            string logSql = ConfigurationManager.AppSettings["logSql"];
            if (!string.IsNullOrEmpty(logSql) && logSql == "1")
                Logs.Trace("执行查询操作返回第一行第一列对象ExecuteScalar:" + sql);
            using (DbCommand cmd = new SqlCommand("", Conn as SqlConnection))//Conn.CreateCommand())
            {
                try
                {
                    PrepareCommand(cmd, sql, CommandType.Text, null);
                    if (Conn.State != ConnectionState.Open)
                        Conn.Open();
                    obj = cmd.ExecuteScalar();
                }
                catch (Exception ex)
                {
                    StackTrace st = new StackTrace(true);
                    StackFrame sf = st.GetFrame(0);
                    string fileName = sf.GetFileName();
                    Type type = sf.GetMethod().ReflectedType;
                    string assName = type.Assembly.FullName;
                    string typeName = type.FullName;
                    string methodName = sf.GetMethod().Name;
                    int lineNo = sf.GetFileLineNumber();
                    int colNo = sf.GetFileColumnNumber();
                    Logs.LogError(ErrorLevel.DBError, fileName + " : " + assName + "." + typeName + "." + methodName + "(" + lineNo + "行" + colNo + "列)", ex.Message);
                    Logs.Error("ExecuteScalar：" + ex.Message);
                }
                finally
                {
                    Conn.Close();
                }
            }
            return obj;
        }

        /// <summary>
        /// 执行增删改操作，返回受影响行数
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="type"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string cmdText, CommandType type, DbParameter[] param)
        {
            int i = 0;
            string logSql = ConfigurationManager.AppSettings["logSql"];
            if (!string.IsNullOrEmpty(logSql) && logSql == "1")
                Logs.Trace("执行数据库操作ExecuteNonQuery(string cmdText, CommandType type, DbParameter[] param):" + cmdText);
            using (DbCommand cmd = new SqlCommand("", Conn as SqlConnection))//Conn.CreateCommand())
            {
                try
                {
                    PrepareCommand(cmd, cmdText, type, param);
                    if (Conn.State != ConnectionState.Open)
                        Conn.Open();
                    i = cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                }
                catch (Exception ex)
                {
                    StackTrace st = new StackTrace(true);
                    StackFrame sf = st.GetFrame(0);
                    string fileName = sf.GetFileName();
                    Type typ = sf.GetMethod().ReflectedType;
                    string assName = typ.Assembly.FullName;
                    string typeName = typ.FullName;
                    string methodName = sf.GetMethod().Name;
                    int lineNo = sf.GetFileLineNumber();
                    int colNo = sf.GetFileColumnNumber();
                    Logs.LogError(ErrorLevel.DBError, fileName + " : " + assName + "." + typeName + "." + methodName + "(" + lineNo + "行" + colNo + "列)", ex.Message);
                    Logs.Error("ExecuteNonQuery：" + ex.Message);
                }
                finally
                {
                    Conn.Close();
                }
            }
            return i;
        }

        /// <summary>
        /// 执行增删改操作，返回受影响行数
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql)
        {
            int i = 0;
            string logSql = ConfigurationManager.AppSettings["logSql"];
            if (!string.IsNullOrEmpty(logSql) && logSql == "1")
                Logs.Trace("执行数据库操作ExecuteNonQuery:" + sql);
            using (DbCommand cmd = new SqlCommand("", Conn as SqlConnection))//Conn.CreateCommand())
            {
                try
                {
                    PrepareCommand(cmd, sql, CommandType.Text, null);
                    if (Conn.State != ConnectionState.Open)
                        Conn.Open();
                    i = cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    StackTrace st = new StackTrace(true);
                    StackFrame sf = st.GetFrame(0);
                    string fileName = sf.GetFileName();
                    Type type = sf.GetMethod().ReflectedType;
                    string assName = type.Assembly.FullName;
                    string typeName = type.FullName;
                    string methodName = sf.GetMethod().Name;
                    int lineNo = sf.GetFileLineNumber();
                    int colNo = sf.GetFileColumnNumber();
                    byte level = (byte)ErrorLevel.DBError;
                    string source = fileName + " : " + assName + "." + typeName + "." + methodName + "(" + lineNo + "行" + colNo + "列)";
                    string ip = Common.GetIPv4().ToString();
                    string mac = Common.GetMac();
                    string client = ip + "(" + mac + ")";
                    string s = "insert into d_error_log (err_level, err_source, err_msg, err_client) values (" + level + ", '" + source + "', '" + ex.Message + "', '" + client + "')";
                    PrepareCommand(cmd, s, CommandType.Text, null);
                    if (Conn.State != ConnectionState.Open)
                        Conn.Open();
                    int r = cmd.ExecuteNonQuery();
                    Logs.Error("ExecuteNonQuery：" + ex.Message);
                }
                finally
                {
                    Conn.Close();
                }
            }
            return i;
        }
        /*
        /// <summary>
        /// 执行备份还原操作
        /// </summary>
        /// <param name="sql"></param>
        public static void BackupOrRestore(string sql)
        {
            string logSql = ConfigurationManager.AppSettings["logSql"];
            if (!string.IsNullOrEmpty(logSql) && logSql == "1")
                Logs.Trace("执行数据库备份还原操作BackupOrRestore:" + sql);
            using (DbCommand cmd = new SqlCommand("", Conn as SqlConnection))//Conn.CreateCommand())
            {
                try
                {
                    PrepareCommand(cmd, sql, CommandType.Text, null);
                    if (Conn.State != ConnectionState.Open)
                        Conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch  (Exception ex)
                {
                    StackTrace st = new StackTrace(true);
                    StackFrame sf = st.GetFrame(0);
                    string fileName = sf.GetFileName();
                    Type type = sf.GetMethod().ReflectedType;
                    string assName = type.Assembly.FullName;
                    string typeName = type.FullName;
                    string methodName = sf.GetMethod().Name;
                    int lineNo = sf.GetFileLineNumber();
                    int colNo = sf.GetFileColumnNumber();
                    Logs.LogError(ErrorLevel.DBError, fileName + " : " + assName + "." + typeName + "." + methodName + "(" + lineNo + "行" + colNo + "列)", ex.Message);
                    Logs.Error("BackupOrRestore：" + ex.Message); throw ex; }
                finally
                {
                    Conn.Close();
                }
            }
        }
        /// <summary>
        /// 备份数据库
        /// </summary>
        /// <param name="dbName"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static bool Backup(string dbName, string filename)
        {
            string sql = "backup database " + dbName + " to disk='" + filename + "'";
            string logSql = ConfigurationManager.AppSettings["logSql"];
            if (!string.IsNullOrEmpty(logSql) && logSql == "1")
                Logs.Trace("备份数据库Backup:" + sql);
            try
            {
                BackupOrRestore(sql);
                return true;
            }
            catch  (Exception ex)
                {
                    StackTrace st = new StackTrace(true);
                    StackFrame sf = st.GetFrame(0);
                    string fileName = sf.GetFileName();
                    Type type = sf.GetMethod().ReflectedType;
                    string assName = type.Assembly.FullName;
                    string typeName = type.FullName;
                    string methodName = sf.GetMethod().Name;
                    int lineNo = sf.GetFileLineNumber();
                    int colNo = sf.GetFileColumnNumber();
                    Logs.LogError(ErrorLevel.DBError, fileName + " : " + assName + "." + typeName + "." + methodName + "(" + lineNo + "行" + colNo + "列)", ex.Message);
                Logs.Error("Backup：" + ex.Message);
            }
            return false;
        }
        /// <summary>
        /// 恢复数据库
        /// </summary>
        /// <param name="dbName"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static bool Restore(string dbName, string filename)
        {
            string logSql = ConfigurationManager.AppSettings["logSql"];
            string sql = "use master; restore database " + dbName + " from disk='" + filename + "'";
            try
            {
                if (!string.IsNullOrEmpty(logSql) && logSql == "1")
                    Logs.Trace("恢复数据库Restore:" + sql);
                BackupOrRestore(sql);
                return true;
            }
            catch (Exception ex)//数据库在使用中，则先脱机再联机，来实现暂时断开数据库的目的
                {
                    StackTrace st = new StackTrace(true);
                    StackFrame sf = st.GetFrame(0);
                    string fileName = sf.GetFileName();
                    Type type = sf.GetMethod().ReflectedType;
                    string assName = type.Assembly.FullName;
                    string typeName = type.FullName;
                    string methodName = sf.GetMethod().Name;
                    int lineNo = sf.GetFileLineNumber();
                    int colNo = sf.GetFileColumnNumber();
                    Logs.LogError(ErrorLevel.DBError, fileName + " : " + assName + "." + typeName + "." + methodName + "(" + lineNo + "行" + colNo + "列)", ex.Message);
                Logs.Error("Restore：" + ex.Message);
                Logs.Trace("恢复数据库时有异常：" + ex.Message);
                try
                {
                    string sql1 = "use master; alter database " + dbName + " set offline with ROLLBACK IMMEDIATE";//脱机
                    if (!string.IsNullOrEmpty(logSql) && logSql == "1")
                        Logs.Trace("脱机Restore:" + sql1); 
                    ExecuteNonQuery(sql1);
                    string sql2 = "use master; alter database " + dbName + " set online with ROLLBACK IMMEDIATE";//联机 
                    if (!string.IsNullOrEmpty(logSql) && logSql == "1")
                        Logs.Trace("联机Restore:" + sql2);
                    ExecuteNonQuery(sql2);
                    BackupOrRestore(sql);
                    return true;
                }
                catch (Exception e1)
                {
                    Logs.Error("脱机Restore又联机Restore的过车出现错误：" + e1.Message);
                }
            }
            return false;
        }
        */
        /// <summary>
        /// 联机指定数据库
        /// </summary>
        /// <param name="DBName"></param>
        public void OnLine(string DBName)
        {
            string DB = DBName;
            if (string.IsNullOrEmpty(DB))
                DB = Conn.Database;
            string logSql = ConfigurationManager.AppSettings["logSql"];
            if (!string.IsNullOrEmpty(logSql) && logSql == "1")
                Logs.Trace("联机数据库");
            string sql = "alter database " + DB + " set online with ROLLBACK IMMEDIATE";
            ExecuteNonQuery(sql);
        }
        /// <summary>
        /// 脱机指定数据库
        /// </summary>
        /// <param name="DBName"></param>
        public void OffLine(string DBName)
        {
            string DB = DBName;
            if (string.IsNullOrEmpty(DB))
                DB = Conn.Database;
            string logSql = ConfigurationManager.AppSettings["logSql"];
            if (!string.IsNullOrEmpty(logSql) && logSql == "1")
                Logs.Trace("脱机数据库");
            string sql = "alter database " + DB + " set offline with ROLLBACK IMMEDIATE";
            ExecuteNonQuery(sql);
        }
        /// <summary>
        /// 脱机再联机指定数据库
        /// </summary>
        /// <param name="DBName"></param>
        public void OffAndOnLine(string DBName)
        {
            string DB = DBName;
            if (string.IsNullOrEmpty(DB))
                DB = Conn.Database;
            string logSql = ConfigurationManager.AppSettings["logSql"];
            if (!string.IsNullOrEmpty(logSql) && logSql == "1")
                Logs.Trace("脱机再联机数据库");
            string sql = "alter database " + DBName + " set offline with ROLLBACK IMMEDIATE;alter database " + DB + " set online with ROLLBACK IMMEDIATE";
            ExecuteNonQuery(sql);
        }
        /// <summary>
        /// 杀死指定数据库的进程
        /// </summary>
        /// <param name="DbName"></param>
        public void KillProcess(string DbName)
        {
            string DB = DbName;
            if (string.IsNullOrEmpty(DB))
                DB = Conn.Database;
            string sql = "select spid from sys.sysprocesses where dbid=(select dbid from master..sysdatabases where name = '" + DB + "')";
            string logSql = ConfigurationManager.AppSettings["logSql"];
            if (!string.IsNullOrEmpty(logSql) && logSql == "1")
                Logs.Trace("杀死指定数据库的进程：" + sql);
            object obj = ExecuteScalar(sql);
            if (obj != null && obj != DBNull.Value)
            {
                ExecuteNonQuery("kill " + obj.ToString());
            }
        }
        /// <summary>
        /// 收缩数据库
        /// </summary>
        /// <param name="DbName"></param>
        /// <returns></returns>
        public bool ShrinkDB(string DbName)
        {
            string DB = DbName;
            if (string.IsNullOrEmpty(DB))
                DB = Conn.Database;
            try
            {
                string sql = "DBCC SHRINKDATABASE('" + DB + "')";
                string logSql = ConfigurationManager.AppSettings["logSql"];
                if (!string.IsNullOrEmpty(logSql) && logSql == "1")
                    Logs.Trace("收缩数据库:" + sql);
                ExecuteNonQuery(sql);
                return true;
            }
            catch (Exception ex)
            {
                StackTrace st = new StackTrace(true);
                StackFrame sf = st.GetFrame(0);
                string fileName = sf.GetFileName();
                Type type = sf.GetMethod().ReflectedType;
                string assName = type.Assembly.FullName;
                string typeName = type.FullName;
                string methodName = sf.GetMethod().Name;
                int lineNo = sf.GetFileLineNumber();
                int colNo = sf.GetFileColumnNumber();
                Logs.LogError(ErrorLevel.DBError, fileName + " : " + assName + "." + typeName + "." + methodName + "(" + lineNo + "行" + colNo + "列)", ex.Message);
                Logs.Error("ShrinkDB：" + ex.Message);
                return false;
            }
        }

        public void Dispose()
        {
            if (conn != null)
            {
                if (conn.State != ConnectionState.Closed)
                    conn.Close();
                conn.Dispose();
            }
        }
    }
}