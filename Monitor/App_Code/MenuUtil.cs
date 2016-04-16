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
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;

namespace Monitor.App_Code
{
    /// <summary>
    ///MenuUtil 的摘要说明
    /// </summary>
    public static class MenuUtil
    {
        public static bool ExistsName(string name)
        {
            SqlHelper sqlHelper = new SqlHelper();
            object o = sqlHelper.ExecuteScalar("select count(*) from a_menu where [Name]='" + name + "'");
            if (o != null && o != DBNull.Value)
            {
                return Convert.ToInt32(o) > 0;
            }
            return false;
        }

        public static bool ExistsNameOther(string name, int id)
        {
            SqlHelper sqlHelper = new SqlHelper();
            object o = sqlHelper.ExecuteScalar("select count(*) from a_menu where [Name]='" + name + "' and ID!=" + id);
            if (o != null && o != DBNull.Value)
            {
                return Convert.ToInt32(o) > 0;
            }
            return false;
        }

        public static List<MeniInfo> GetMeniInfoList(string where = null)
        {
            List<MeniInfo> menus = new List<MeniInfo>();
            string w = "";
            if (!string.IsNullOrEmpty(where))
            {
                w = where.Trim().ToLower();
                if (!w.StartsWith("where "))
                    w = "where " + w;
            }
            SqlHelper sqlHelper = new SqlHelper();
            using (DbDataReader reader = sqlHelper.ExecuteQueryReader("select ID,[Name] from a_menu " + w + " order by SortIndex asc"))
            {
                while (reader.Read())
                {
                    MeniInfo mi = new MeniInfo();
                    mi.ID = reader.GetInt32(0);
                    mi.Name = reader.GetString(1);
                    menus.Add(mi);
                }
                sqlHelper.Conn.Close();
            }
            return menus;
        }

        public static List<MenuInfo> GetMenuList(string where = null)
        {
            List<MenuInfo> menus = new List<MenuInfo>();
            string w = "";
            if (!string.IsNullOrEmpty(where))
            {
                w = where.Trim().ToLower();
                if (!w.StartsWith("where "))
                    w = "where " + w;
            }
            SqlHelper sqlHelper = new SqlHelper();
            using (DbDataReader reader = sqlHelper.ExecuteQueryReader("select ID,ParentID,[Name],ReferenceForm,AssemblyFile,Args,SortIndex,[Enabled] from a_menu " + w + " order by SortIndex asc"))
            {
                while (reader.Read())
                {
                    MenuInfo mi = new MenuInfo();
                    mi.ID = reader.GetInt32(0);
                    mi.ParentID = reader.GetInt32(1);
                    mi.Name = reader.GetString(2);
                    mi.ReferenceForm = reader.GetString(3);
                    string assFile = Convert.IsDBNull(reader[4]) ? "" : reader.GetString(4);
                    mi.AssemblyFile = assFile;
                    string args = Convert.IsDBNull(reader[5]) ? "" : reader.GetString(5);
                    mi.Args = new List<string>(args.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
                    mi.SortIndex = reader.GetInt32(6);
                    mi.Enabled = reader.GetBoolean(7);
                    menus.Add(mi);
                }
                sqlHelper.Conn.Close();
            }
            return menus;
        }
        public static List<MenuInfo> GetEnabledMenuList(string where = null)
        {
            List<MenuInfo> menus = new List<MenuInfo>();
            string w = "";
            if (!string.IsNullOrEmpty(where))
            {
                w = where.Trim().ToLower();
                if (!w.StartsWith("where "))
                    w = "where " + w;
            }
            if (w != "")
                w += " and Enabled=1";
            else
                w = "where Enabled=1";
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                using (DbDataReader reader = sqlHelper.ExecuteQueryReader("select ID,ParentID,[Name],ReferenceForm,AssemblyFile,Args,SortIndex,[Enabled] from a_menu " + w + " order by SortIndex asc"))
                {
                    while (reader.Read())
                    {
                        MenuInfo mi = new MenuInfo();
                        mi.ID = reader.GetInt32(0);
                        mi.ParentID = reader.GetInt32(1);
                        mi.Name = reader.GetString(2);
                        mi.ReferenceForm = reader.GetString(3);
                        string assFile = Convert.IsDBNull(reader[4]) ? "" : reader.GetString(4);
                        mi.AssemblyFile = assFile;
                        string args = Convert.IsDBNull(reader[5]) ? "" : reader.GetString(5);
                        mi.Args = new List<string>(args.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
                        mi.SortIndex = reader.GetInt32(6);
                        mi.Enabled = reader.GetBoolean(7);
                        menus.Add(mi);
                    }
                }
            }
            return menus;
        }

        public static DataTable GetMenus(string where = null)
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
                return sqlHelper.ExecuteQueryDataTable("select ID,ParentID,[Name],ReferenceForm,AssemblyFile,Args,SortIndex,[Enabled] from a_menu " + w + " order by SortIndex asc");
            }
        }

