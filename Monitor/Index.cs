using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Echevil;
using MergeQueryUtil;
using Monitor.App_Code;
using Monitor.Report;
using Monitor.SystemManager;
using Monitor.Touble;

namespace Monitor
{
    public partial class Index : Form
    {
        const int CLOSE_SIZE = 12;
        private bool sure;
        NetworkMonitor netMonitor;
        System.Timers.Timer timer2;
        //ChangeNotification change;
        NewDataNotification notifier;
        public static UserInfo User;
        int menuWidth;

        #region
        public void AddTabPage(string str, Form form)
        {
            ThreadPool.QueueUserWorkItem(delegate
            {
                ShowStatus("正在拼命加载中，请稍候...");
                try
                {
                    AddTabForm(str, form);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    ShowStatus("Ready...");
                }
            });
        }

        private void AddTabForm(string str, Form form)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<string, Form>(AddTabForm), str, form);
            }
            else
            {
                int have = TabControlCheckHave(this.tabControl1, form.GetType().FullName);
                if (have > -1)
                {
                    tabControl1.SelectTab(have);
                    tabControl1.SelectedTab.Text = str;
                    tabControl1.SelectedTab.Controls.Clear();
                    form.TopLevel = false;//设置非顶级窗口
                    form.Parent = tabControl1.SelectedTab;
                    form.WindowState = FormWindowState.Maximized;
                    form.FormBorderStyle = FormBorderStyle.None;
                    form.Show();
                }
                else
                {
                    tabControl1.TabPages.Add(str);
                    tabControl1.SelectTab(tabControl1.TabPages.Count - 1);
                    string classFullname = form.GetType().FullName;
                    tabControl1.SelectedTab.Tag = classFullname;
                    int index = classFullname.LastIndexOf(".");
                    string className = classFullname;
                    if (index > -1)
                        className = classFullname.Substring(index + 1);
                    tabControl1.SelectedTab.Name = "Tab_" + className;
                    form.TopLevel = false;//设置非顶级窗口
                    form.Parent = tabControl1.SelectedTab;
                    form.WindowState = FormWindowState.Maximized;
                    form.FormBorderStyle = FormBorderStyle.None;
                    form.Show();
                }
            }
        }

        public void CloseSubForm(TabPage page)
        {
            string name = page.Name.Substring(4);
            Control[] cs = page.Controls.Find(name, false);
            if (cs != null && cs.Length > 0)
            {
                if (cs[0] is Form)
                {
                    Form f = cs[0] as Form;
                    f.Close();
                }
            }
            this.tabControl1.TabPages.Remove(page);
        }

        //判断TabPage是否已创建
        private int TabControlCheckHave(TabControl tab, string tabPage)
        {
            int index = -1;
            for (int i = 0; i < tab.TabPages.Count; i++)
            {
                TabPage tp = tab.TabPages[i];
                if (tp.Tag.ToString() == tabPage)
                {
                    index = i;
                    break;
                }
            }
            return index;
        }
        #endregion

        internal void SureExit()
        {
            SafeTimer.Dispose();
            //if (change != null)
            //{
            //    if (change.IsStart) change.Stop();
            //}
            if (notifier != null)
            {
                if (notifier.IsStart) notifier.Stop();
            }
            notifyIcon1.Dispose();
            Environment.Exit(0);
        }

        void timer2_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            MethodInvoker mi = new MethodInvoker(RefreshNetworkInfo);
            mi.BeginInvoke(null, null);
        }

        private void RefreshNetworkInfo()
        {
            if (netMonitor.Adapters.Length > 0)
            {
                StringBuilder sb = new StringBuilder();
                int i = 0;
                foreach (NetworkAdapter adp in netMonitor.Adapters)
                {
                    i++;
                    string netAvanda = "[网卡(" + i.ToString() + "): " + Math.Round(adp.DownloadSpeedKbps, 2) + "Kbps/" + Math.Round(adp.UploadSpeedKbps, 2) + "Kbps]";
                    if (sb.Length == 0)
                        sb.Append(netAvanda);
                    else
                        sb.Append(" " + netAvanda);
                }
                Action<string> a = new Action<string>(RefreshNet);
                a.BeginInvoke(sb.ToString(), null, null);
            }
        }
        private void RefreshNet(string info)
        {
            if (statusStrip1.InvokeRequired)
            {
                statusStrip1.BeginInvoke(new Action<string>(a =>
                {
                    netInfo.Text = a;
                    statusStrip1.Refresh();
                }), info);
            }
            else
            {
                netInfo.Text = info;
                statusStrip1.Refresh();
            }
        }

        static Index instance;

        public static Index Instance
        {
            get { return Index.instance; }
        }

        public static Index GetInstance()
        {
            if (instance == null || instance.IsDisposed)
            {
                instance = new Index();
            }
            if (!instance.Visible)
                instance.Show();
            instance.WindowState = FormWindowState.Maximized;
            instance.BringToFront();
            instance.Focus();
            return instance;
        }

        private Index()
        {
            InitializeComponent();
            this.tabControl1.DrawMode = TabDrawMode.OwnerDrawFixed;//由自己绘制标题
            this.tabControl1.Padding = new System.Drawing.Point(CLOSE_SIZE + 2, 2);
            this.tabControl1.DrawItem += new DrawItemEventHandler(this.tabControl1_DrawItem);
            this.tabControl1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tabControl1_MouseDown);
            menuWidth = treeView1.Width;
            User = new UserInfo();
            Logs.Init();
            //bool start = SqlNotification.ChangeNotification.Start(null);
            //Logs.Info("SqlDependency " + (start ? "Start!" : "No Start!"));
            string ids = ConfigurationManager.AppSettings["IPAddrs"];
            string inter = ConfigurationManager.AppSettings["Inter"];
            string timeo = ConfigurationManager.AppSettings["Timeout"];
            int interval = 30000;
            int RET;
            if (int.TryParse(inter, out RET))
            {
                interval = RET;
            }
            int timeout = 10000;
            if (int.TryParse(timeo, out RET))
            {
                timeout = RET;
            }
            SafeTimer.Checking += new EventHandler<IntEventArgs>(SafeTimer_Checking);
            SafeTimer.Init(interval, timeout, ids);
        }

        void SafeTimer_Checking(object sender, IntEventArgs e)
        {
            CheckTerminalIsLink(SafeTimer.State, e.Interval, e.Timeout);
        }

        void CheckTerminalIsLink(object ips, int interval, int timeout)
        {
            if (ips != null)
            {
                System.Net.NetworkInformation.Ping ping = new System.Net.NetworkInformation.Ping();
                string ipAddrs = ips.ToString();
                string[] ipss = ipAddrs.Split('|');
                foreach (string ipAddr in ipss)
                {
                    System.Net.NetworkInformation.PingReply pr = ping.Send(ipAddr, timeout > interval ? interval : timeout);
                    bool off = pr.Status != System.Net.NetworkInformation.IPStatus.Success;
                    if (off)
                        ShowStatus("与服务器的连接已断开！");
                    else
                        ShowStatus("Ready...");
                    int status = Common.LastOnOff(ipAddr, off);
                    if (status < 2)
                    {
                        try
                        {
                            using (SqlHelper sqlHelper = new SqlHelper())
                            {
                                sqlHelper.ExecuteNonQuery("insert into d_ping_log (ip,onOff) values ('" + ipAddr + "'," + (off ? "1" : "0") + ")");
                            }
                        }
                        catch (Exception ex)
                        {
                            StackTrace st = new StackTrace(true);
                            StackFrame sf = st.GetFrame(0);
                            string fileName = sf.GetFileName();
                            Type type = sf.GetMethod().ReflectedType;
                            string assName = type.Assembly.FullName;
                            string typeName = type.FullName;
                            string methodName = sf.GetMethod().Name;
                            int lineNo = sf.GetFileLineNumber();
                            int colNo = sf.GetFileColumnNumber();
                            Logs.LogError(ErrorLevel.Normal, fileName + " : " + assName + "." + typeName + "." + methodName + "(" + lineNo + "行" + colNo + "列)", ex.Message);
                            Logs.Error("记录网络状况到数据库时失败：" + ex.Message);
                        }
                    }
                    //if (pr.Status != System.Net.NetworkInformation.IPStatus.Success)
                    //{
                    //Logs.Error("工作站[" + ipAddr + "]与服务器的网络连接已断开！");
                    //System.IO.File.AppendAllText(AppDomain.CurrentDomain.BaseDirectory + "CheckTerminalIsLink.txt", "-------" + DateTime.Now + "-------" + Environment.NewLine + "Ping [" + ipAddr + "] failture!" + Environment.NewLine + Environment.NewLine);
                    //if (HttpContext.Current != null)
                    //    HttpContext.Current.MessageBox.Show("工作站[" + ipAddr + @"]跟服务器的网络通讯已断开')" + CryptUtil.DecodeDes("븎葖ǑΌᾦ樰꯻㜎쭖䦔꽪"));
                    //GoToLogin.Alert("工作站[" + ipAddr + "]跟服务器的网络通讯已断开");
                    //}
                    //System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo();
                    //psi.FileName="cmd.exe";
                    //psi.Arguments = "ping " + ipAddr;
                    //psi.UseShellExecute = false;
                    //psi.CreateNoWindow = true;
                    //psi.RedirectStandardInput = true;
                    //psi.RedirectStandardOutput = true;
                    //psi.RedirectStandardError = true;
                    //System.Diagnostics.Process proc = System.Diagnostics.Process.Start(psi);
                    //proc.WaitForExit();
                }
                ping.Dispose();
            }
        }

        private void index_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (sure)
            {
                if (MessageBox.Show("确定要退出系统么？", "退出提醒", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    if (netMonitor.Adapters.Length > 0)
                        netMonitor.StopMonitoring();
                    if (timer2.Enabled)
                        timer2.Stop();
                    SureExit();
                }
                else
                {
                    sure = false;
                    e.Cancel = true;
                }
            }
            else
            {
                e.Cancel = true;
                this.Hide();
            }
        }

        private void index_Load(object sender, EventArgs e)
        {
            netMonitor = new NetworkMonitor();
            if (netMonitor.Adapters.Length > 0)
                netMonitor.StartMonitoring();
            timer2 = new System.Timers.Timer(1000);
            timer2.AutoReset = true;
            timer2.Elapsed += new System.Timers.ElapsedEventHandler(timer2_Elapsed);
            timer2.Start();
            ShowStatus("正在初始化系统...");
            Login login = new Login();
            if (login.ShowDialog() == DialogResult.OK)
            {
                User.ID = login.UserId;
                User.Login_name = login.UserName;
                User.Password = login.Password;
                User.Operation = login.Operater;
                userLabel.Text = "当前用户：" + login.UserName;
                BindMenu();
                if (login.UserId.ToString() == Common.AdminId)
                {
                    MenuManage mm = new MenuManage(instance);
                    AddTabPage("菜单管理", mm);
                }
                //change = new ChangeNotification(ConfigurationManager.ConnectionStrings["MonitoringSystem"].ConnectionString);
                //change.DataChanged += Change_DataChanged;
                //string simpleSQL = "select id,train_no,[address],start_time,direction,status_type_name from v_alarm_log";
                //if (!string.IsNullOrEmpty(simpleSQL))
                //{
                //    change.ChangeEvent(simpleSQL);
                //    change.Start();
                //}
                notifier = new NewDataNotification(int.Parse(ConfigurationManager.AppSettings["Timeout"]), ConfigurationManager.ConnectionStrings["MonitoringSystem"].ConnectionString, "v_alarm_log", "id", null);
                notifier.NewDataComing += Notifier_NewDataComing;
                notifier.Start();
                ShowStatus("Ready...");
            }
            else
            {
                SureExit();
            }
        }

        private void Notifier_NewDataComing(NewDataNotification sender, NewDataArgs e)
        {
            List<decimal> ids = e.NewIds;
            if (ids != null && ids.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (decimal id in ids)
                {
                    if (sb.Length == 0)
                        sb.Append(id.ToString());
                    else
                        sb.Append("," + id.ToString());
                }
                using (SqlHelper sqlHelper = new SqlHelper())
                {
                    DataTable dt = sqlHelper.ExecuteQueryDataTable("select id,train_no,[address],start_time,direction,status_type_name from v_alarm_log where id in (" + sb.ToString() + ")");
                    foreach (DataRow row in dt.Rows)
                    {
                        int id = Convert.ToInt32(row["id"]);
                        string train_no = row["train_no"].ToString();
                        string address = row["address"].ToString();
                        DateTime start_time = Convert.ToDateTime(row["start_time"]);
                        bool direction = Convert.ToBoolean(row["direction"]);
                        string status_type_name = row["status_type_name"].ToString();
                        Color statusColor = Color.Green;
                        if (direction)
                            statusColor = Color.Blue;
                        string info = "车号：" + train_no + "  报警地点：" + address + "  报警状态：" + status_type_name + "  报警时间：" + start_time.ToString("MM-dd HH:mm:ss");
                        SdgReport sr = new SdgReport(instance, start_time, train_no);
                        KellScrollList.LinkObject link = new KellScrollList.LinkObject("报警信息", true, new Action<string, Form>(AddTabPage), this, new object[] { "未处理报警", sr });
                        ShowNewInfo(info, link, statusColor);
                    }
                }
            }
        }

        //private void Change_DataChanged(object sender, DependencyArgs e)
        //{
        //    if (e.Source == SqlNotificationSource.Data && e.Type == SqlNotificationType.Change && e.Info == SqlNotificationInfo.Insert)
        //    {
        //        DataSet ds = e.TriggerResult;
        //        if (ds != null && ds.Tables.Count > 0)
        //        { 
        //            DataTable dt = ds.Tables[0];
        //            DataView dv = new DataView(dt);
        //            dv.Sort = "start_time asc";
        //            foreach (DataRowView row in dv)
        //            {
        //                int id = Convert.ToInt32(row["id"]);
        //                string train_no = row["train_no"].ToString();
        //                string address = row["address"].ToString();
        //                DateTime start_time = Convert.ToDateTime(row["start_time"]);
        //                bool direction = Convert.ToBoolean(row["direction"]);
        //                string status_type_name = row["status_type_name"].ToString();
        //                Color statusColor = Color.Green;
        //                if (direction)
        //                    statusColor = Color.Blue;
        //                string info = "车号：" + train_no + "  报警地点：" + address + "  报警状态：" + status_type_name + "  报警时间：" + start_time.ToString("MM-dd HH:mm:ss");
        //                SearchTroubleW tr = new SearchTroubleW(instance, id);
        //                KellScrollList.LinkObject link = new KellScrollList.LinkObject("报警信息", true, new Action<string, Form>(AddTabPage), this, new object[] { "未处理报警", tr });
        //                ShowNewInfo(info, link, statusColor);
        //            }
        //        }
        //    }
        //}

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            sure = true;
            this.Close();
        }

        private void 显示ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowUI();
        }

        private void ShowUI()
        {
            this.Show();
            this.BringToFront();
            this.Focus();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ShowUI();
        }

        private void panel4_MouseClick(object sender, MouseEventArgs e)
        {
            ShowOrHideMenuBar();
        }

        private void ShowOrHideMenuBar()
        {
            bool hide = menuPanel.Width == panel4.Width;
            if (hide)//open
            {
                menuPanel.Width = menuWidth + panel4.Width;
                panel4.BackgroundImage = Monitor.Properties.Resources.close;
            }
            else//close
            {
                menuPanel.Width = panel4.Width;
                panel4.BackgroundImage = Monitor.Properties.Resources.open;
            }
        }

        public void ShowInfo(string msg)
        {
            if (infoLabel.InvokeRequired)
            {
                infoLabel.BeginInvoke(new MethodInvoker(delegate
                {
                    infoLabel.Text = msg;
                    infoLabel.Refresh();
                }));
            }
            else
            {
                infoLabel.Text = msg;
                infoLabel.Refresh();
            }
        }

        public void ShowStatus(string msg)
        {
            if (statusStrip1.InvokeRequired)
            {
                statusStrip1.BeginInvoke(new MethodInvoker(delegate
                {
                    statusLabel.Text = msg;
                    statusStrip1.Refresh();
                }));
            }
            else
            {
                statusLabel.Text = msg;
                statusStrip1.Refresh();
            }
        }
        private List<int> GetMenuIdList(int operation)
        {
            List<int> idList = new List<int>();
            using (SqlHelper sqlHelper = new SqlHelper())
            {
                using (DbDataReader reader = sqlHelper.ExecuteQueryReader("select menuid from a_role where operation=" + operation))
                {
                    if (reader.Read())
                    {
                        string menuid = Convert.IsDBNull(reader[0]) ? "" : reader.GetString(0);
                        string[] menuids = menuid.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        int id;
                        foreach (string menu in menuids)
                        {
                            if (int.TryParse(menu, out id))
                            {
                                idList.Add(id);
                            }
                        }
                    }
                }
            }
            return idList;
        }
        /// <summary>
        /// 加载主菜单
        /// </summary>
        internal void BindMenu()
        {
            treeView1.Nodes.Clear();
            if (User.Operation > 0)
            {
                List<int> idList = GetMenuIdList(User.Operation);
                DataTable dt = null;
                if (User.ID.ToString() == Common.AdminId)
                    dt = MenuUtil.GetMenus();
                else
                    dt = MenuUtil.GetEnabledMenus();
                DataView dv = new DataView(dt);
                dv.RowFilter = "ParentID=0";
                foreach (DataRowView drv in dv)
                {
                    int menuid = Convert.ToInt32(drv["ID"]);
                    if (idList.Contains(menuid))
                    {
                        MenuInfo mi = MenuUtil.GetMenu(menuid);
                        string text = drv["Name"].ToString();//显示的文本
                        TreeNode node = new TreeNode();
                        node.ToolTipText = text;
                        node.Text = text;
                        node.Tag = mi;
                        node.Expand();
                        treeView1.Nodes.Add(node);//添加到根节点
                        AddChildren(node, dt);
                    }
                }
            }
        }

        /// <summary>
        /// 加载子菜单
        /// </summary>
        /// <param name="node"></param>
        /// <param name="dt"></param>
        private void AddChildren(TreeNode node, DataTable dt = null)
        {
            node.Nodes.Clear();
            if (User.Operation > 0)
            {
                List<int> idList = GetMenuIdList(User.Operation);
                if (dt == null)
                {
                    if (User.ID.ToString() == Common.AdminId)
                        dt = MenuUtil.GetMenus();
                    else
                        dt = MenuUtil.GetEnabledMenus();
                }
                DataView dv = new DataView(dt);
                dv.RowFilter = "ParentID=" + (node.Tag as MenuInfo).ID + "";
                if (dv.Count > 0)
                {
                    foreach (DataRowView row in dv)
                    {
                        int menuid = Convert.ToInt32(row["ID"]);
                        if (idList.Contains(menuid))
                        {
                            MenuInfo mi = MenuUtil.GetMenu(menuid);
                            string text = row["Name"].ToString();//显示的文本
                            TreeNode replyNode = new TreeNode();//新建一个节点
                            replyNode.ToolTipText = text;
                            replyNode.Text = text;
                            replyNode.Tag = mi;
                            List<int> idList2 = GetMenuIdList(User.Operation);
                            if (dt == null)
                            {
                                if (User.ID.ToString() == Common.AdminId)
                                    dt = MenuUtil.GetMenus();
                                else
                                    dt = MenuUtil.GetEnabledMenus();
                            }
                            DataView dv2 = new DataView(dt);
                            dv2.RowFilter = "ParentID=" + mi.ID + "";
                            if (dv2.Count == 0)
                            {
                                replyNode.ImageIndex = 2;
                            }
                            node.Nodes.Add(replyNode);//添加到子节点
                        }
                    }
                }
                else
                {
                    node.ImageIndex = 2;
                }
            }
        }

        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            try
            {
                Rectangle myTabRect = this.tabControl1.GetTabRect(e.Index);
                e.Graphics.FillRectangle(new SolidBrush(SystemColors.Control), myTabRect);

                //先添加TabPage属性   
                e.Graphics.DrawString(this.tabControl1.TabPages[e.Index].Text
                , this.Font, SystemBrushes.ControlText, myTabRect.X + 2, myTabRect.Y + 2);


                //再画一个矩形框
                using (Pen p = new Pen(Color.Black))
                {
                    myTabRect.Offset(myTabRect.Width - (CLOSE_SIZE + 3), 2);
                    myTabRect.Width = CLOSE_SIZE;
                    myTabRect.Height = CLOSE_SIZE;
                    e.Graphics.DrawRectangle(p, myTabRect);
                }
                //填充矩形框
                Color recColor = e.State == DrawItemState.Selected ? Color.MediumVioletRed : Color.DarkGray;
                using (Brush b = new SolidBrush(recColor))
                {
                    e.Graphics.FillRectangle(b, myTabRect);
                }

                //画关闭符号
                using (Pen p = new Pen(Color.White))
                {
                    //画"\"线
                    Point p1 = new Point(myTabRect.X + 3, myTabRect.Y + 3);
                    Point p2 = new Point(myTabRect.X + myTabRect.Width - 3, myTabRect.Y + myTabRect.Height - 3);
                    e.Graphics.DrawLine(p, p1, p2);

                    //画"/"线
                    Point p3 = new Point(myTabRect.X + 3, myTabRect.Y + myTabRect.Height - 3);
                    Point p4 = new Point(myTabRect.X + myTabRect.Width - 3, myTabRect.Y + 3);
                    e.Graphics.DrawLine(p, p3, p4);
                }
                e.Graphics.Dispose();
            }
            catch (Exception ex)
            {
                StackTrace st = new StackTrace(true);
                StackFrame sf = st.GetFrame(0);
                string fileName = sf.GetFileName();
                Type type = sf.GetMethod().ReflectedType;
                string assName = type.Assembly.FullName;
                string typeName = type.FullName;
                string methodName = sf.GetMethod().Name;
                int lineNo = sf.GetFileLineNumber();
                int colNo = sf.GetFileColumnNumber();
                Logs.LogError(ErrorLevel.Normal, fileName + " : " + assName + "." + typeName + "." + methodName + "(" + lineNo + "行" + colNo + "列)", ex.Message);
            }
        }

        private void tabControl1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int x = e.X, y = e.Y;

                //计算关闭区域   
                Rectangle myTabRect = this.tabControl1.GetTabRect(this.tabControl1.SelectedIndex);

                myTabRect.Offset(myTabRect.Width - (CLOSE_SIZE + 3), 2);
                myTabRect.Width = CLOSE_SIZE;
                myTabRect.Height = CLOSE_SIZE;

                //如果鼠标在区域内就关闭选项卡   
                bool isClose = x > myTabRect.X && x < myTabRect.Right
                 && y > myTabRect.Y && y < myTabRect.Bottom;

                if (isClose)
                {
                    if (this.tabControl1.SelectedTab.Name.Substring(4) != "MenuManage")
                        CloseSubForm(this.tabControl1.SelectedTab);
                }
                else
                {
                    ShowInfo(this.tabControl1.SelectedTab.Text);
                }
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            MenuInfo menu = e.Node.Tag as MenuInfo;
            string text = menu.Name;
            Dictionary<string, object> param = new Dictionary<string, object>();//保存本系统可能用到的所有参数类型
            //...Start...
            param.Add("{OWNER}", instance);
            param.Add("{TARGET}", instance);
            param.Add("{USERNAME}", User.Login_name);
            param.Add("{PWD}", User.Password);
            param.Add("{ID}", menu.ID);
            DateTime now = DateTime.Now;
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
            DateTime start = now.AddDays(-last);
            param.Add("{STARTTIME}", start.ToString());
            param.Add("{ENDTIME}", now.ToString());
            param.Add("{FLASHTIME}", null);
            param.Add("{DEVICENAME}", null);
            param.Add("{TRAINNO}", null);
            //...End...
            Form form = Common.GetForm(menu, param);
            if (form == null)
            {
                if (!string.IsNullOrEmpty(menu.ReferenceForm))
                {
                    if (!string.IsNullOrEmpty(menu.AssemblyFile))
                        MessageBox.Show("找不到该菜单对应的窗体！[" + menu.AssemblyFile + ":" + menu.ReferenceForm + "]");
                    else
                        MessageBox.Show("找不到该菜单对应的窗体！[" + menu.ReferenceForm + "]");
                }
            }
            else
            {
                AddTabPage(text, form);
            }
        }

        private void treeView1_AfterExpand(object sender, TreeViewEventArgs e)
        {
            e.Node.ImageIndex = 1;
            AddChildren(e.Node);
        }

        private void treeView1_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            e.Node.ImageIndex = 0;
        }

        public void ShowNewInfo(string info, KellScrollList.LinkObject link, Color statusColor)
        {
            if (infoListControl1.InvokeRequired)
            {
                infoListControl1.BeginInvoke(new MethodInvoker(
                    delegate
                    {
                        infoListControl1.AddInfo(info, link, statusColor);
                        infoListControl1.Refresh();
                    }));
            }
            else
            {
                infoListControl1.AddInfo(info, link, statusColor);
                infoListControl1.Refresh();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            sure = true;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Process.Start("help\\hepl.htm");
        }
    }
}

namespace System.Runtime.CompilerServices
{
    public class ExtensionAttribute : Attribute { }
}