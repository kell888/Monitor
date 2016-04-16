using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MergeQueryUtil;
using System.Data;
using System.IO;

namespace Monitor.App_Code
{
    public class TrainInfo
    {
        int id;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        int alarm_status;

        public int Alarm_status
        {
            get { return alarm_status; }
            set { alarm_status = value; }
        }

        public string Alarm_statusName
        {
            get
            {
                return GetStatusName(alarm_status);
            }
        }

        public static string GetStatusName(int alarm_status)
        {
            string sql = "SELECT status_type_name FROM s_status_type where status_type_id=" + alarm_status;

            using (SqlHelper sqlHelper = new SqlHelper())
            {
                object o = sqlHelper.ExecuteScalar(sql);
                if (o != null && o != DBNull.Value)
                    return o.ToString();
                else
                    return "";
            }
        }

        public static List<string> GetTrainPicFiles(int train_log_id)
        {
            List<string> files = new List<string>();
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                DataTable dt = sqlHelper.ExecuteQueryDataTable("select filepath from d_picVid_log where train_log_id=" + train_log_id + " and isVideo=0 order by saveTime");//MergeQuery.GetData("d_picVid_log", "filepath", "saveTime", "train_log_id=" + train_log_id + " and isVideo=0", "saveTime");
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i][0] != null && dt.Rows[i][0] != DBNull.Value)
                        files.Add(dt.Rows[i][0].ToString());
                }
            }
            return files;
        }

        public static int GetTrainLogId(string train_no, DateTime time)
        {
            int id = 0;
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                object o = sqlHelper.ExecuteScalar("select id from v_train_log where train_no='" + train_no + "' and come_time='" + time + "' order by come_time asc");
                //DataTable dt = MergeQuery.GetData("v_train_log", "id", "come_time", "train_no='" + train_no + "' and come_time='" + time + "'", "come_time asc");
                //if (dt.Rows.Count > 0)
                if (o != null && o != DBNull.Value)
                {
                    //if (dt.Rows[0][0] != null && dt.Rows[0][0] != DBNull.Value)
                    id = Convert.ToInt32(o);//Convert.ToInt32(dt.Rows[0][0]);
                }
            }
            return id;
        }

        public static string GetTrainPicPath(int train_log_id)
        {
            //DataTable dt = MergeQuery.GetData("d_picVid_log", "filepath", "saveTime", "train_log_id=" + train_log_id + " and isVideo=0", "saveTime asc");
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                object o = sqlHelper.ExecuteScalar("select filepath from d_picVid_log where train_log_id=" + train_log_id + " and isVideo=0 order by saveTime asc");
                if (o != null && o != DBNull.Value)
                //if (dt.Rows.Count > 0)
                {
                    //if (dt.Rows[0][0] != null && dt.Rows[0][0] != DBNull.Value)
                    return Path.GetDirectoryName(o.ToString());//dt.Rows[0][0].ToString());
                }
                return "";
            }
        }

        public static string GetTrainVideo(int train_log_id)
        {
            //DataTable dt = MergeQuery.GetData("d_picVid_log", "filepath", "saveTime", "train_log_id=" + train_log_id + " and isVideo=1", "saveTime asc");

            using (SqlHelper sqlHelper = new SqlHelper())
            {
                object o = sqlHelper.ExecuteScalar("select filepath from d_picVid_log where train_log_id=" + train_log_id + " and isVideo=1 order by saveTime asc");
                if (o != null && o != DBNull.Value)
                //if (dt.Rows.Count > 0)
                {
                    //if (dt.Rows[0][0] != null && dt.Rows[0][0] != DBNull.Value)
                    return o.ToString();// dt.Rows[0][0].ToString();
                }
                return "";
            }
        }

        public static DateTime GetTimeByDataId(int data_id)
        {
            DateTime flash_time = DateTime.MinValue;
            //DataTable dt = MergeQuery.GetData("v_data_log", "flash_time", "flash_time", "id=" + data_id, "flash_time asc");

            using (SqlHelper sqlHelper = new SqlHelper())
            {
                object o = sqlHelper.ExecuteScalar("select flash_time from v_data_log where id=" + data_id + " order by flash_time asc");
                if (o != null && o != DBNull.Value)
                //if (dt.Rows.Count > 0)
                {
                    //if (dt.Rows[0][0] != null && dt.Rows[0][0] != DBNull.Value)
                    flash_time = Convert.ToDateTime(o);//Convert.ToDateTime(dt.Rows[0][0]);
                }
                return flash_time;
            }
        }

        public static string GetTrainNoByDataId(int data_id)
        {
            //DataTable dt = MergeQuery.GetData("v_data_log", "train_no", "flash_time", "id=" + data_id, "flash_time asc");

            using (SqlHelper sqlHelper = new SqlHelper())
            {
                object o = sqlHelper.ExecuteScalar("select train_no from v_data_log where id=" + data_id + " order by flash_time asc");
                if (o != null && o != DBNull.Value)
                //if (dt.Rows.Count > 0)
                {
                    //if (dt.Rows[0][0] != null && dt.Rows[0][0] != DBNull.Value)
                    return o.ToString();// dt.Rows[0][0].ToString();
                }
                return "";
            }
        }

        public static string GetTrainNoByTime(DateTime time)
        {
            //DataTable dt = MergeQuery.GetDataAt("v_data_log", "train_no", "flash_time", time, null, "flash_time asc");

            using (SqlHelper sqlHelper = new SqlHelper())
            {
                object o = sqlHelper.ExecuteScalar("select train_no from v_data_log where flash_time='" + time + "' order by flash_time asc");
                if (o != null && o != DBNull.Value)
                //if (dt.Rows.Count > 0)
                {
                    //if (dt.Rows[0][0] != null && dt.Rows[0][0] != DBNull.Value)
                    return o.ToString();// dt.Rows[0][0].ToString();
                }
                return "";
            }
        }

        public static int GetTrainIdByNo(string train_no)
        {
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                object o = sqlHelper.ExecuteScalar("select id from m_train where train_no = '" + train_no + "'");
                if (o != null && o != DBNull.Value)
                    return Convert.ToInt32(o);
                return 0;
            }
        }

        public static string GetTrainNoByTrainLogId(int id)
        {
            //DataTable dt = MergeQuery.GetData("v_train_log", "train_no", "come_time", "id=" + id);

            using (SqlHelper sqlHelper = new SqlHelper())
            {
                object o = sqlHelper.ExecuteScalar("select train_no from v_train_log where id=" + id);
                if (o != null && o != DBNull.Value)
                //if (dt.Rows.Count > 0)
                {
                    //if (dt.Rows[0][0] != null && dt.Rows[0][0] != DBNull.Value)
                    return o.ToString();// dt.Rows[0][0].ToString();
                }
                return "";
            }
        }

        public static DateTime GetTimeByTrainLogId(int id)
        {
            //DataTable dt = MergeQuery.GetData("v_train_log", "come_time", "come_time", "id=" + id);

            using (SqlHelper sqlHelper = new SqlHelper())
            {
                object o = sqlHelper.ExecuteScalar("select come_time from v_train_log where id=" + id);
                if (o != null && o != DBNull.Value)
                //if (dt.Rows.Count > 0)
                {
                    //if (dt.Rows[0][0] != null && dt.Rows[0][0] != DBNull.Value)
                    return Convert.ToDateTime(o);// Convert.ToDateTime(dt.Rows[0][0]);
                }
                return DateTime.MinValue;
            }
        }

        public static void GetParas(int data_id, out DateTime time, out string point_type_name, out string device_name, out string train_no, out string direction)
        {
            time = DateTime.MinValue;
            point_type_name = "";
            device_name = "";
            train_no = "";
            direction = "";
            SqlHelper sqlHelper = new SqlHelper();
            DataTable dt = sqlHelper.ExecuteQueryDataTable("select flash_time,point_type_name,device_name,train_no,direction from v_data_log where id=" + data_id);//MergeQuery.GetData("v_data_log", "flash_time,point_type_name,device_name,train_no,direction", "flash_time", "id=" + data_id);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0][0] != null && dt.Rows[0][0] != DBNull.Value)
                    time = Convert.ToDateTime(dt.Rows[0][0]);
                if (dt.Rows[0][1] != null && dt.Rows[0][1] != DBNull.Value)
                    point_type_name = Convert.ToString(dt.Rows[0][1]);
                if (dt.Rows[0][2] != null && dt.Rows[0][2] != DBNull.Value)
                    device_name = Convert.ToString(dt.Rows[0][2]);
                if (dt.Rows[0][3] != null && dt.Rows[0][3] != DBNull.Value)
                    train_no = Convert.ToString(dt.Rows[0][3]);
                if (dt.Rows[0][4] != null && dt.Rows[0][4] != DBNull.Value)
                    direction = Convert.ToString(dt.Rows[0][4]);
            }
        }
    }
}
