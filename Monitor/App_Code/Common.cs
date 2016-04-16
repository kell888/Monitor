using System;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Data.Common;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using MergeQueryUtil;
using System.Windows.Forms;
using System.Reflection;
using System.Net;
using System.Configuration;
using System.Collections.Generic;
using KellControls;
using System.Management;

namespace Monitor.App_Code
{
    /// <summary>
    ///Common 的摘要说明
    /// </summary>
    public static class Common
    {
        public static string AdminId
        {
            get
            {
                return ConfigurationManager.AppSettings["AdminId"];
            }
        }

        public static int GetIndexById(this DataTable dt, string colName, string id)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i][colName].ToString() == id)
                {
                    return i;
                }
            }
            return -1;
        }

        public static int GetIndexByText(this KellComboBox combo, string text)
        {
            for (int i = 0; i < combo.Items.Count; i++)
            {
                if (combo.GetItemText(i) == text)
                {
                    return i;
                }
            }
            return -1;
        }

        public static IPAddress GetIPv4()
        {
            IPAddress[] ips = Dns.GetHostAddresses("");
            foreach (IPAddress ip in ips)
            {
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    return ip;
            }
            return IPAddress.Loopback;
        }

        public static string GetMac()
        {
            //using (ManagementObjectSearcher nisc = new ManagementObjectSearcher("select IPEnabled,MACAddress from Win32_NetworkAdapterConfiguration"))
            //{
            //    foreach (ManagementObject nic in nisc.Get())
            //    {
            //        if (Convert.ToBoolean(nic["IPEnabled"]))
            //        {
            //            return nic["MACAddress"].ToString();
            //        }
            //    }
            //}
            using (ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration"))
            {
                using (ManagementObjectCollection moc = mc.GetInstances())
                {
                    foreach (ManagementObject mo in moc)
                    {
                        if (mo["IPEnabled"].ToString() == "True")
                            return mo["MacAddress"].ToString();
                    }
                }
            }
            return "";
        }
        /// <summary>
        /// 获取当前的数据库服务器名(或者IP)
        /// </summary>
        /// <returns></returns>
        public static string GetServer()
        {
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                return sqlHelper.Conn.DataSource;
            }
        }
        /// <summary>
        /// 根据设备类型获取设备地址的前缀
        /// </summary>
        /// <param name="device_type_id"></param>
        /// <returns></returns>
        public static string GetAddressPrefix(int device_type_id)
        {
            string prefix = "";
            switch (device_type_id)
            {
                case 101:
                    prefix = "pantograph_";
                    break;
                case 102:
                    prefix = "train_";
                    break;
                case 103:
                    prefix = "plc_";
                    break;
                case 104:
                    prefix = "alnico_";
                    break;
                case 105:
                    prefix = "camera_";
                    break;
                case 106:
                    prefix = "vidicon_";
                    break;
                case 107:
                    prefix = "laser_";
                    break;
                case 108:
                    prefix = "stress_";
                    break;
            }
            return prefix;
        }
        /// <summary>
        /// 根据列车ID获取对应的编组数
        /// </summary>
        /// <param name="trainId">列车ID</param>
        /// <returns>编组数</returns>
        public static int GetCountByTrainId(int trainId)
        {
            string sql = "select [count] from m_train where object_id=" + trainId;
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                object obj = sqlHelper.ExecuteScalar(sql);
                if (obj != null && obj != DBNull.Value)
                {
                    int RET;
                    if (int.TryParse(obj.ToString(), out RET))
                    {
                        return RET;
                    }
                }
                return 0;
            }
        }
        /// <summary>
        /// 设备表中是否已经存在指定对象ID的设备
        /// </summary>
        /// <param name="object_id"></param>
        /// <returns></returns>
        public static bool HasDevice(int object_id)
        {
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                string sql = "select count(*) from m_device where object_id=" + object_id;
                object obj = sqlHelper.ExecuteScalar(sql);
                if (obj != null && obj != DBNull.Value)
                    return Convert.ToInt32(obj) > 0;
                return false;
            }
        }
        /// <summary>
        /// 获取上线或下线的状态（如果原来记录中没有则返回-1，如存在记录，且最后一个记录跟现在检测的状态一致则返回2(不必记录)，否则就返回上次的状态，如：上线0(则记录本次下线)或者下线1(则记录本次上线)）
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="off">现在检测的状态(是否断网)</param>
        /// <returns></returns>
        public static int LastOnOff(string ip, bool off)
        {
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                object obj = sqlHelper.ExecuteScalar("select top 1 onOff from d_ping_log where ip='" + ip + "' order by pingtime desc");
                if (obj != null && obj != DBNull.Value)
                {
                    bool of = Convert.ToBoolean(obj);
                    if (of == off)
                        return 2;
                    else
                        return (of ? 1 : 0);
                }
                return -1;
            }
        }
        public static string GetStandard(string point_type_id)
        {
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                string sql = "select mutation, normal_low_alarm, normal_high_alarm from s_rm_type where point_type_id=" + point_type_id;
                DataTable dt = sqlHelper.ExecuteQueryDataTable(sql);
                if (dt.Rows.Count > 0)
                    return "突变值=" + dt.Rows[0][0].ToString() + ",上下限范围[" + dt.Rows[0][1].ToString() + "," + dt.Rows[0][2].ToString() + "]";
                return "";
            }
        }

        public static string GetStandard(string object_name, string alarm_status, string object_id, string point_type_id)
        {
            string pt = GetPointPictureId(Convert.ToInt32(object_id));
            string s = "前";
            if (pt == "B_")
                s = "后";
            if (object_name == s + "滑板磨耗")
            {
                return GetSX(point_type_id);
            }
            else if (object_name == s + "滑板缺口")
            {
                return GetSX(point_type_id);
            }
            else if (object_name == "中心偏移平均值")
            {
                return GetSX(point_type_id);
            }
            else if (object_name == "上下倾斜平均值")
            {
                return GetSX(point_type_id);
            }
            else if (object_name == "前后倾斜量")
            {
                return GetSX(point_type_id);
            }
            else
            {
                if (alarm_status == "1")
                {
                    return GetTB(point_type_id);
                }
                else
                {
                    return GetXX(point_type_id) + "～" + GetSX(point_type_id);
                }
            }
        }

        private static string GetSX(string ptid)
        {
            string sx = "0";
            DataSet Ds = new DataSet();
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                object o = sqlHelper.ExecuteScalar("Select max_value from s_argument where isEnable=1 and point_type_id=" + ptid);//报警值表
                if (o != null && o != DBNull.Value)
                    sx = o.ToString();//上限
            }
            return sx;
        }

        private static string GetXX(string ptid)
        {
            string xx = "0";
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                object o = sqlHelper.ExecuteScalar("Select min_value from s_argument where isEnable=1 and point_type_id=" + ptid);//报警值表                
                if (o != null && o != DBNull.Value)
                    xx = o.ToString();//下限
            }
            return xx;
        }

        private static string GetTB(string ptid)
        {
            string tb = "0";
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                object o = sqlHelper.ExecuteScalar("Select standard_value from s_argument where isEnable=1 and point_type_id=" + ptid);//报警值表
                if (o != null && o != DBNull.Value)
                    tb = o.ToString();//突变
            }
            return tb;
        }

        /// <summary>
        /// 可以匹配yyyyMM,yyyyMMdd,yyyyMMddHH,yyyyMMddHHmm,yyyyMMddHHmmss,yyyyMMddHHmmssfff等时间格式
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetPicPath(string path)
        {
            string p = path;
            //根据无连接符号的时间格式来找
            Match mat = Regex.Match(p, @"(\d{1,4}[0-1]\d[0-3]\d[0-2]\d[0-5]\d[0-5]\d\d{3})|(\d{1,4}[0-1]\d[0-3]\d[0-2]\d[0-5]\d[0-5]\d)|(\d{1,4}[0-1]\d[0-3]\d[0-2]\d[0-5]\d)|(\d{1,4}[0-1]\d[0-3]\d[0-2]\d)|(\d{1,4}[0-1]\d[0-3]\d)");
            if (mat.Success)
            {
                p = mat.Value;
            }
            //int index = path.LastIndexOf(@"\");
            //if (index > -1)//a\sc\c=4
            //{
            //    p = path.Substring(0, index);//a\sc=1
            //    int i = p.LastIndexOf(@"\");
            //    if (i > -1)
            //        p = p.Substring(i + 1);//sc
            //}
            return p;
        }

        public static string GetMohaoOrQuekou(int objectid)
        {
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                string sql = "select object_name from m_object where object_id=" + objectid;
                DataTable dt = sqlHelper.ExecuteQueryDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    string objectname = dt.Rows[0][0].ToString();
                    if (objectname.Contains("磨耗"))
                    {
                        return "磨耗";
                    }
                    else
                    {
                        return "缺口";
                    }
                }
                return "磨耗";
            }
        }

        public static string GetLine(int station_id)
        {
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                string sql = "select line_no from m_station where id = " + station_id;
                DataTable dt = sqlHelper.ExecuteQueryDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    return dt.Rows[0][0].ToString();
                }
                return "";
            }
        }

        public static string GetStation(int station_id)
        {
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                string sql = "select station_name from m_station where id = " + station_id;
                DataTable dt = sqlHelper.ExecuteQueryDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    return dt.Rows[0][0].ToString();
                }
                return "";
            }
        }

        public static string GetAddress(int station_id)
        {
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                string sql = "select line_no,station_name from m_station where id = " + station_id;
                DataTable dt = sqlHelper.ExecuteQueryDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    return dt.Rows[0][0].ToString() + " " + dt.Rows[0][1].ToString();
                }
                return "";
            }
        }

        public static string GetFrontOrBack(int device_id)
        {
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                string sql = "select device_name from m_device where device_id = " + device_id;
                DataTable dt = sqlHelper.ExecuteQueryDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    return dt.Rows[0][0].ToString();
                }
                return "";
            }
        }

        public static string GetPointPictureId(int device_id)
        {
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                string sql = "select device_name from m_device where device_id = " + device_id;
                DataTable dt = sqlHelper.ExecuteQueryDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    string objectname = dt.Rows[0][0].ToString();
                    if (objectname.Contains("球铰高度"))
                    {
                        return "1";
                    }
                    else if (objectname.Contains("前滑板"))
                    {
                        return "A_";
                    }
                    else if (objectname.Contains("后滑板"))
                    {
                        return "B_";
                    }
                }
                return "1";
            }
        }

        public static DataTable GetAlarmType(int alarm_type_id)
        {
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                string sql = "select * from s_alarm_type where alarm_type_id=" + alarm_type_id;
                DataTable dt = sqlHelper.ExecuteQueryDataTable(sql);
                return dt;
            }
        }

        public static DataTable GetStatusType(int status_type_id)
        {
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                string sql = "select * from s_status_type where status_type_id=" + status_type_id;
                DataTable dt = sqlHelper.ExecuteQueryDataTable(sql);
                return dt;
            }
        }

        public static DataTable GetPointType(int point_type_id)
        {
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                string sql = "select * from s_point_type where point_type_id=" + point_type_id;
                DataTable dt = sqlHelper.ExecuteQueryDataTable(sql);
                return dt;
            }
        }

        public static DataTable GetDeviceType(int device_type_id)
        {
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                string sql = "select * from s_device_type where device_type_id=" + device_type_id;
                DataTable dt = sqlHelper.ExecuteQueryDataTable(sql);
                return dt;
            }
        }

        public static DataTable GetTrain(int trainid)
        {
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                string sql = "select * from m_train where id=" + trainid;
                DataTable dt = sqlHelper.ExecuteQueryDataTable(sql);
                return dt;
            }
        }

        public static DataTable GetDevice(int deviceid)
        {
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                string sql = "select * from m_device where id=" + deviceid;
                DataTable dt = sqlHelper.ExecuteQueryDataTable(sql);
                return dt;
            }
        }

        public static DataTable GetDevices(int devicetype)
        {
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                string sql = "select * from m_device where device_type_id=" + devicetype;
                DataTable dt = sqlHelper.ExecuteQueryDataTable(sql);
                return dt;
            }
        }

        public static void GetParameter(ref Dictionary<string, object> args, out object target)
        {
            if (args == null)
                args = new Dictionary<string,object>();

            target = null;

            string parameter = ConfigurationManager.AppSettings["Parameter"];
            string[] paras = parameter.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            foreach (string arg in paras)
            {
                if (args.ContainsKey(arg))
                {
                    UserInfo ui = args[arg] as UserInfo;
                    int id = Convert.ToInt32(args[arg]);
                    switch (arg)
                    {
                        case "{USERNAME}":
                            if (ui != null)
                                args[arg] = ui.Login_name;
                            break;
                        case "{PWD}":
                            if (ui != null)
                                args[arg] = ui.Password;
                            break;
                        case "{ID}":
                            args[arg] = id;
                            break;
                        case "{TARGET}":
                            target = args[arg];
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public static Form GetForm(MenuInfo menu, Dictionary<string, object> param = null)
        {
            if (menu == null)
                return null;
            if (string.IsNullOrEmpty(menu.ReferenceForm))
                return null;

            string parameter = ConfigurationManager.AppSettings["Parameter"];
            string[] paras = parameter.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            List<string> ps = new List<string>(paras);
            List<object> args = new List<object>();
            string assFile = "";
            if (!string.IsNullOrEmpty(menu.AssemblyFile))
            {
                assFile = AppDomain.CurrentDomain.BaseDirectory + menu.AssemblyFile;
            }
            if (File.Exists(assFile))
            {
                Assembly ass = Assembly.LoadFrom(assFile);
                if (ass != null)
                {
                    Type[] types = ass.GetTypes();
                    if (types != null)
                    {
                        foreach (Type t in types)
                        {
                            if (t.IsSubclassOf(typeof(Form)) && t.FullName.Equals(menu.ReferenceForm, StringComparison.InvariantCultureIgnoreCase))
                            {
                                foreach (string arg in menu.Args)
                                {
                                    if (ps.Contains(arg))
                                    {
                                        if (param.ContainsKey(arg))
                                        {
                                            object para = param[arg];
                                            args.Add(para);
                                        }
                                    }
                                    else
                                    {
                                        args.Add(arg);
                                    }
                                }
                                if (args.Count > 0)
                                    return (Form)ass.CreateInstance(t.FullName, true, BindingFlags.CreateInstance | BindingFlags.Instance | BindingFlags.Public, null, args.ToArray(), null, null);
                                else
                                    return (Form)ass.CreateInstance(t.FullName, true, BindingFlags.CreateInstance | BindingFlags.Instance | BindingFlags.Public, null, null, null, null);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("指定的程序集中没有任何类！");
                    }
                }
            }
            else
            {
                if (assFile == "")
                {
                    Assembly ass = Assembly.GetExecutingAssembly();
                    Type[] types = ass.GetTypes();
                    if (types != null)
                    {
                        foreach (Type t in types)
                        {
                            if (t.IsSubclassOf(typeof(Form)) && t.FullName.Equals(menu.ReferenceForm, StringComparison.InvariantCultureIgnoreCase))
                            {
                                foreach (string arg in menu.Args)
                                {
                                    if (ps.Contains(arg))
                                    {
                                        if (param.ContainsKey(arg))
                                        {
                                            object para = param[arg];
                                            args.Add(para);
                                        }
                                    }
                                    else
                                    {
                                        args.Add(arg);
                                    }
                                }
                                if (args.Count > 0)
                                    return (Form)ass.CreateInstance(t.FullName, true, BindingFlags.CreateInstance | BindingFlags.Instance | BindingFlags.Public, null, args.ToArray(), null, null);
                                else
                                    return (Form)ass.CreateInstance(t.FullName, true, BindingFlags.CreateInstance | BindingFlags.Instance | BindingFlags.Public, null, null, null, null);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("本程序集中没有任何类！");
                    }
                }
                else
                {
                    MessageBox.Show("指定的程序集[" + assFile + "]在当前目录下不存在！");
                }
            }
            return null;
        }

        public static MonitorForm GetForm(string formName, object owner, object[] args)
        {
            if (!string.IsNullOrEmpty(formName))
                return null;
            List<object> para = new List<object>();
            para.Add(owner);
            para.AddRange(args);
            Assembly ass = Assembly.GetExecutingAssembly();
            Type[] types = ass.GetTypes();
            if (types != null)
            {
                foreach (Type t in types)
                {
                    if (t.IsSubclassOf(typeof(MonitorForm)) && t.Name.Equals(formName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return (MonitorForm)ass.CreateInstance(t.FullName, true, BindingFlags.CreateInstance | BindingFlags.Instance | BindingFlags.Public, null, para.ToArray(), null, null);
                    }
                }
            }
            else
            {
                MessageBox.Show("本程序集中没有任何类！");
            }
            return null;
        }

        public static bool LoadTrains(KellComboBox combo, bool showAll = false)
        {
            combo.Items.Clear();
            if (showAll)
            {
                combo.AddItem(null, "全部");
            }
            string sql = "select id,train_no from m_train";
            DbCommand cmd = GenericDataAccess.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            DataTable dt = GenericDataAccess.ExecuteSelectCommand(cmd);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    combo.AddItem(dt.Rows[i][0], dt.Rows[i][1].ToString());
                }
            }
            if (dt.Rows.Count > 0)
            {
                combo.SelectedIndex = 0;
            }
            if (showAll)
                return dt.Rows.Count > 1;
            else
                return dt.Rows.Count > 0;
        }

        public static bool LoadStations(KellComboBox combo, bool showAll = false)
        {
            combo.Items.Clear();
            if (showAll)
            {
                combo.AddItem(null, "全部");
            }
            string sql = "select id,station_name from m_station";
            DbCommand cmd = GenericDataAccess.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            DataTable dt = GenericDataAccess.ExecuteSelectCommand(cmd);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    combo.AddItem(dt.Rows[i][0], dt.Rows[i][1].ToString());
                }
            }
            if (dt.Rows.Count > 0)
            {
                combo.SelectedIndex = 0;
            }
            if (showAll)
                return dt.Rows.Count > 1;
            else
                return dt.Rows.Count > 0;
        }

        public static bool LoadDevices(KellComboBox combo, int trainid, bool showAll = false)
        {
            combo.Items.Clear();
            if (showAll)
            {
                combo.AddItem(null, "全部");
            }
            string sql = "select id, device_name from m_device where train_id=" + trainid;
            DbCommand cmd = GenericDataAccess.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            DataTable dt = GenericDataAccess.ExecuteSelectCommand(cmd);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    combo.AddItem(dt.Rows[i][0], dt.Rows[i][1].ToString());
                }
            }
            if (dt.Rows.Count > 0)
            {
                combo.SelectedIndex = 0;
            }
            if (showAll)
                return dt.Rows.Count > 1;
            else
                return dt.Rows.Count > 0;
        }

        public static bool LoadStatusTypes(KellComboBox combo, bool showAll = false)
        {
            combo.Items.Clear();
            if (showAll)
            {
                combo.AddItem(null, "全部");
            }
            string sql = "select status_type_id,status_type_name from s_status_type";
            DbCommand cmd = GenericDataAccess.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            DataTable dt = GenericDataAccess.ExecuteSelectCommand(cmd);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    combo.AddItem(dt.Rows[i][0], dt.Rows[i][1].ToString());
                }
            }
            if (dt.Rows.Count > 0)
            {
                combo.SelectedIndex = 0;
            }
            if (showAll)
                return dt.Rows.Count > 1;
            else
                return dt.Rows.Count > 0;
        }

        public static bool LoadAlarmTypes(KellComboBox combo, bool showAll = false)
        {
            combo.Items.Clear();
            if (showAll)
            {
                combo.AddItem(null, "全部");
            }
            string sql = "select alarm_type_id,alarm_type_name from s_alarm_type";
            DbCommand cmd = GenericDataAccess.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            DataTable dt = GenericDataAccess.ExecuteSelectCommand(cmd);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    combo.AddItem(dt.Rows[i][0], dt.Rows[i][1].ToString());
                }
            }
            if (dt.Rows.Count > 0)
            {
                combo.SelectedIndex = 0;
            }
            if (showAll)
                return dt.Rows.Count > 1;
            else
                return dt.Rows.Count > 0;
        }

        public static bool LoadDeviceTypes(KellComboBox combo, bool showAll = false)
        {
            combo.Items.Clear();
            if (showAll)
            {
                combo.AddItem(null, "全部");
            }
            string sql = "select device_type_id,device_type_name from s_device_type";
            DbCommand cmd = GenericDataAccess.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            DataTable dt = GenericDataAccess.ExecuteSelectCommand(cmd);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    combo.AddItem(dt.Rows[i][0], dt.Rows[i][1].ToString());
                }
            }
            if (dt.Rows.Count > 0)
            {
                combo.SelectedIndex = 0;
            }
            if (showAll)
                return dt.Rows.Count > 1;
            else
                return dt.Rows.Count > 0;
        }

        public static bool LoadPointTypes(KellComboBox combo, int deviceTypeId, bool showAll = false)
        {
            combo.Items.Clear();
            if (showAll)
            {
                combo.AddItem(null, "全部");
            }
            string sql = "select point_type_id, point_type_name from s_point_type where device_type_id=" + deviceTypeId;
            DbCommand cmd = GenericDataAccess.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            DataTable dt = GenericDataAccess.ExecuteSelectCommand(cmd);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    combo.AddItem(dt.Rows[i][0], dt.Rows[i][1].ToString());
                }
            }
            if (dt.Rows.Count > 0)
            {
                combo.SelectedIndex = 0;
            }
            if (showAll)
                return dt.Rows.Count > 1;
            else
                return dt.Rows.Count > 0;
        }

        public static string GetEnableId(string name)
        {
            string id = "2";
            if (name == "不可见不报警")
            {
                id = "0";
            }
            else if (name == "可见不报警")
            {
                id = "1";
            }
            else if (name == "可见可报警")
            {
                id = "2";
            }
            return id;
        }

        public static string GetEnableById(string id)
        {
            string name = "可见可报警";
            int RET;
            if (int.TryParse(id, out RET))
            {
                if (RET == 0)
                {
                    name = "不可见不报警";
                }
                else if (RET == 1)
                {
                    name = "可见不报警";
                }
                else if (RET == 2)
                {
                    name = "可见可报警";
                }
            }
            return name;
        }

        public static Argument GetArgument(int deviceTypeId, short pointTypeId, bool? enable = null)
        {
            Argument arg = null;
            string sql = "select * from s_argument where device_type_id=" + deviceTypeId + " and point_type_id=" + pointTypeId;
            if (enable != null && enable.HasValue)
                sql += " and isEnable=" + (enable.Value ? "1" : "0");
            DbCommand cmd = GenericDataAccess.CreateCommand();
            cmd.CommandText = sql;
            cmd.CommandType = CommandType.Text;
            DataTable dt = GenericDataAccess.ExecuteSelectCommand(cmd);
            if (dt != null && dt.Rows.Count > 0)
            {
                arg = new Argument(Convert.ToInt32(dt.Rows[0]["id"]), Convert.ToInt16(dt.Rows[0]["device_type_id"]), Convert.ToInt16(dt.Rows[0]["point_type_id"]), dt.Rows[0]["standard_value"].ToString(), dt.Rows[0]["min_value"].ToString(), dt.Rows[0]["max_value"].ToString(), Convert.ToBoolean(dt.Rows[0]["isRange"]), Convert.ToBoolean(dt.Rows[0]["isEnable"]), Convert.ToBoolean(dt.Rows[0]["valueIsNumeric"]), dt.Rows[0]["argument_name"].ToString());
            }
            return arg;
        }

        public static string GetDirection(string code)
        {
            return code == "0" ? "正向" : "反向";
        }

        public static string GetDirectionCode(string dir)
        {
            return dir == "正向" ? "0" : "1";
        }

        public static DateTime GetLastestAlarmTime()
        {
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                string sql = "select top 1 start_time from d_alarm_log where alarm_status<>0 and affirmance=0 order by start_time desc";
                object obj = sqlHelper.ExecuteScalar(sql);
                if (obj != null && obj != DBNull.Value)
                {
                    return Convert.ToDateTime(obj);
                }
                return DateTime.MinValue;
            }
        }

        public static DateTime GetAlarmTimeById(int id)
        {
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                string sql = "select start_time from d_alarm_log where id=" + id;
                object obj = sqlHelper.ExecuteScalar(sql);
                if (obj != null && obj != DBNull.Value)
                {
                    return Convert.ToDateTime(obj);
                }
                return DateTime.MinValue;
            }
        }
    }
    public class CryptUtil
    {
        static MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

        public static MD5CryptoServiceProvider Md5
        {
            get { return md5; }
        }

        public static string GetMD5Hash(string input)
        {
            input = input ?? "";
            byte[] res = md5.ComputeHash(Encoding.Unicode.GetBytes(input), 0, input.Length);
            char[] temp = new char[res.Length];
            System.Array.Copy(res, temp, res.Length);
            return new String(temp);
        }

        static DESCryptoServiceProvider des = new DESCryptoServiceProvider();

        public static DESCryptoServiceProvider DES
        {
            get { return des; }
        }

        const string EncryptionKey = "OuKell";
        const string EncryptionIV = "kell";

        public static string EncodeDes(string input)
        {
            byte[] SourceData = Encoding.Unicode.GetBytes(input);
            byte[] returnData = null;
            try
            {
                des.Key = ASCIIEncoding.Unicode.GetBytes(EncryptionKey);
                des.IV = ASCIIEncoding.Unicode.GetBytes(EncryptionIV);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);
                cs.Write(SourceData, 0, SourceData.Length);
                cs.FlushFinalBlock();
                returnData = ms.ToArray();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Encoding.Unicode.GetString(returnData);
        }

        public static string DecodeDes(string input)
        {
            byte[] SourceData = Encoding.Unicode.GetBytes(input);
            byte[] returnData = null;
            try
            {
                DESCryptoServiceProvider desProvider = new DESCryptoServiceProvider();
                desProvider.Key = Encoding.Unicode.GetBytes(EncryptionKey);
                desProvider.IV = Encoding.Unicode.GetBytes(EncryptionIV);
                MemoryStream ms = new MemoryStream();
                ICryptoTransform encrypto = desProvider.CreateDecryptor();
                CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);
                cs.Write(SourceData, 0, SourceData.Length);
                cs.FlushFinalBlock();
                returnData = ms.ToArray();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Encoding.Unicode.GetString(returnData);
        }
    }
}