using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data;
using MergeQueryUtil;

namespace Monitor.App_Code
{
    public class ObjectManager
    {
        SqlHelper SqlHelper;
        public ObjectManager()
        {
            SqlHelper = new SqlHelper(null);
        }
        ~ObjectManager()
        {
            if (SqlHelper != null)
                SqlHelper.Dispose();
        }
        //得到所有对象
        public List<ObjectList> GetAllObject()
        {
            List<ObjectList> uis = new List<ObjectList>();
            string sql = "select * from [m_object] order by parent_id, object_type_id";
            DbDataReader reader = SqlHelper.ExecuteQueryReader(sql);
            try
            {
                while (reader.Read())
                {
                    ObjectList ui = new ObjectList();
                    ui.Id = int.Parse(reader["object_id"].ToString());
                    ui.ObjectName = reader["object_name"].ToString();
                    ui.ObjectTypeId = int.Parse(reader["object_type_id"].ToString());
                    ui.ParentId = int.Parse(reader["parent_id"].ToString());
                    ui.FullName = reader["full_name"].ToString();
                    ui.DeviceTypeId = int.Parse(reader["device_type_id"].ToString());
                    ui.Enable = int.Parse(reader["Enable"].ToString());
                    uis.Add(ui);
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                reader.Close();
            }
            return uis;
        }
        //得到所有对象
        public List<ObjectList> GetObjects(string where)
        {
            List<ObjectList> uis = new List<ObjectList>();
            string w = "";
            if (!string.IsNullOrEmpty(where))
            {
                w = where.Trim().ToLower();
                if (w.StartsWith("and "))
                {
                    w = w.Substring(4).Trim();
                    w = " where " + w;
                }
                else
                {
                    if (!w.StartsWith("where "))
                        w = " where " + w;
                }
            }
            string sql = "select * from [m_object]" + w + " order by parent_id, object_type_id";
            DbDataReader reader = SqlHelper.ExecuteQueryReader(sql);
            try
            {
                while (reader.Read())
                {
                    ObjectList ui = new ObjectList();
                    ui.Id = int.Parse(reader["object_id"].ToString());
                    ui.ObjectName = reader["object_name"].ToString();
                    ui.ObjectTypeId = int.Parse(reader["object_type_id"].ToString());
                    ui.ParentId = int.Parse(reader["parent_id"].ToString());
                    ui.FullName = reader["full_name"].ToString();
                    ui.DeviceTypeId = int.Parse(reader["device_type_id"].ToString());
                    ui.Enable = int.Parse(reader["Enable"].ToString());
                    uis.Add(ui);
                }
            }
            catch (Exception)
            {

            }
            finally
            {
                reader.Close();
            }
            return uis;
        }
        //得到指定的对象
        public ObjectList GetObject(int id)
        {
            string sql = "select * from [m_object] where [object_id] = " + id + "";
            DbDataReader reader = SqlHelper.ExecuteQueryReader(sql);
            try
            {
                if (reader.Read())
                {
                    ObjectList ui = new ObjectList();
                    ui.Id = int.Parse(reader["object_id"].ToString());
                    ui.ObjectName = reader["object_name"].ToString();
                    ui.ObjectTypeId = int.Parse(reader["object_type_id"].ToString());
                    ui.ParentId = int.Parse(reader["parent_id"].ToString());
                    ui.FullName = reader["full_name"].ToString();
                    ui.DeviceTypeId = int.Parse(reader["device_type_id"].ToString());
                    ui.Enable = int.Parse(reader["Enable"].ToString());
                    return ui;
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                reader.Close();
            }
            return new ObjectList();
        }

        //添加对象
        public int AddObject(ObjectList ui)
        {
            lock (this)
            {
                string sql = "usp_AddObject";
                SqlParameter[] param = new SqlParameter[] {     new SqlParameter("@object_name",ui.ObjectName),
                                                        new SqlParameter("@object_type_id",ui.ObjectTypeId),
                                                        new SqlParameter("@parent_id",ui.ParentId),
                                                        new SqlParameter("@full_name",ui.FullName),
                                                        new SqlParameter("@device_type_id",ui.DeviceTypeId),
                                                        new SqlParameter("@Enable",ui.Enable)};
                if (SqlHelper.ExecuteNonQuery(sql, CommandType.StoredProcedure, param) > 0)
                {
                    object obj = SqlHelper.ExecuteScalar("select IDENT_CURRENT('m_object')");
                    if (obj != null && obj != DBNull.Value)
                    {
                        return Convert.ToInt32(obj);
                    }
                }
                return 0;
            }
        }

        // //修改对象
        public bool EditObject(ObjectList ui)
        {
            string cmdText = "usp_UpdateObject";
            SqlParameter[] param = new SqlParameter[] {new SqlParameter("@object_id",ui.Id),
                                                        new SqlParameter("@object_name",ui.ObjectName),
                                                        new SqlParameter("@object_type_id",ui.ObjectTypeId),
                                                        new SqlParameter("@parent_id",ui.ParentId),
                                                        new SqlParameter("@full_name",ui.FullName),
                                                        new SqlParameter("@device_type_id",ui.DeviceTypeId),
                                                        new SqlParameter("@Enable",ui.Enable)};
            return SqlHelper.ExecuteNonQuery(cmdText, CommandType.StoredProcedure, param) > 0;
        }

        //删除对象
        public bool DelObject(int id)
        {
            string sql = "delete from [m_object] where [object_id]=@id";
            SqlParameter[] param = new SqlParameter[] { new SqlParameter("@id", id) };
            return SqlHelper.ExecuteNonQuery(sql, CommandType.Text, param) > 0;
        }

    }
}