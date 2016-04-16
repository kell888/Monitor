using System;
using System.Collections.Generic;
using System.Web;

namespace Monitor.App_Code
{
    /// <summary>
    /// 系统参数类
    /// </summary>
    public class Argument
    {
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
        bool valueIsNumeric;

        public bool ValueIsNumeric
        {
            get { return valueIsNumeric; }
            set { valueIsNumeric = value; }
        }
        bool isEnable;

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

        public Argument(short device_type_id, short point_type_id, string standard_value, string min_value, string max_value, bool isRange, bool isEnable, bool valueIsNumeric, string argument_name = null)
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
    }
}