        public static DataTable GetEnabledMenus(string where = null)
        {
            string w = "";
            if (!string.IsNullOrEmpty(where))
            {
                w = where.Trim().ToLower();
                if (!w.StartsWith("where "))
                    w = "where " + w;
            }
            if (w != "")
                w += " and [Enabled]=1";
            else
                w = "where [Enabled]=1";
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                return sqlHelper.ExecuteQueryDataTable("select ID,ParentID,[Name],ReferenceForm,AssemblyFile,Args,SortIndex,[Enabled] from a_menu " + w + " order by SortIndex asc");
            }
        }

        public static MenuInfo GetMenu(int id)
        {
            MenuInfo mi = new MenuInfo();
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                using (DbDataReader reader = sqlHelper.ExecuteQueryReader("select ID,ParentID,[Name],ReferenceForm,AssemblyFile,Args,SortIndex,[Enabled] from a_menu where ID=" + id))
                {
                    if (reader.Read())
                    {
                        mi.ID = reader.GetInt32(0);
                        mi.ParentID = reader.GetInt32(1);
                        mi.Name = reader.GetString(2);
                        mi.ReferenceForm = reader.GetString(3);
                        string assFile = Convert.IsDBNull(reader[4]) ? "" : reader.GetString(4);
                        mi.AssemblyFile = assFile;
                        string args = Convert.IsDBNull(reader[5]) ? "" : reader.GetString(5);
                        mi.Args = new List<string>(args.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
                        mi.SortIndex = reader.GetInt32(6);
                        mi.Enabled = reader.GetBoolean(7);
                    }
                }
            }
            return mi;
        }

        public static int AddMenu(MenuInfo mi)
        {
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                SqlParameter[] paras = new SqlParameter[]
            {
                new SqlParameter("@ParentID", mi.ParentID),
                new SqlParameter("@Name", mi.Name),
                new SqlParameter("@ReferenceForm", mi.ReferenceForm),
                new SqlParameter("@AssemblyFile", mi.AssemblyFile),
                new SqlParameter("@Args", string.Join(",", mi.Args.ToArray())),
                new SqlParameter("@SortIndex", mi.SortIndex),
                new SqlParameter("@Enabled", mi.Enabled)
            };
                object o = sqlHelper.ExecuteScalar("insert into a_menu (ParentID,[Name],ReferenceForm,AssemblyFile,Args,SortIndex,[Enabled]) values (@ParentID,@Name,@ReferenceForm,@AssemblyFile,@Args,@SortIndex,@Enabled); select SCOPE_IDENTITY()", CommandType.Text, paras);
                if (o != null && o != DBNull.Value)
                    return Convert.ToInt32(o);
                return 0;
            }
        }

        public static bool UpdateMenu(MenuInfo mi)
        {
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                SqlParameter[] paras = new SqlParameter[]
            {
                new SqlParameter("@ParentID", mi.ParentID),
                new SqlParameter("@Name", mi.Name),
                new SqlParameter("@ReferenceForm", mi.ReferenceForm),
                new SqlParameter("@AssemblyFile", mi.AssemblyFile),
                new SqlParameter("@Args", string.Join(",", mi.Args.ToArray())),
                new SqlParameter("@SortIndex", mi.SortIndex),
                new SqlParameter("@Enabled", mi.Enabled)
            };
                return sqlHelper.ExecuteNonQuery("update a_menu set ParentID=@ParentID,[Name]=@Name,ReferenceForm=@ReferenceForm,AssemblyFile=@AssemblyFile,Args=@Args,SortIndex=@SortIndex,[Enabled]=@Enabled where ID=" + mi.ID, CommandType.Text, paras) > 0;
            }
        }

        public static bool DeleteMenu(MenuInfo mi)
        {
            return DeleteMenu(mi.ID);
        }

        public static bool DeleteMenu(int id)
        {
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                return sqlHelper.ExecuteNonQuery("delete from a_menu where ID=" + id) > 0;
            }
        }
    }

    public static class Converter
    {
        public static object[] GetArgs(byte[] data)
        {
            object[] args = null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream(data))
            {
                args = (object[])bf.Deserialize(ms);
            }
            return args;
        }

        public static byte[] GetArgsData(object[] args)
        {
            byte[] data = null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream(data))
            {
                bf.Serialize(ms, args);
                data = ms.ToArray();
            }
            return data;
        }
    }

    public class MenuInfo
    {
        /// <summary>
        /// 菜单ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 上级菜单ID(顶级菜单时为0)
        /// </summary>
        public int ParentID { get; set; }
        /// <summary>
        /// 菜单名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 窗体的类全名
        /// </summary>
        public string ReferenceForm { get; set; }
        /// <summary>
        /// 为空即为本程序集，否则为外部程序集
        /// </summary>
        public string AssemblyFile { get; set; }
        /// <summary>
        /// 初始化窗体需要的参数列表
        /// </summary>
        public List<string> Args { get; set; }
        /// <summary>
        /// 排序位置
        /// </summary>
        public int SortIndex { get; set; }
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; }
        public override string ToString()
        {
            if (!string.IsNullOrEmpty(ReferenceForm))
                return SortIndex + ": " + Name + " - " + ReferenceForm;
            else
                return SortIndex + ": " + Name;
        }
    }

    public class MeniInfo
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public override string ToString()
        {
            return Name;
        }
    }
}