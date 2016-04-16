using System;
using System.Collections.Generic;
using System.Web;

namespace Monitor.App_Code
{
    /// <summary>
    /// 标准值与阀值
    /// </summary>
    public class StandardValues
    {
        float standard_value;

        public float Standard_value
        {
            get { return standard_value; }
            set { standard_value = value; }
        }
        float mutation;

        public float Mutation
        {
            get { return mutation; }
            set { mutation = value; }
        }
        float normal_low_alarm;

        public float Normal_low_alarm
        {
            get { return normal_low_alarm; }
            set { normal_low_alarm = value; }
        }
        float normal_high_alarm;

        public float Normal_high_alarm
        {
            get { return normal_high_alarm; }
            set { normal_high_alarm = value; }
        }
        short enable = 2;

        public short Enable
        {
            get { return enable; }
            set { enable = value; }
        }

        public StandardValues(float standard_value, float mutation, float normal_low_alarm, float normal_high_alarm, short enable)
        {
            this.standard_value = standard_value;
            this.mutation = mutation;
            this.normal_low_alarm = normal_low_alarm;
            this.normal_high_alarm = normal_high_alarm;
            this.enable = enable;
        }
        public StandardValues(float mutation, float normal_low_alarm, float normal_high_alarm)
        {
            this.mutation = mutation;
            this.normal_low_alarm = normal_low_alarm;
            this.normal_high_alarm = normal_high_alarm;
        }
    }
}