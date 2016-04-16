using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

namespace Monitor
{
    public partial class SelectControlForm : Form
    {
        public SelectControlForm()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 程序集文件全路径，为空即为本程序集
        /// </summary>
        public string AssemblyFile
        {
            get
            {
                return textBox1.Text.Trim();
            }
        }
        /// <summary>
        /// 类全名
        /// </summary>
        public string FormName
        {
            get
            {
                return textBox2.Text.Trim();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                fbd.SelectedPath = Application.StartupPath;
                if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    textBox1.Text = fbd.SelectedPath;
                }
            }
        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox2.Text = comboBox4.Text;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            List<string> types = GetTypes();
            types.Sort();
            comboBox4.Items.Clear();
            comboBox4.Items.AddRange(types.ToArray());
        }

        private List<string> GetTypes()
        {
            List<string> ts = new List<string>();
            string assembly = textBox1.Text;
            if (assembly != "")
            {
                string assFile = AppDomain.CurrentDomain.BaseDirectory + assembly;
                if (!File.Exists(assFile))
                {
                    assFile = assembly;
                }
                if (File.Exists(assFile))
                {
                    Assembly ass = Assembly.LoadFrom(assFile);
                    if (ass != null)
                    {
                        Type[] types = ass.GetTypes();
                        if (types != null)
                        {
                            foreach (Type t in types)
                            {
                                if (t.IsSubclassOf(typeof(Form)))
                                {
                                    ts.Add(t.FullName);
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("指定的程序集中没有任何类！");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("指定的程序集[" + assembly + "]在当前目录下不存在！");
                }
            }
            else//本程序集
            {
                Assembly ass = Assembly.GetExecutingAssembly();
                Type[] types = ass.GetTypes();
                if (types != null)
                {
                    foreach (Type t in types)
                    {
                        if (t.IsSubclassOf(typeof(Form)))
                        {
                            ts.Add(t.FullName);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("当前程序集中没有任何类！");
                }
            }
            return ts;
        }

        private void SelectControlForm_Load(object sender, EventArgs e)
        {

        }
    }
}
