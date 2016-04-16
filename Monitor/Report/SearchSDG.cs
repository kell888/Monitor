using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Monitor.App_Code;
using System.Configuration;
using System.Data.Common;
using MergeQueryUtil;

namespace Monitor.Report
{
    public partial class SearchSDG : MonitorForm
    {
        public SearchSDG(Index owner)
        : base(owner)
        {
            InitializeComponent();
            winFormPager1.PageSize = 20;
            this.owner = owner;
            owner.ShowInfo("监测数据查询");
        }

        Index owner;
        DataTable dt;

        private void SearchSDG_Load(object sender, EventArgs e)
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
            if (Common.LoadTrains(comboBox1, true))
            {
                int train_id = Convert.ToInt32(comboBox1.GetCurrentItemValue());
                Common.LoadDevices(comboBox1, train_id, true);
            }
            Common.LoadPointTypes(comboBox3, 1, true);
            Common.LoadStations(comboBox5, true);
            comboBox4.SelectedIndex = 0;
            button1.PerformClick();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Common.LoadDevices(comboBox2, Convert.ToInt32(comboBox1.GetCurrentItemValue()));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataTable dtt = null;
            string s = "";
            string device_name = null;
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
                device_name = comboBox2.GetCurrentItemText();
                s += " and device_name='" + device_name + "'";
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
                dtt = sqlHelper.ExecuteQueryDataTable("select * from v_data_log where flash_time between '" + dateTimePicker1.Value + "' and '" + dateTimePicker2.Value + "'" + s);//MergeQuery.GetDataRange("v_data_log", "*", "flash_time", dateTimePicker1.Value, dateTimePicker2.Value, "(1=1)" + s);
            }
            int pageCount;
            string msg;
            Dictionary<string, string> cols = new Dictionary<string, string>();
            cols.Add("id", "ID");
            cols.Add("flash_time", "监测时间");
            cols.Add("device_name", "设备名称");
            cols.Add("train_no", "列车号");
            cols.Add("point_type_name", "监测项目");
            cols.Add("data_value", "检测值");
            cols.Add("status_type_name", "检测状态");
            cols.Add("station_name", "监测站");
            cols.Add("direction", "行车方向");
            cols.Add("line_no", "线路");
            dt = GridUtil.ViewData(dtt, cols);
            winFormPager1.RecordCount = dtt.Rows.Count;
            DataView dv = Paging.GetPagerForView(dt, winFormPager1.PageSize, winFormPager1.PageIndex, out pageCount, out msg);
            GridUtil.BindData(outlookGrid1, dv.Table);
            if (dv.Count > 0)
            {
                Delegate dlgt = new Action<string, Form>(owner.AddTabPage);
                object target = owner;
                CallbackArgs callback = new CallbackArgs(dlgt, target, new object[] { "监测数据", "$SdgReport|{OWNER},{FLASHTIME},{TRAINNO}$" });
                string columnKey;
                GridUtil.AddLinkColumn(outlookGrid1, "查看报告", out columnKey, callback);
            }
        }

        private void winFormPager1_PageIndexChanged(object sender, EventArgs e)
        {
            int pageCount;
            string msg;
            DataView dv = Paging.GetPagerForView(dt, winFormPager1.PageSize, winFormPager1.PageIndex, out pageCount, out msg);
            GridUtil.BindData(outlookGrid1, dv.Table);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Exporter.ExportToExcel(outlookGrid1.DataSource as DataTable, "监测数据报表");
        }
    }
}
