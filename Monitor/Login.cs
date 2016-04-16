using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using Monitor.App_Code;

namespace Monitor
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        int id;
        string name;
        string pwd;
        int operater;

        public int UserId
        {
            get
            {
                return id;
            }
        }

        public string UserName
        {
            get
            {
                return name;
            }
        }

        public string Password
        {
            get
            {
                return pwd;
            }
        }

        public int Operater
        {
            get
            {
                return operater;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Text = "登录中...";
            string username = txtName.Text.Trim();
            string userpwd = txtPassword.Text;
            DateTime UserTime = DateTime.Now;
            using (SqlConnection strcon = new SqlConnection(MonitoringSystemConfiguration.DbConnectionString))
            {
                using (SqlCommand scd = new SqlCommand("select id,power_time as ff from a_user where login_name='" + username + "' and password='" + new RSA(System.Text.Encoding.Unicode.GetString(Convert.FromBase64String(ConfigurationManager.AppSettings["privateKey"]))).Encrypt(userpwd) + "'", strcon))
                {
                    strcon.Open();
                    SqlDataReader reader = scd.ExecuteReader();
                    if (reader.Read())
                    {
                        int id = reader.GetInt32(0);
                        DateTime count = reader.GetDateTime(1);
                        reader.Close();
                        if (count > DateTime.Now)
                        {
                            label3.Visible = false;
                            //Session["txtName"] = username;
                            //Session["txtPassword"] = userpwd;
                            string N_IP = Common.GetIPv4().ToString();
                            string User = txtName.Text;
                            string Operator = "未知角色";
                            using (SqlCommand comm = new SqlCommand("select count(*) as ff from a_user where login_name='" + User + "'and operation=3", strcon))
                            {
                                int count1 = Convert.ToInt32(comm.ExecuteScalar());
                                if (count1 > 0)
                                {
                                    Operator = "管理员";
                                    operater = 3;
                                }
                                else
                                {
                                    using (SqlCommand com = new SqlCommand("select count(*) as ff from a_user where login_name='" + User + "'and operation=1", strcon))
                                    {
                                        int count2 = Convert.ToInt32(com.ExecuteScalar());
                                        if (count2 > 0)
                                        {
                                            Operator = "普通用户";
                                            operater = 1;
                                        }
                                        else
                                        {
                                            using (SqlCommand con = new SqlCommand("select count(*) as ff from a_user where login_name='" + User + "'and operation=2", strcon))
                                            {
                                                int count3 = Convert.ToInt32(con.ExecuteScalar());
                                                if (count3 > 0)
                                                {
                                                    Operator = "检修用户";
                                                    operater = 2;
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            scd.CommandText = "insert into d_login_log (N_Operator,N_User,N_IP,N_UserTime)values(@Operator,@N_User,@N_IP,@UserTime)";
                            SqlParameter para = new SqlParameter("@Operator", SqlDbType.VarChar, 20);
                            para.Value = Operator;
                            scd.Parameters.Add(para);
                            para = new SqlParameter("@N_User", SqlDbType.VarChar, 10);
                            para.Value = username;
                            scd.Parameters.Add(para);
                            para = new SqlParameter("@N_IP", SqlDbType.VarChar, 30);
                            para.Value = N_IP;
                            scd.Parameters.Add(para);
                            para = new SqlParameter("@UserTime", SqlDbType.DateTime);
                            para.Value = UserTime;
                            scd.Parameters.Add(para);
                            scd.ExecuteNonQuery();

                            this.id = id;
                            this.name = username;
                            this.pwd = userpwd;
                            this.DialogResult = DialogResult.OK;
                        }
                        else
                        {
                            label3.Text = "该用户的登陆有效期已经过了，请联系管理员！";
                            label3.Visible = true;
                            MessageBox.Show("该用户的登陆有效期已经过了，请联系管理员！");
                        }
                    }
                    else
                    {
                        label3.Text = "用户名或者密码错误，请重新输入！";
                        label3.Visible = true;
                        MessageBox.Show("用户名或者密码错误，请重新输入！");
                    }
                }
            }
            button1.Text = "登录";
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            txtPassword.Clear();
        }

        private void txtPassword_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                button1.PerformClick();
            }
        }
    }
}
