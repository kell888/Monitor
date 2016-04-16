using System;
using System.Collections.Generic;
using System.Text;


namespace Monitor.App_Code
{
    public class ObjectList
    {
        int id;
        int parent_id;
        string object_name;
        string full_name;
        int object_type_id;
        int device_type_id;
        int enable;

        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public string ObjectName
        {
            get { return object_name; }
            set { object_name = value; }
        }

        public int ObjectTypeId
        {
            get { return object_type_id; }
            set { object_type_id = value; }
        }

        public int ParentId
        {
            get { return parent_id; }
            set { parent_id = value; }
        }

        public string FullName
        {
            get { return full_name; }
            set { full_name = value; }
        }

        public int DeviceTypeId
        {
            get { return device_type_id; }
            set { device_type_id = value; }
        }

        public int Enable
        {
            get { return enable; }
            set { enable = value; }
        }
    }
}