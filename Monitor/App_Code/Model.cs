using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MergeQueryUtil;
using System.Data;

namespace Monitor.App_Code
{
    public class PointType
    {
        public int id { get; set; }
        public short device_type_id { get; set; }
        public short point_type_id { get; set; }
        public string point_type_name { get; set; }
        public string point_type_unit { get; set; }
        public int point_type_state { get; set; }

        public override string ToString()
        {
            return point_type_name;
        }

        public static List<PointType> GetAllPointTypes()
        {
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                List<PointType> elements = new List<PointType>();
                string sql = "select * from s_point_type";
                DataTable dt = sqlHelper.ExecuteQueryDataTable(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        PointType element = new PointType();
                        element.id = Convert.ToInt32(dt.Rows[i]["id"]);
                        element.device_type_id = Convert.ToInt16(dt.Rows[i]["device_type_id"]);
                        element.point_type_id = Convert.ToInt16(dt.Rows[i]["point_type_id"]);
                        element.point_type_name = dt.Rows[i]["point_type_name"].ToString();
                        element.point_type_unit = dt.Rows[i]["point_type_unit"].ToString();
                        element.point_type_state = Convert.ToInt32(dt.Rows[i]["point_type_state"]);
                        elements.Add(element);
                    }
                }
                return elements;
            }
        }
    }
    public class DeviceType
    {
        public int id { get; set; }
        public short device_type_id { get; set; }
        public string device_type_name { get; set; }

        public override string ToString()
        {
            return device_type_name;
        }

        public static List<DeviceType> GetAllDeviceTypes()
        {
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                List<DeviceType> elements = new List<DeviceType>();
                string sql = "select * from s_device_type";
                DataTable dt = sqlHelper.ExecuteQueryDataTable(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DeviceType element = new DeviceType();
                        element.id = Convert.ToInt32(dt.Rows[i]["id"]);
                        element.device_type_id = Convert.ToInt16(dt.Rows[i]["device_type_id"]);
                        element.device_type_name = dt.Rows[i]["device_type_name"].ToString();
                        elements.Add(element);
                    }
                }
                return elements;
            }
        }
    }
    public class Device
    {
        public int id { get; set; }
        public int device_type_id { get; set; }
        public int train_id { get; set; }
        public string device_name { get; set; }
        public string address { get; set; }

        public override string ToString()
        {
            return device_name;
        }

        public static List<Device> GetAllDevices()
        {
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                List<Device> elements = new List<Device>();
                string sql = "select * from m_device";
                DataTable dt = sqlHelper.ExecuteQueryDataTable(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Device element = new Device();
                        element.id = Convert.ToInt32(dt.Rows[i]["id"]);
                        element.train_id = Convert.ToInt32(dt.Rows[i]["train_id"]);
                        element.device_type_id = Convert.ToInt16(dt.Rows[i]["device_type_id"]);
                        element.device_name = dt.Rows[i]["device_name"].ToString();
                        element.address = dt.Rows[i]["address"].ToString();
                        elements.Add(element);
                    }
                }
                return elements;
            }
        }

        public static bool Update(Device element)
        {
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                int r = sqlHelper.ExecuteNonQuery("update m_device set train_id=" + element.train_id + ", device_name='" + element.device_name + "', device_type_id=" + element.device_type_id + ", [address]='" + element.address + "' where id=" + element.id);
                return r > 0;
            }
        }

        public static bool Delete(int id)
        {
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                int r = sqlHelper.ExecuteNonQuery("delete from m_device where id=" + id);
                return r > 0;
            }
        }

        public static int Insert(Device element)
        {
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                object o = sqlHelper.ExecuteScalar("insert into m_device (train_id, device_name, device_type_id, [address]) values (" + element.train_id + ", '" + element.device_name + "', " + element.device_type_id + ", '" + element.address + "'); select SCOPE_IDENTITY()");
                if (o != null && o != DBNull.Value)
                    return Convert.ToInt32(o);
                return 0;
            }
        }
    }
    public class Argument
    {
        int id;
        public int Id
        {
            get { return id; }
            set { id = value; }
        }
        short device_type_id;

        public short Device_type_id
        {
            get { return device_type_id; }
            set { device_type_id = value; }
        }
        short point_type_id;

        public short Point_type_id
        {
            get { return point_type_id; }
            set { point_type_id = value; }
        }
        string standard_value;

        public string Standard_value
        {
            get { return standard_value; }
            set { standard_value = value; }
        }

        string argument_name;

        public string Argument_name
        {
            get { return argument_name; }
            set { argument_name = value; }
        }
        string min_value;

        public string Min_value
        {
            get { return min_value; }
            set { min_value = value; }
        }
        string max_value;

        public string Max_value
        {
            get { return max_value; }
            set { max_value = value; }
        }
        bool valueIsNumeric = true;

        public bool ValueIsNumeric
        {
            get { return valueIsNumeric; }
            set { valueIsNumeric = value; }
        }
        bool isEnable = true;

        public bool IsEnable
        {
            get { return isEnable; }
            set { isEnable = value; }
        }
        bool isRange;

        public bool IsRange
        {
            get { return isRange; }
            set { isRange = value; }
        }

        public override string ToString()
        {
            return Argument_name;
        }

        public Argument()
        {
        }

        public Argument(int id, short device_type_id, short point_type_id, string standard_value, string min_value, string max_value, bool isRange, bool isEnable, bool valueIsNumeric, string argument_name = null)
        {
            this.device_type_id = device_type_id;
            this.point_type_id = point_type_id;
            this.standard_value = standard_value;
            this.min_value = min_value;
            this.max_value = max_value;
            this.isRange = isRange;
            this.isEnable = isEnable;
            this.valueIsNumeric = valueIsNumeric;
            this.argument_name = argument_name;
        }

        public static List<Argument> GetAllArguments()
        {
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                List<Argument> elements = new List<Argument>();
                string sql = "select * from s_argument";
                DataTable dt = sqlHelper.ExecuteQueryDataTable(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Argument element = new Argument();
                        element.Id = Convert.ToInt32(dt.Rows[i]["id"]);
                        element.Device_type_id = Convert.ToInt16(dt.Rows[i]["device_type_id"]);
                        element.Point_type_id = Convert.ToInt16(dt.Rows[i]["point_type_id"]);
                        element.Argument_name = dt.Rows[i]["argument_name"].ToString();
                        element.Standard_value = dt.Rows[i]["standard_value"].ToString();
                        element.Min_value = dt.Rows[i]["min_value"].ToString();
                        element.Max_value = dt.Rows[i]["max_value"].ToString();
                        element.ValueIsNumeric = Convert.ToBoolean(dt.Rows[i]["valueIsNumeric"]);
                        element.IsRange = Convert.ToBoolean(dt.Rows[i]["isRange"]);
                        element.IsEnable = Convert.ToBoolean(dt.Rows[i]["isEnable"]);
                        elements.Add(element);
                    }
                }
                return elements;
            }
        }

        public static bool Update(Argument element)
        {
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                int r = sqlHelper.ExecuteNonQuery("update s_argument set device_type_id=" + element.Device_type_id + ", point_type_id=" + element.Point_type_id + ", argument_name='" + element.Argument_name + "', standard_value='" + element.Standard_value + "', min_value='" + element.Min_value + "', max_value='" + element.Max_value + "', valueIsNumeric=" + (element.ValueIsNumeric ? "1" : "0") + ", isRange=" + (element.IsRange ? "1" : "0") + ", isEnable=" + (element.IsEnable ? "1" : "0") + " where id=" + element.id);
                return r > 0;
            }
        }

        public static bool Delete(int id)
        {
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                int r = sqlHelper.ExecuteNonQuery("delete from s_argument where id=" + id);
                return r > 0;
            }
        }

        public static int Insert(Argument element)
        {
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                object o = sqlHelper.ExecuteScalar("insert into s_argument (device_type_id, point_type_id, argument_name, standard_value, min_value, max_value, valueIsNumeric, isRange, isEnable) values (" + element.Device_type_id + ", " + element.Point_type_id + ", '" + element.Argument_name + "', '" + element.Standard_value + "', '" + element.Min_value + "', '" + element.Max_value + "', " + element.ValueIsNumeric + ", " + element.IsRange + ", " + element.IsEnable + "'); select SCOPE_IDENTITY()");
                if (o != null && o != DBNull.Value)
                    return Convert.ToInt32(o);
                return 0;
            }
        }
    }
    public class Train
    {
        public int id { get; set; }
        public string train_no { get; set; }
        public int count { get; set; }

        public override string ToString()
        {
            return train_no;
        }

        public static List<Train> GetAllTrains()
        {
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                List<Train> elements = new List<Train>();
                string sql = "select * from m_train";
                DataTable dt = sqlHelper.ExecuteQueryDataTable(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Train element = new Train();
                        element.id = Convert.ToInt32(dt.Rows[i]["id"]);
                        element.train_no = dt.Rows[i]["train_no"].ToString();
                        element.count = Convert.ToInt16(dt.Rows[i]["count"]);
                        elements.Add(element);
                    }
                }
                return elements;
            }
        }

        public static bool Update(Train element)
        {
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                int r = sqlHelper.ExecuteNonQuery("update m_train set train_no='" + element.train_no + "', [count]=" + element.count + " where id=" + element.id);
                return r > 0;
            }
        }

        public static bool Delete(int id)
        {
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                int r = sqlHelper.ExecuteNonQuery("delete from m_train where id=" + id);
                return r > 0;
            }
        }

        public static int Insert(Train element)
        {
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                object o = sqlHelper.ExecuteScalar("insert into m_train (train_no, [count]) values ('" + element.train_no + "', " + element.count + "); select SCOPE_IDENTITY()");
                if (o != null && o != DBNull.Value)
                    return Convert.ToInt32(o);
                return 0;
            }
        }
    }
    public class Station
    {
        public int id { get; set; }
        public string line_no { get; set; }
        public string station_name { get; set; }
        public int Vedio_count { get; set; }

        public override string ToString()
        {
            return station_name;
        }

        public string address
        {
            get
            {
                return line_no + " " + station_name;
            }
        }

        public static List<Station> GetAllStations()
        {
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                List<Station> elements = new List<Station>();
                string sql = "select * from m_station";
                DataTable dt = sqlHelper.ExecuteQueryDataTable(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Station element = new Station();
                        element.id = Convert.ToInt32(dt.Rows[i]["id"]);
                        element.line_no = dt.Rows[i]["line_no"].ToString();
                        element.station_name = dt.Rows[i]["station_name"].ToString();
                        element.Vedio_count = Convert.ToInt32(dt.Rows[i]["Vedio_count"]);
                        elements.Add(element);
                    }
                }
                return elements;
            }
        }

        public static bool Update(Station element)
        {
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                int r = sqlHelper.ExecuteNonQuery("update m_station set line_no='" + element.line_no + "', station_name='" + element.station_name + "', Vedio_count=" + element.Vedio_count + " where id=" + element.id);
                return r > 0;
            }
        }

        public static bool Delete(int id)
        {
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                int r = sqlHelper.ExecuteNonQuery("delete from m_station where id=" + id);
                return r > 0;
            }
        }

        public static int Insert(Station element)
        {
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                object o = sqlHelper.ExecuteScalar("insert into m_station (line_no, station_name, Vedio_count) values ('" + element.line_no + "', '" + element.station_name + "', " + element.Vedio_count + "); select SCOPE_IDENTITY()");
                if (o != null && o != DBNull.Value)
                    return Convert.ToInt32(o);
                return 0;
            }
        }
    }
}
