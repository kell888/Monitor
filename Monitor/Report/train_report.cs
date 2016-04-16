using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MergeQueryUtil;

namespace Monitor.Report
{
    public partial class train_report : MonitorForm
    {
        public train_report(Index owner, DateTime starttime, DateTime endtime)
            : base(owner, starttime, endtime)
        {
            InitializeComponent();
            this.starttime = starttime;
            this.endtime = endtime;
            owner.ShowInfo("过车信息报表");
        }
        DateTime starttime;
        DateTime endtime;

        private void print_Load(object sender, EventArgs e)
        {
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                DataTable dt = sqlHelper.ExecuteQueryDataTable("select * from v_train_log where come_time between '" + starttime + "' and '" + endtime + "' order by come_time");//MergeQuery.GetDataRange("v_train_log", "*", "come_time", starttime, endtime, null, "come_time");
                this.reportViewer1.ProcessingMode = Microsoft.Reporting.WinForms.ProcessingMode.Local;
                this.reportViewer1.LocalReport.DisplayName = "过车信息";
                this.reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WinForms.ReportParameter("train_no", "车号"));
                this.reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WinForms.ReportParameter("direction", "方向"));
                this.reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WinForms.ReportParameter("station_name", "监测站"));
                this.reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WinForms.ReportParameter("come_time", "来车时间"));
                this.reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WinForms.ReportParameter("go_time", "去车时间"));
                this.reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WinForms.ReportParameter("alarm_count", "报警数"));
                this.reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WinForms.ReportParameter("alarm_status", "报警状态"));
                Microsoft.Reporting.WinForms.ReportDataSource item = new Microsoft.Reporting.WinForms.ReportDataSource("DataSet1", dt);
                this.reportViewer1.LocalReport.DataSources.Add(item);
                this.reportViewer1.LocalReport.Refresh();
            }
        }
    }
}
