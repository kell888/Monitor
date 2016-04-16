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
    public partial class LoginList : MonitorForm
    {
        public LoginList(Index owner)
            : base(owner)
        {
            InitializeComponent();
            winFormPager1.PageSize = 20;
            owner.ShowInfo("用户登录日志");
        }

        DataTable dt;

        private void LoginList_Load(object sender, EventArgs e)
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
                dtt = sqlHelper.ExecuteQueryDataTable("select * from d_login_log order by N_UserTime desc");
            }
            if (dtt != null)
                allCount = dtt.Rows.Count;
            Dictionary<string, string> cols = new Dictionary<string, string>();
            cols.Add("N_User", "用户名");
            cols.Add("N_IP", "IP地址");
            cols.Add("N_Operator", "角色");
            cols.Add("N_UserTime", "登录时间");
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
