using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MergeQueryUtil;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data;

namespace Monitor.App_Code
{
    /// <summary>
    ///UserUtil 的摘要说明
    /// </summary>
    public static class UserUtil
    {
        public static bool ExistsName(string name)
        {
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                object o = sqlHelper.ExecuteScalar("select count(*) from a_user where login_name='" + name + "'");
                if (o != null && o != DBNull.Value)
                {
                    return Convert.ToInt32(o) > 0;
                }
                return false;
            }
        }

        public static bool ExistsNameOther(string name, int id)
        {
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                object o = sqlHelper.ExecuteScalar("select count(*) from a_user where login_name='" + name + "' and ID!=" + id);
                if (o != null && o != DBNull.Value)
                {
                    return Convert.ToInt32(o) > 0;
                }
                return false;
            }
        }

        public static List<UserInfo> GetUserList(string where = null)
        {
            List<UserInfo> users = new List<UserInfo>();
            string w = "";
            if (!string.IsNullOrEmpty(where))
            {
                w = where.Trim().ToLower();
                if (!w.StartsWith("where "))
                    w = "where " + w;
            }
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                using (DbDataReader reader = sqlHelper.ExecuteQueryReader("select id, login_name, password, operation, add_time, power_time from a_user " + w))
                {
                    while (reader.Read())
                    {
                        UserInfo ui = new UserInfo();
                        ui.ID = reader.GetInt32(0);
                        ui.Login_name = reader.GetString(1);
                        ui.Password = Convert.IsDBNull(reader[2]) ? "" : reader.GetString(2);
                        ui.Operation = reader.GetInt32(3);
                        ui.Add_time = reader.GetDateTime(4);
                        ui.Power_time = reader.GetDateTime(5);
                        users.Add(ui);
                    }
                }
            }
            return users;
        }

        public static DataTable GetUsers(string where = null)
        {
            string w = "";
            if (!string.IsNullOrEmpty(where))
            {
                w = where.Trim().ToLower();
                if (!w.StartsWith("where "))
                    w = "where " + w;
            }
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                return sqlHelper.ExecuteQueryDataTable("select id, login_name, password, operation, add_time, power_time from a_user " + w);
            }
        }

        public static UserInfo GetUser(int id)
        {
            UserInfo ui = new UserInfo();
            SqlHelper sqlHelper = new SqlHelper();
            using (DbDataReader reader = sqlHelper.ExecuteQueryReader("select login_name, password, operation, add_time, power_time from a_user where id=" + id))
            {
                if (reader.Read())
                {
                    ui.ID = id;
                    ui.Login_name = reader.GetString(1);
                    ui.Password = Convert.IsDBNull(reader[2]) ? "" : reader.GetString(2);
                    ui.Operation = reader.GetInt32(3);
                    ui.Add_time = reader.GetDateTime(4);
                    ui.Power_time = reader.GetDateTime(5);
                }
                sqlHelper.Conn.Close();
            }
            return ui;
        }

        public static int AddUser(UserInfo ui)
        {
            SqlHelper sqlHelper = new SqlHelper();
            SqlParameter[] paras = new SqlParameter[]
            {
                new SqlParameter("@login_name", ui.Login_name),
                new SqlParameter("@password", ui.Password),
                new SqlParameter("@operation", ui.Operation),
                new SqlParameter("@power_time", ui.Power_time)
            };
            object o = sqlHelper.ExecuteScalar("insert into a_user (login_name, password, operation, power_time) values (@login_name, @password, @operation, @power_time); select SCOPE_IDENTITY()", CommandType.Text, paras);
            if (o != null && o != DBNull.Value)
                return Convert.ToInt32(o);
            return 0;
        }

        public static bool UpdateUser(UserInfo ui)
        {
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                SqlParameter[] paras = new SqlParameter[]
            {
                new SqlParameter("@login_name", ui.Login_name),
                new SqlParameter("@password", ui.Password),
                new SqlParameter("@operation", ui.Operation),
                new SqlParameter("@power_time", ui.Power_time)
            };
                return sqlHelper.ExecuteNonQuery("update a_user set login_name=@login_name,password=@password,operation=@operation,power_time=@power_time where ID=" + ui.ID, CommandType.Text, paras) > 0;
            }
        }

        public static bool ChangePwd(int id, string newPwd)
        {
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                SqlParameter[] paras = new SqlParameter[]
            {
                new SqlParameter("@id", id),
                new SqlParameter("@password", newPwd)
            };
                return sqlHelper.ExecuteNonQuery("update a_user set password=@password where id=" + id, CommandType.Text, paras) > 0;
            }
        }

        public static bool DeleteUser(UserInfo ui)
        {
            return DeleteUser(ui.ID);
        }

        public static bool DeleteUser(int id)
        {
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                return sqlHelper.ExecuteNonQuery("delete from a_user where ID=" + id) > 0;
            }
        }
    }

    public class UserInfo
    {
        int id;

        public int ID
        {
            get { return id; }
            set { id = value; }
        }
        string login_name;

        public string Login_name
        {
            get { return login_name; }
            set { login_name = value; }
        }
        string password = string.Empty;

        public string Password
        {
            get { return password; }
            set { password = value; }
        }
        int operation;

        public int Operation
        {
            get { return operation; }
            set { operation = value; }
        }
        DateTime add_time;

        public DateTime Add_time
        {
            get { return add_time; }
            set { add_time = value; }
        }
        DateTime power_time;

        public DateTime Power_time
        {
            get { return power_time; }
            set { power_time = value; }
        }

        public override string ToString()
        {
            return login_name;
        }
    }
}
