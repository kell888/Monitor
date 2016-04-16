using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Monitor.App_Code;

namespace Monitor.SystemManager
{
    public partial class MenuManage : Form
    {
        Index owner;
        public MenuManage(Index owner)
        {
            InitializeComponent();
            this.owner = owner;
            owner.ShowInfo("菜单管理");
        }

        private void EditForm()
        {
            using (SelectControlForm smf = new SelectControlForm())
            {
                if (smf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    textBox2.Text = smf.FormName;
                    textBox3.Text = smf.AssemblyFile;
                }
            }
        }

        private void MenuManage_Load(object sender, EventArgs e)
        {
            RefreshMenuInfo();
        }

        private void RefreshMenuInfo()
        {
            List<MenuInfo> data = MenuUtil.GetMenuList();
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            comboBox2.Items.Add("-- 无 --");
            foreach (MenuInfo d in data)
            {
                comboBox1.Items.Add(d);
                comboBox2.Items.Add(d);
            }
            comboBox2.SelectedIndex = 0;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                MenuInfo d = comboBox1.SelectedItem as MenuInfo;
                if (d != null)
                {
                    LoadInfo(d);
                }
            }
        }

        private void LoadInfo(MenuInfo d)
        {
            textBox1.Text = d.Name;
            comboBox2.SelectedIndex = GetIndexById(d.ParentID, comboBox2);
            textBox2.Text = d.ReferenceForm;
            textBox3.Text = d.AssemblyFile;
            textBox4.Text = string.Join(",", d.Args.ToArray());
            numericUpDown1.Value = d.SortIndex;
            checkBox1.Checked = d.Enabled;
        }

        private int GetIndexById(int id, ComboBox combo)
        {
            for (int index = 0; index < combo.Items.Count; index++)
            {
                MenuInfo mi = combo.Items[index] as MenuInfo;
                if (mi != null &&mi.ID == id)
                        return index;
            }
            return 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MenuInfo menu = new MenuInfo();
            menu.Name = textBox1.Text.Trim();
            menu.ParentID = 0;
            MenuInfo mi = comboBox2.SelectedItem as MenuInfo;
            if (mi != null)
                menu.ParentID = mi.ID;
            menu.ReferenceForm = textBox2.Text.Trim();
            menu.AssemblyFile = textBox3.Text.Trim();
            menu.Args = new List<string>(textBox4.Text.Trim().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
            menu.SortIndex = (int)numericUpDown1.Value;
            menu.Enabled = checkBox1.Checked;
            if (MenuUtil.ExistsName(menu.Name))
            {
                if (MessageBox.Show("系统中已经存在该菜单，确定还要继续保存么？", "重名提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    int id = MenuUtil.AddMenu(menu);
                    if (id > 0)
                    {
                        RefreshMenuInfo();
                        MessageBox.Show("添加菜单成功！");
                    }
                }
                else
                {
                    textBox1.Focus();
                    textBox1.SelectAll();
                }
            }
            else
            {
                int id = MenuUtil.AddMenu(menu);
                if (id > 0)
                {
                    menu.ID = id;
                    RefreshMenuInfo();
                    MessageBox.Show("添加菜单成功！");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                MenuInfo menu = new MenuInfo();
                menu.ID = (comboBox1.SelectedItem as MenuInfo).ID;
                menu.Name = textBox1.Text.Trim();
                menu.ParentID = 0;
                MenuInfo mi = comboBox2.SelectedItem as MenuInfo;
                if (mi != null)
                    menu.ParentID = mi.ID;
                menu.ReferenceForm = textBox2.Text.Trim();
                menu.AssemblyFile = textBox3.Text.Trim();
                menu.Args = new List<string>(textBox4.Text.Trim().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
                menu.SortIndex = (int)numericUpDown1.Value;
                menu.Enabled = checkBox1.Checked;
                if (MenuUtil.ExistsNameOther(menu.Name, menu.ID))
                {
                    if (MessageBox.Show("系统中已经存在该菜单，确定还要继续保存么？", "重名提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                    {
                        if (MenuUtil.UpdateMenu(menu))
                        {
                            RefreshMenuInfo();
                            MessageBox.Show("修改菜单成功！");
                        }
                    }
                    else
                    {
                        textBox1.Focus();
                        textBox1.SelectAll();
                    }
                }
                else
                {
                    if (MenuUtil.UpdateMenu(menu))
                    {
                        RefreshMenuInfo();
                        MessageBox.Show("修改菜单成功！");
                    }
                }
            }
            else
            {
                MessageBox.Show("先选定要修改的菜单！");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                if (MessageBox.Show("确定要删除该菜单？", "删除提醒", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    MenuInfo mi = comboBox1.SelectedItem as MenuInfo;
                    if (MenuUtil.DeleteMenu(mi))
                    {
                        RefreshMenuInfo();
                        MessageBox.Show("删除菜单成功！");
                    }
                }
            }
            else
            {
                MessageBox.Show("先选定要删除的菜单！");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            EditForm();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            UserManage um = new UserManage(owner);
            owner.AddTabPage("用户管理", um);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            RoleManage rm = new RoleManage(owner);
            owner.AddTabPage("配置权限", rm);
        }
    }
}
