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
using Monitor.Analysis;

namespace Monitor.Report
{
    public partial class XxReport : MonitorForm
    {
        public XxReport(Index owner, DateTime time, string train_no)
            : base(owner, time, train_no)
        {
            InitializeComponent();
            this.owner = owner;
            this.time = time;
            this.train_no = train_no;
            owner.ShowInfo("监测数据明细");
        }

        Index owner;
        DateTime time;
        string train_no;

        void XxReport_Load(object sender, System.EventArgs e)
        {
            RefreshList();
        }

        public void ShowForm(Form f)
        {
            if (f.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (this.InvokeRequired)
                    this.Invoke(new MethodInvoker(delegate { this.RefreshList(); }));
                else
                    this.RefreshList();
            }
        }

        private void RefreshList()
        {
            int train_log_id = TrainInfo.GetTrainIdByNo(train_no);
            this.Text = "监测数据明细 - 列车号：" + train_no;
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                DataTable dt = sqlHelper.ExecuteQueryDataTable("select * from v_data_log where flash_time='" + time + "' and train_no='" + train_no + "' order by device_name, point_type_name");//MergeQuery.GetDataAt("v_data_log", "*", "flash_time", time, "train_no='" + train_no + "'", "device_name, point_type_name");
                Dictionary<string, string> cols = new Dictionary<string, string>();
                cols.Add("id", "ID");
                cols.Add("flash_time", "监测时间");
                cols.Add("device_name", "设备名称");
                cols.Add("point_type_name", "监测项目");
                cols.Add("data_value", "检测值");
                cols.Add("status_type_name", "检测状态");
                DataTable d = GridUtil.ViewData(dt, cols);
                GridUtil.BindData(outlookGrid1, d);
                if (d.Rows.Count > 0)
                {
                    Delegate dlgt = new Action<string, Form>(owner.AddTabPage);
                    object target = owner;
                    CallbackArgs callback00 = new CallbackArgs(dlgt, target, new object[] { "碳滑板分析", "$THB|{OWNER},{ID}$" });
                    CallbackArgs callback01 = new CallbackArgs(dlgt, target, new object[] { "羊角分析", "$YJ|{OWNER},{ID}$" });
                    CallbackArgs callback02 = new CallbackArgs(dlgt, target, new object[] { "磨耗缺口分析", "$MHQK|{OWNER},{ID}$" });
                    CallbackArgs callback03 = new CallbackArgs(dlgt, target, new object[] { "球铰分析", "$QJ|{OWNER},{ID}$" });
                    string columnKey00;
                    GridUtil.AddLinkColumn(outlookGrid1, "碳滑板分析", out columnKey00, callback00);
                    string columnKey01;
                    GridUtil.AddLinkColumn(outlookGrid1, "查羊角分析", out columnKey01, callback01);
                    string columnKey02;
                    GridUtil.AddLinkColumn(outlookGrid1, "磨耗缺口分析", out columnKey02, callback02);
                    string columnKey03;
                    GridUtil.AddLinkColumn(outlookGrid1, "球铰分析", out columnKey03, callback03);
                    image img = new image(owner, train_log_id);
                    CallbackArgs callback1 = new CallbackArgs(dlgt, target, new object[] { "图片浏览", img });
                    string columnKey1;
                    GridUtil.AddLinkColumn(outlookGrid1, "查看影像", out columnKey1, callback1);
                    video vid = new video(owner, train_log_id);
                    CallbackArgs callback2 = new CallbackArgs(dlgt, target, new object[] { "视频浏览", vid });
                    string columnKey2;
                    GridUtil.AddLinkColumn(outlookGrid1, "查看视频", out columnKey2, callback2);
                    CallbackArgs callback3 = new CallbackArgs(new Action<Form>(ShowForm), this, new object[] { "$Determine|{FLASHTIME},{DEVICENAME}$" });
                    string columnKey3 = "确认报警";
                    GridUtil.AddLinkColumn(outlookGrid1, "确认报警", columnKey3, callback3);
                    CallbackArgs callback4 = new CallbackArgs(new Action<Form>(ShowForm), this, new object[] { "$Processing|{FLASHTIME},{DEVICENAME}$" });
                    string columnKey4 = "处理报警";
                    GridUtil.AddLinkColumn(outlookGrid1, "处理报警", columnKey4, callback4);
                }
            }
        }
    }
}
