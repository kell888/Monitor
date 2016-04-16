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
    public partial class ChangePwdForm : MonitorForm
    {
        public ChangePwdForm(Index owner, string username, string pwd)
            : base(owner, username, pwd)
        {
            InitializeComponent();
            this.owner = owner;
            this.label5.Text = username;
            this.pwd = pwd;
            owner.ShowInfo("修改密码");
        }
        Index owner;
        string pwd;

        private void button1_Click(object sender, EventArgs e)
        {
            CloseThisSubForm();
        }

        public override void CloseThisSubForm()
        {
            TabPage page = this.Parent as TabPage;
            if (page != null)
            {
                owner.CloseSubForm(page);
            }
            else
            {
                this.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string oldPwd = textBox1.Text;
            if (!this.pwd.Equals(oldPwd, StringComparison.InvariantCultureIgnoreCase))
            {
                MessageBox.Show("原密码有误！请重新输入。");
                owner.ShowInfo("原密码有误！请重新输入。");
                textBox1.Focus();
                textBox1.SelectAll();
                return;
            }
            string newPwd = textBox2.Text;
            string newPwd2 = textBox3.Text;
            if (newPwd != newPwd2)
            {
                MessageBox.Show("新密码确认有误！请重新确认。");
                owner.ShowInfo("新密码确认有误！请重新确认。");
                return;
            }
            if (UserUtil.ChangePwd(Index.User.ID, new RSA(System.Text.Encoding.Unicode.GetString(Convert.FromBase64String(ConfigurationManager.AppSettings["privateKey"]))).Encrypt(newPwd)))
            {
                Index.User.Password = newPwd;
                MessageBox.Show("密码修改成功！");
                owner.ShowInfo("密码修改成功！");
            }
            else
            {
                MessageBox.Show("密码修改失败！");
                owner.ShowInfo("密码修改失败！");
            }
            CloseThisSubForm();
        }
    }
}
