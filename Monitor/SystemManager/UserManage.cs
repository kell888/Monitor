using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Monitor.App_Code;
using System.Configuration;

namespace Monitor.SystemManager
{
    public partial class UserManage : Form
    {
        public UserManage(Index owner)
        {
            InitializeComponent();
            this.owner = owner;
            owner.ShowInfo("用户管理");
        }

        Index owner;

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                UserInfo d = comboBox1.SelectedItem as UserInfo;
                if (d != null)
                {
                    LoadInfo(d);
                }
            }
        }

        private void LoadInfo(UserInfo d)
        {
            textBox1.Text = d.Login_name;
            textBox2.Text = new RSA(System.Text.Encoding.Unicode.GetString(Convert.FromBase64String(ConfigurationManager.AppSettings["privateKey"]))).Decrypt(d.Password);
            comboBox2.SelectedIndex = GetIndexByOperation(d.Operation);
            dateTimePicker1.Value = d.Power_time;
            textBox3.Text = d.Add_time.ToString("yyyy-MM-dd HH:mm");
        }

        private int GetIndexByOperation(int operation)
        {
            if (operation > 0)
            {
                switch (operation)
                {
                    case 3:
                        return 0;
                    case 2:
                        return 1;
                    case 1:
                        return 2;
                }
            }
            return -1;
        }

        private void UserManage_Load(object sender, EventArgs e)
        {
            LoadAllUsers();
        }

        private void LoadAllUsers()
        {
            List<UserInfo> data = UserUtil.GetUserList();
            comboBox1.Items.Clear();
            foreach (UserInfo d in data)
            {
                comboBox1.Items.Add(d);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UserInfo user = new UserInfo();
            user.Login_name = textBox1.Text.Trim();
            user.Password = new RSA(System.Text.Encoding.Unicode.GetString(Convert.FromBase64String(ConfigurationManager.AppSettings["privateKey"]))).Encrypt(textBox2.Text);
            int operation = 1;
            switch (comboBox2.SelectedIndex)
            {
                case 0:
                    operation = 3;
                    break;
                case 1:
                    operation = 2;
                    break;
                case 2:
                    operation = 1;
                    break;
            }
            user.Operation = operation;
            user.Power_time = dateTimePicker1.Value;
            if (UserUtil.ExistsName(user.Login_name))
            {
                if (MessageBox.Show("系统中已经存在该用户，确定还要继续保存么？", "重名提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    int id = UserUtil.AddUser(user);
                    if (id > 0)
                    {
                        LoadAllUsers();
                        MessageBox.Show("添加用户成功！");
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
                int id = UserUtil.AddUser(user);
                if (id > 0)
                {
                    LoadAllUsers();
                    MessageBox.Show("添加用户成功！");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                UserInfo user = new UserInfo();
                user.ID = (comboBox1.SelectedItem as UserInfo).ID;
                user.Login_name = textBox1.Text.Trim();
                user.Password = new RSA(System.Text.Encoding.Unicode.GetString(Convert.FromBase64String(ConfigurationManager.AppSettings["privateKey"]))).Encrypt(textBox2.Text);
                int operation = 1;
                switch (comboBox2.SelectedIndex)
                {
                    case 0:
                        operation = 3;
                        break;
                    case 1:
                        operation = 2;
                        break;
                    case 2:
                        operation = 1;
                        break;
                }
                user.Operation = operation;
                user.Power_time = dateTimePicker1.Value;
                if (UserUtil.ExistsNameOther(user.Login_name, user.ID))
                {
                    if (MessageBox.Show("系统中已经存在该用户，确定还要继续保存么？", "重名提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                    {
                        if (UserUtil.UpdateUser(user))
                        {
                            LoadAllUsers();
                            MessageBox.Show("修改用户成功！");
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
                    if (UserUtil.UpdateUser(user))
                    {
                        LoadAllUsers();
                        MessageBox.Show("修改用户成功！");
                    }
                }
            }
            else
            {
                MessageBox.Show("先选定要修改的用户！");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                if (MessageBox.Show("确定要删除该用户？", "删除提醒", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                {
                    UserInfo mi = comboBox1.SelectedItem as UserInfo;
                    if (UserUtil.DeleteUser(mi))
                    {
                        LoadAllUsers();
                        MessageBox.Show("删除用户成功！");
                    }
                }
            }
            else
            {
                MessageBox.Show("先选定要删除的用户！");
            }
        }
    }
}
