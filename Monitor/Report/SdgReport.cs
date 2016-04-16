using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MergeQueryUtil;
using Monitor.App_Code;

namespace Monitor.Report
{
    public partial class SdgReport : MonitorForm
    {
        public SdgReport(Index owner, DateTime time, string train_no)
            : base(owner, time)
        {
            InitializeComponent();
            this.owner = owner;
            this.time = time;
            this.train_no = train_no;
            owner.ShowInfo("监测数据浏览");
        }

        Index owner;
        DateTime time;
        string train_no;

        private void SdgReport_Load(object sender, EventArgs e)
        {
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                DataTable dt = sqlHelper.ExecuteQueryDataTable("select * from v_data_log where flash_time='" + time + "' order by device_name, point_type_name");//MergeQuery.GetDataAt("v_data_log", "*", "flash_time", time, null, "device_name, point_type_name");
                GridUtil.BindData(outlookGrid1, dt);
                if (dt != null && dt.Rows.Count > 0)
                {
                    Delegate dlgt = new Action<string, Form>(owner.AddTabPage);
                    object target = owner;
                    XxReport xx = new XxReport(owner, time, train_no);
                    CallbackArgs callback = new CallbackArgs(dlgt, target, new object[] { "详情", xx });
                    string columnKey;
                    GridUtil.AddLinkColumn(outlookGrid1, "详情", out columnKey, callback);

                    lbl_Time.Text = dt.Rows[0]["flash_time"].ToString();
                    lbl_TrainNo.Text = dt.Rows[0]["train_no"].ToString(); ;
                    lbl_Station.Text = dt.Rows[0]["station_name"].ToString();
                    lbl_Direction.Text = dt.Rows[0]["direction"].ToString();
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            data_report data = new data_report(owner, time, train_no);
            owner.AddTabPage("监测数据报表", data);
        }
    }
}
