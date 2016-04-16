using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Monitor.App_Code;
using MergeQueryUtil;
using System.Configuration;

namespace Monitor.Touble
{
    public partial class SdgAlarmDispose : MonitorForm
    {
        public SdgAlarmDispose(Index owner)
            : base(owner)
        {
            InitializeComponent();
            winFormPager1.PageSize = 20;
            this.owner = owner;
            owner.ShowInfo("已处理故障");
        }

        Index owner;
        DataTable dt;

        private void SearchTroubleW_Load(object sender, EventArgs e)
        {
            winFormPager1.PageIndex = 1;
            int last = 50;
            string lastDays = ConfigurationManager.AppSettings["lastDays"];
            int RET;
            if (!string.IsNullOrEmpty(lastDays))
            {
                if (int.TryParse(lastDays, out RET))
                {
                    last = RET;
                }
            }
            DateTime dt = DateTime.Now;
            dateTimePicker1.Value = dt.AddDays(-last);//默认最近50天内的数据
            dateTimePicker2.Value = DateTime.Now;
            Common.LoadTrains(comboBox1, true);
            Common.LoadStations(comboBox2, true);
            //if (Common.LoadTrains(comboBox1, true))
            //{
            //    int train_id = Convert.ToInt32(comboBox1.GetCurrentItemValue());
            //    Common.LoadDevices(comboBox2, train_id, true);
            //}
            Common.LoadPointTypes(comboBox3, 1, true);
            comboBox4.SelectedIndex = 0;
            button1.PerformClick();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string s = "";
            string station_name = null;
            string direction = null;
            string point_type = null;
            string train_no = null;
            if (comboBox1.SelectedIndex > 0)
            {
                train_no = comboBox1.GetCurrentItemText();
                s += " and train_no='" + train_no + "'";
            }
            if (comboBox2.SelectedIndex > 0)
            {
                station_name = comboBox2.GetCurrentItemText();
                s += " and station_name='" + station_name + "'";
            }
            if (comboBox3.SelectedIndex > 0)
            {
                point_type = comboBox3.GetCurrentItemText();
                s += " and point_type_name='" + point_type + "'";
            }
            if (comboBox4.SelectedIndex > 0)
            {
                direction = comboBox4.SelectedValue.ToString();
                s += " and direction = '" + direction + "'";
            }
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                dt = sqlHelper.ExecuteQueryDataTable("select * from v_alarm_log where start_time between '" + dateTimePicker1.Value + "' and '" + dateTimePicker2.Value + "' and status_type_name='已处理'" + s);//MergeQuery.GetDataRange("v_alarm_log", "*", "start_time", dateTimePicker1.Value, dateTimePicker2.Value, "status_type_name='已处理'" + s);
            }
            int pageCount;
            string msg;
            DataView dv = Paging.GetPagerForView(dt, winFormPager1.PageSize, winFormPager1.PageIndex, out pageCount, out msg);
            GridUtil.BindData(outlookGrid1, dv.Table);
            if (dv.Count > 0)
            {
                Delegate dlgt = new Action<string, Form>(owner.AddTabPage);
                object target = owner;
                CallbackArgs callback = new CallbackArgs(dlgt, target, new object[] { "已处理故障", "$SdgReport|{FLASHTIME},{TRAINNO}$" });
                string columnKey;
                GridUtil.AddLinkColumn(outlookGrid1, "详情", out columnKey, callback);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Exporter.ExportToExcel(outlookGrid1.DataSource as DataTable, "已处理故障报表");
        }

        private void winFormPager1_PageIndexChanged(object sender, EventArgs e)
        {
            int pageCount;
            string msg;
            DataView dv = Paging.GetPagerForView(dt, winFormPager1.PageSize, winFormPager1.PageIndex, out pageCount, out msg);
            GridUtil.BindData(outlookGrid1, dv.Table);
        }
    }
}
