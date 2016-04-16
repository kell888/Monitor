using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace Monitor.App_Code
{
    /// <summary>
    /// 新数据产生时的处理委托
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void NewDataHandler(NewDataNotification sender, NewDataArgs e);

    /// <summary>
    /// 新数据参数
    /// </summary>
    public class NewDataArgs
    {
        string database;
        /// <summary>
        /// 监视的数据库
        /// </summary>
        public string Database
        {
            get { return database; }
        }
        string table;
        /// <summary>
        /// 监视的表名
        /// </summary>
        public string Table
        {
            get { return table; }
        }
        string primaryKey;
        /// <summary>
        /// 监视表的主键名
        /// </summary>
        public string PrimaryKey
        {
            get { return primaryKey; }
        }
        decimal lastId;
        /// <summary>
        /// 上一个ID
        /// </summary>
        public decimal LastId
        {
            get { return lastId; }
        }
        List<decimal> newIds;
        /// <summary>
        /// 新数据的ID
        /// </summary>
        public List<decimal> NewIds
        {
            get { return newIds; }
        }
        DateTime newTime;
        /// <summary>
        /// 新产生数据的时刻
        /// </summary>
        public DateTime NewTime
        {
            get { return newTime; }
            set { newTime = value; }
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="database"></param>
        /// <param name="table"></param>
        /// <param name="primaryKey"></param>
        /// <param name="lastId"></param>
        /// <param name="newIds"></param>
        public NewDataArgs(string database, string table, string primaryKey, decimal lastId, List<decimal> newIds)
        {
            this.database = database;
            this.table = table;
            this.primaryKey = primaryKey;
            this.lastId = lastId;
            this.newIds = newIds;
            this.newTime = DateTime.Now;
        }
    }
    /// <summary>
    /// 有新数据产生时的通知类(注意：要监控的表必须要有自增长的主键)
    /// </summary>
    public class NewDataNotification
    {
        Timer timer;
        TinySqlHelper sql;
        static readonly object syncObject = new object();
        /// <summary>
        /// 有新数据产生时触发的事件
        /// </summary>
        public event NewDataHandler NewDataComing;
        string database;
        volatile bool getFirstMaxID;
        /// <summary>
        /// 当前监视的数据库
        /// </summary>
        public string Database
        {
            get { return database; }
        }
        string monitorTB;
        /// <summary>
        /// 监视的通表名
        /// </summary>
        public string MonitorTB
        {
            get { return monitorTB; }
            set { monitorTB = value; }
        }
        string tablename;
        /// <summary>
        /// 当前监视的表名
        /// </summary>
        public string Tablename
        {
            get { return tablename; }
        }
        string primaryKey;
        /// <summary>
        /// 主键
        /// </summary>
        public string PrimaryKey
        {
            get { return primaryKey; }
            set { primaryKey = value; }
        }
        string where;
        /// <summary>
        /// 监视条件(简单的where语句，即不能带order by或者group by等等后缀)
        /// </summary>
        public string Condition
        {
            get { return where; }
            set { where = value; }
        }
        decimal lastId;
        /// <summary>
        /// 触发之前的上一个表标识的值
        /// </summary>
        public decimal LastId
        {
            get { return lastId; }
        }
        /// <summary>
        /// 是否已经开始监视数据库
        /// </summary>
        public bool IsStart
        {
            get
            {
                return timer.Enabled;
            }
        }
        /// <summary>
        /// 轮询数据库指定的数据表，监测是否有满足条件的新纪录产生
        /// </summary>
        /// <param name="interval">单位：毫秒</param>
        /// <param name="connString">数据库连接字符串</param>
        /// <param name="monitorTB">数据通表名</param>
        /// <param name="primaryKey">主键</param>
        /// <param name="where">要满足的条件</param>
        public NewDataNotification(int interval, string connString, string monitorTB, string primaryKey, string where)
        {
            getFirstMaxID = false;
            sql = new TinySqlHelper(connString);
            timer = new Timer();
            timer.Interval = interval;
            timer.Tick += new EventHandler(timer_Tick);
            SqlConnectionStringBuilder connBuilder = new SqlConnectionStringBuilder(connString);
            this.database = connBuilder.InitialCatalog;
            this.monitorTB = monitorTB;
            this.primaryKey = primaryKey;
            this.where = where;
            object obj = sql.GetObject("select IDENT_CURRENT('" + tablename + "')");
            if (obj != null && obj != DBNull.Value)
            {
                try
                {
                    lastId = Convert.ToDecimal(obj);
                }
                catch { }
            }
        }
        /// <summary>
        /// 根据通表名获取当前的实际表名
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        public static string GetTableName(string table)
        {
            DateTime now = DateTime.Now;
            string TB = table;
            string Tail = "";//默认为不带时间后缀的表
            if (table.ToUpper().EndsWith("YYYYMM"))
            {
                TB = table.Substring(0, table.Length - 6);
                Tail = now.Year.ToString().PadLeft(4, '0') + now.Month.ToString().PadLeft(2, '0');
            }
            else if (table.ToUpper().EndsWith("YYYYMMDD"))
            {
                TB = table.Substring(0, table.Length - 8);
                Tail = now.Year.ToString().PadLeft(4, '0') + now.Month.ToString().PadLeft(2, '0') + now.Day.ToString().PadLeft(2, '0');
            }
            return TB + Tail;
        }
        void timer_Tick(object sender, EventArgs e)
        {
            lock (syncObject)
            {
                if (NewDataComing != null && GetFirstMaxID())
                {
                    timer.Stop();
                    DataTable dt = sql.GetTable("select " + primaryKey + " from " + tablename + " where " + primaryKey + ">" + lastId + " order by " + primaryKey + " asc");
                    if (dt.Rows.Count > 0)
                    {
                        List<decimal> newIds = new List<decimal>();
                        decimal newId = 0;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            newId = Convert.ToDecimal(dt.Rows[i][0]);
                            newIds.Add(newId);
                        }
                        if (newId != lastId)
                        {
                            string w = " where (1=1)";
                            if (!string.IsNullOrEmpty(where))
                            {
                                if (where.Trim().ToLower().StartsWith("where "))
                                {
                                    w = " " + where;
                                }
                                else
                                {
                                    if (where.Trim().ToLower().StartsWith("and "))
                                        w = " where (1=1) " + where;
                                    else
                                        w = " where " + where;
                                }
                            }
                            List<string> ids = new List<string>();
                            newIds.ForEach(a => ids.Add(a.ToString()));
                            SqlDataReader reader = sql.GetDataReader("select " + primaryKey + " from " + tablename + w + " and " + primaryKey + " in (" + string.Join(",", ids.ToArray()) + ") order by " + primaryKey + " asc", CommandBehavior.CloseConnection);
                            decimal nId = 0;
                            List<decimal> nids = new List<decimal>();
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    nId = reader.GetDecimal(0);
                                    nids.Add(nId);
                                }
                            }
                            reader.Close();
                            if (nids.Count > 0)
                            {
                                NewDataArgs arg = new NewDataArgs(database, tablename, primaryKey, lastId, nids);
                                NewDataComing(this, arg);
                                lastId = nId;
                            }
                        }
                    }
                    timer.Start();
                }
            }
        }
        private bool GetFirstMaxID()
        {
            string newName = GetTableName(monitorTB);
            if (newName.Equals(this.tablename, StringComparison.InvariantCultureIgnoreCase))
            {
                if (getFirstMaxID)
                    return true;
            }
            else
            {
                this.tablename = newName;
                getFirstMaxID = false;
                lastId = 0;
                if (sql.ExistsTable(tablename))
                {
                    object obj = sql.GetObject("select top 1 " + primaryKey + " from " + tablename + " order by " + primaryKey + " DESC");
                    if (obj != null && obj != DBNull.Value)
                    {
                        lastId = Convert.ToDecimal(obj);
                        getFirstMaxID = true;
                    }
                }
                return true;
            }
            return false;
        }
        /// <summary>
        /// 开始监视
        /// </summary>
        public void Start()
        {
            string newName = GetTableName(monitorTB);
            if (!newName.Equals(this.tablename, StringComparison.InvariantCultureIgnoreCase))
            {
                this.tablename = newName;
                getFirstMaxID = false;
                lastId = 0;
            }
            if (sql.ExistsTable(tablename))
            {
                object obj = sql.GetObject("select top 1 " + primaryKey + " from " + tablename + " order by " + primaryKey + " DESC");
                if (obj != null && obj != DBNull.Value)
                {
                    lastId = Convert.ToDecimal(obj);
                    getFirstMaxID = true;
                }
            }
            if (NewDataComing != null && !timer.Enabled)
                timer.Start();
        }
        /// <summary>
        /// 停止监视
        /// </summary>
        public void Stop()
        {
            if (NewDataComing != null && timer.Enabled)
            {
                timer.Stop();
                object obj = sql.GetObject("select IDENT_CURRENT('" + tablename + "')");
                if (obj != null && obj != DBNull.Value)
                {
                    try
                    {
                        lastId = Convert.ToDecimal(obj);
                    }
                    catch { }
                }
            }
        }
    }

    /// <summary>
    /// 微型数据库操作类
    /// </summary>
    public class TinySqlHelper
    {
        static readonly object syncObj = new object();
        SqlConnection conn;

        /// <summary>
        /// 当前连接对象
        /// </summary>
        public SqlConnection Connection
        {
            get { return conn; }
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connString"></param>
        public TinySqlHelper(string connString)
        {
            conn = new SqlConnection(connString);
        }
        /// <summary>
        /// 当前数据库是否可连接
        /// </summary>
        public bool IsConnectable
        {
            get
            {
                lock (syncObj)
                {
                    bool opened = conn.State == ConnectionState.Open;
                    if (opened)
                    {
                        return true;
                    }
                    else
                    {
                        try
                        {
                            conn.Open();
                            return true;
                        }
                        catch
                        {
                            return false;
                        }
                        finally
                        {
                            conn.Close();
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 获取指定的对象
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public object GetObject(string sql)
        {
            object obj = null;
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();
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
                Logs.LogError(ErrorLevel.DBError, fileName + " : " + assName + "." + typeName + "." + methodName + "(" + lineNo + "行"+ colNo + "列)", ex.Message);
            }
            finally
            {
                if (cmd.Connection.State == ConnectionState.Open)
                    cmd.Connection.Close();
            }
            return obj;
        }
        /// <summary>
        /// 获取指定的数据列表
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable GetTable(string sql)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter adp = new SqlDataAdapter(sql, conn);
            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                adp.Fill(dt);

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
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
            return dt;
        }
        /// <summary>
        /// 获取指定的数据读取器
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="cmdBehavior"></param>
        /// <returns></returns>
        public SqlDataReader GetDataReader(string sql, CommandBehavior cmdBehavior)
        {
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();
                return cmd.ExecuteReader(cmdBehavior);
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
            }
            return null;
        }
        /// <summary>
        /// 执行指定的命令
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int Execute(string sql)
        {
            SqlCommand cmd = new SqlCommand(sql, conn);
            try
            {
                if (cmd.Connection.State != ConnectionState.Open)
                    cmd.Connection.Open();
                return cmd.ExecuteNonQuery();
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
            }
            finally
            {
                if (cmd.Connection.State == ConnectionState.Open)
                    cmd.Connection.Close();
            }
            return -1;
        }
        /// <summary>
        /// 判断指定的数据库对象是否存在
        /// </summary>
        /// <param name="tablename"></param>
        /// <returns></returns>
        public bool ExistsTable(string tablename)
        {
            string sql = "select 1 from sysobjects where id=object_id('" + tablename + "')";// and objectproperty(id, N'IsUserTable')=1";
            DataTable dt = GetTable(sql);
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            return false;
        }
    }
}
