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
    public partial class StationManage : MonitorForm
    {
        public StationManage(Index owner)
            : base(owner)
        {
            InitializeComponent();
            owner.ShowInfo("监测站管理");
        }

        private void StationManage_Load(object sender, EventArgs e)
        {
            RefreshList();
        }

        private void RefreshList()
        {
            comboBox1.Items.Clear();
            List<Station> sts = Station.GetAllStations();
            foreach (Station st in sts)
            {
                comboBox1.Items.Add(st);
            }
            numericUpDown1.Value = 0;
            textBox1.Clear();
            textBox2.Clear();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                Station st = comboBox1.SelectedItem as Station;
                if (st != null)
                {
                    ShowInfo(st);
                }
            }
        }

        private void ShowInfo(Station st)
        {
            textBox1.Text = st.line_no;
            textBox2.Text = st.station_name;
            numericUpDown1.Value = st.Vedio_count;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Station element = new Station();
            element.line_no = textBox1.Text.Trim();
            element.station_name = textBox2.Text.Trim();
            element.Vedio_count = (int)numericUpDown1.Value;
            if (Station.Insert(element) > 0)
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
                Station element = comboBox1.SelectedItem as Station;
                if (element != null)
                {
                    element.line_no = textBox1.Text.Trim();
                    element.station_name = textBox2.Text.Trim();
                    element.Vedio_count = (int)numericUpDown1.Value;
                    if (Station.Update(element))
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
                MessageBox.Show("请选择一个需要修改的监测站！");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                Station element = comboBox1.SelectedItem as Station;
                if (element != null)
                {
                    if (MessageBox.Show("确定要删除该监测站[" + element.station_name + "]？", "删除提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                    {
                        if (Station.Delete(element.id))
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
                MessageBox.Show("请选择一个需要删除的监测站！");
            }
        }
    }
}
