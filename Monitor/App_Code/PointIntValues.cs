using System;
using System.Collections.Generic;
using System.Web;

namespace Monitor.App_Code
{
    /// <summary>
    /// 检测项目的数字量报警参数类
    /// </summary>
    public class PointIntValues
    {
        short point_status;

        public short Point_status
        {
            get { return point_status; }
            set { point_status = value; }
        }
        short alarm_status;

        public short Alarm_status
        {
            get { return alarm_status; }
            set { alarm_status = value; }
        }
        short enable = 2;

        public short Enable
        {
            get { return enable; }
            set { enable = value; }
        }

        public PointIntValues(short point_status, short alarm_status, short enable)
        {
            this.point_status = point_status;
            this.alarm_status = alarm_status;
            this.enable = enable;
        }
        public PointIntValues(short point_status, short alarm_status)
        {
            this.point_status = point_status;
            this.alarm_status = alarm_status;
        }
    }
}