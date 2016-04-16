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
    public partial class data_report : MonitorForm
    {
        public data_report(Index owner, DateTime time, string train_no)
            : base(owner, time, train_no)
        {
            InitializeComponent();
            this.time = time;
            this.train_no = train_no;
            owner.ShowInfo("监测数据报表");
        }

        DateTime time;
        string train_no;

        private void print_Load(object sender, EventArgs e)
        {
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                DataTable dt = sqlHelper.ExecuteQueryDataTable("select * from v_data_log where flash_time='" + time + "' train_no='" + train_no + "' order by device_name,point_type_name");//MergeQuery.GetDataAt("v_data_log", "*", "flash_time", time, "train_no='" + train_no + "'", "device_name, point_type_name");
                this.reportViewer1.ProcessingMode = Microsoft.Reporting.WinForms.ProcessingMode.Local;
                this.reportViewer1.LocalReport.DisplayName = "监测数据";
                this.reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WinForms.ReportParameter("device_name", "设备名称"));
                this.reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WinForms.ReportParameter("point_type_name", "监测类型"));
                this.reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WinForms.ReportParameter("station_name", "监测站"));
                this.reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WinForms.ReportParameter("data_value", "监测值"));
                this.reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WinForms.ReportParameter("flash_time", "监测时间"));
                this.reportViewer1.LocalReport.SetParameters(new Microsoft.Reporting.WinForms.ReportParameter("alarm_status", "报警状态"));
                Microsoft.Reporting.WinForms.ReportDataSource item = new Microsoft.Reporting.WinForms.ReportDataSource("DataSet2", dt);
                this.reportViewer1.LocalReport.DataSources.Add(item);
                this.reportViewer1.LocalReport.Refresh();
            }
        }
    }
}
