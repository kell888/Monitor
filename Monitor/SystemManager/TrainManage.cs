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
    public partial class TrainManage : MonitorForm
    {
        public TrainManage(Index owner)
            : base(owner)
        {
            InitializeComponent();
            owner.ShowInfo("列车管理");
        }

        private void TrainManage_Load(object sender, EventArgs e)
        {
            RefreshList();
        }

        private void RefreshList()
        {
            comboBox1.Items.Clear();
            List<Train> sts = Train.GetAllTrains();
            foreach (Train st in sts)
            {
                comboBox1.Items.Add(st);
            }
            numericUpDown1.Value = 6;
            textBox1.Clear();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                Train st = comboBox1.SelectedItem as Train;
                if (st != null)
                {
                    ShowInfo(st);
                }
            }
        }

        private void ShowInfo(Train st)
        {
            textBox1.Text = st.train_no;
            numericUpDown1.Value = st.count;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Train element = new Train();
            element.train_no = textBox1.Text.Trim();
            element.count = (int)numericUpDown1.Value;
            if (Train.Insert(element) > 0)
            {
                MessageBox.Show("添加成功！");
                RefreshList();
            }
            else
            {
                MessageBox.Show("添加失败！");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                Train element = comboBox1.SelectedItem as Train;
                if (element != null)
                {
                    element.train_no = textBox1.Text.Trim();
                    element.count = (int)numericUpDown1.Value;
                    if (Train.Update(element))
                    {
                        MessageBox.Show("修改成功！");
                        RefreshList();
                    }
                    else
                    {
                        MessageBox.Show("修改失败！");
                    }
                }
            }
            else
            {
                MessageBox.Show("请选择一个需要修改的列车！");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                Train element = comboBox1.SelectedItem as Train;
                if (element != null)
                {
                    if (MessageBox.Show("确定要删除该列车[" + element.train_no + "]？", "删除提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                    {
                        if (Train.Delete(element.id))
                        {
                            MessageBox.Show("删除成功！");
                            RefreshList();
                        }
                        else
                        {
                            MessageBox.Show("删除失败！");
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("请选择一个需要删除的列车！");
            }
        }
    }
}
