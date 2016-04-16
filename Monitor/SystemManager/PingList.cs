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
    public partial class PingList : MonitorForm
    {
        public PingList(Index owner)
            : base(owner)
        {
            InitializeComponent();
            winFormPager1.PageSize = 20;
            owner.ShowInfo("通讯自检日志");
        }

        DataTable dt;

        private void PingList_Load(object sender, EventArgs e)
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
            DataTable dtt = null;
            int allCount = 0;
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                dtt = sqlHelper.ExecuteQueryDataTable("select * from d_ping_log order by pingtime desc");
            }
            if (dtt != null)
                allCount = dtt.Rows.Count;
            Dictionary<string, string> cols = new Dictionary<string, string>();
            cols.Add("ip", "服务器IP");
            cols.Add("onOff", "是否断网");
            cols.Add("pingtime", "时间");
            dt = GridUtil.ViewData(dtt, cols);
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
