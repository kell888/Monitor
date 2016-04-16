using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Web;
using MergeQueryUtil;

namespace Monitor.App_Code
{
    /// <summary>
    /// 错误级别
    /// </summary>
    public enum ErrorLevel : byte
    {
        Normal = 0,
        DBError = 1,
        AppError = 2
    }
    public static class Logs
    {
        public static bool LogError(ErrorLevel err_level, string err_source, string err_msg)
        {
            byte level = (byte)err_level;
            string ip = Common.GetIPv4().ToString();
            string mac = Common.GetMac();
            string client = ip + "(" + mac + ")";
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                try
                {
                    int r = sqlHelper.ExecuteNonQuery("insert into d_error_log (err_level, err_source, err_msg, err_client) values (" + level + ", '" + err_source + "', '" + err_msg + "', '" + client + "')");
                    return r > 0;
                }
                catch (Exception e)
                {
                    Logs.Error("LogError:" + e.Message);
                }
            }
            return false;
        }
        static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public static void Error(string msg)
        {
            try
            {
                logger.Error(string.Format("[{0}]:{1}" + Environment.NewLine, DateTime.Now, msg));
            }
            catch { }
        }
        public static void Trace(string msg)
        {
            try
            {
                logger.Trace(string.Format("[{0}]:{1}" + Environment.NewLine, DateTime.Now, msg));
            }
            catch { }
        }
        public static void Info(string msg)
        {
            try
            {
                logger.Info(string.Format("[{0}]:{1}" + Environment.NewLine, DateTime.Now, msg));
            }
            catch { }
        }
        public static void Init()
        {
            try
            {
                logger.Info(string.Format("[{0}]:{1}" + Environment.NewLine, DateTime.Now, "应用程序启动中..."));
            }
            catch { }
        }
    }
}