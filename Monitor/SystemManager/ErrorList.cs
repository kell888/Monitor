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

namespace Monitor.SystemManager
{
    public partial class ErrorList : MonitorForm
    {
        public ErrorList(Index owner)
            : base(owner)
        {
            InitializeComponent();
            winFormPager1.PageSize = 20;
            owner.ShowInfo("错误日志");
        }

        DataTable dt;

        private void ErrorList_Load(object sender, EventArgs e)
        {
            winFormPager1.PageIndex = 1;
            SearchOrders();
            int pageCount;
            string msg;
            DataView dv = Paging.GetPagerForView(dt, winFormPager1.PageSize, winFormPager1.PageIndex, out pageCount, out msg);
            GridUtil.BindData(outlookGrid1, dv.Table);
        }

        void SearchOrders()
        {
            //DataTable dtt = null;
            int allCount = 0;
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                dt = sqlHelper.ExecuteQueryDataTable("select * from v_error_log order by 发生时间 desc");
            }
            if (dt != null)
                allCount = dt.Rows.Count;
            //Dictionary<string, string> cols = new Dictionary<string, string>();
            //cols.Add("id", "ID");
            //cols.Add("err_level", "错误级别");
            //cols.Add("err_source", "错误来源");
            //cols.Add("err_msg", "错误信息");
            //cols.Add("err_client", "客户端");
            //cols.Add("err_time", "发送时间");
            //dt = GridUtil.ViewData(dtt, cols);
            winFormPager1.RecordCount = allCount;
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
