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
    public partial class SearchTrain : MonitorForm
    {
        public SearchTrain(Index owner)
        : base(owner)
        {
            InitializeComponent();
            winFormPager1.PageSize = 20;
            this.owner = owner;
            owner.ShowInfo("过车信息查询");
        }

        Index owner;
        DataTable dt;

        private void SearchTrain_Load(object sender, EventArgs e)
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
            DataTable dtt = null;
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
                dtt = sqlHelper.ExecuteQueryDataTable("select * from v_train_log where come_time between '" + dateTimePicker1.Value + "' and '" + dateTimePicker2.Value + "'" + s);//MergeQuery.GetDataRange("v_train_log", "*", "come_time", dateTimePicker1.Value, dateTimePicker2.Value, "(1=1)" + s);
            }
            Dictionary<string, string> cols = new Dictionary<string, string>();
            cols.Add("id", "ID");
            cols.Add("train_no", "列车号");
            cols.Add("station_name", "监测站");
            cols.Add("come_time", "来车时间");
            cols.Add("go_time", "去车时间");
            cols.Add("alarm_count", "报警数");
            cols.Add("direction", "行车方向");
            cols.Add("status_type_name", "检测状态");
            cols.Add("line_no", "线路");
            dt = GridUtil.ViewData(dtt, cols);
            winFormPager1.RecordCount = dtt.Rows.Count;
            int pageCount;
            string msg;
            DataView dv = Paging.GetPagerForView(dt, winFormPager1.PageSize, winFormPager1.PageIndex, out pageCount, out msg);
            GridUtil.BindData(outlookGrid1, dv.Table);
            if (dv.Count > 0)
            {
                Delegate dlgt = new Action<string, Form>(owner.AddTabPage);
                object target = owner;
                CallbackArgs callback = new CallbackArgs(dlgt, target, new object[] { "过车信息", "$TrainReport|{OWNER},{ID}$" });
                string columnKey;
                GridUtil.AddLinkColumn(outlookGrid1, "详情", out columnKey, callback);
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
            Exporter.ExportToExcel(outlookGrid1.DataSource as DataTable, "过车信息报表");
        }
    }
}
