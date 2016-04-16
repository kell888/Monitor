using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MergeQueryUtil;
using Monitor.App_Code;

namespace Monitor.SystemManager
{
    public partial class RoleManage : Form
    {
        public RoleManage(Index owner)
        {
            InitializeComponent();
            this.owner = owner;
            owner.ShowInfo("配置权限");
        }
        Index owner;

        private void MenuManage_Load(object sender, EventArgs e)
        {
            LoadAllMenus();
        }

        private void LoadAllMenus()
        {
            List<MeniInfo> data = MenuUtil.GetMeniInfoList();
            checkedListBox1.Items.Clear();
            foreach (MeniInfo mi in data)
            {
                checkedListBox1.Items.Add(mi, false);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                int operation = 1;
                switch (comboBox1.SelectedIndex)
                {
                    case 0:
                        operation = 3;
                        break;
                    case 1:
                        operation = 2;
                        break;
                    case 2:
                    default:
                        operation = 1;
                        break;
                }
                LoadInfo(operation);
            }
        }

        private void LoadInfo(int operation)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
            {
                checkedListBox1.SetItemChecked(i, false);
            }
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
                                for (int i = 0; i < checkedListBox1.Items.Count; i++)
                                {
                                    MeniInfo mi = checkedListBox1.Items[i] as MeniInfo;
                                    if (mi != null && mi.ID == id)
                                        checkedListBox1.SetItemChecked(i, true);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                int operation = 1;
                switch (comboBox1.SelectedIndex)
                {
                    case 0:
                        operation = 3;
                        break;
                    case 1:
                        operation = 2;
                        break;
                    case 2:
                    default:
                        operation = 1;
                        break;
                }
                StringBuilder sb = new StringBuilder();
                for (int i=0; i< checkedListBox1.Items.Count; i++)
                {
                    if (checkedListBox1.GetItemChecked(i))
                    {
                        MeniInfo mi = checkedListBox1.Items[i] as MeniInfo;
                        if (mi != null)
                        {
                            if (sb.Length == 0)
                                sb.Append(mi.ID);
                            else
                                sb.Append("," + mi.ID);
                        }
                    }
                }
                string menuid = sb.ToString();
                try
                {
                    using (SqlHelper sqlHelper = new SqlHelper())
                    {
                        int r = sqlHelper.ExecuteNonQuery("if exists (select 1 from a_role where operation=" + operation + ") update a_role set menuid='" + menuid + "' where operation=" + operation + " else insert into a_role (operation, menuid) values (" + operation + ", '" + menuid + "')");
                        MessageBox.Show("保存权限成功！");
                        owner.BindMenu();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("保存权限失败：" + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("先选定要修改的权限！");
            }
        }
    }
}
