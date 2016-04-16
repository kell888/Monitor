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
    public partial class ArgumentManage : MonitorForm
    {
        public ArgumentManage(Index owner)
           : base(owner)
        {
            InitializeComponent();
            owner.ShowInfo("系统参数配置");
        }

        private void ArgumentManage_Load(object sender, EventArgs e)
        {
            RefreshList();
        }

        private void RefreshList()
        {
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            comboBox3.Items.Clear();
            List<Argument> args = Argument.GetAllArguments();
            foreach (Argument arg in args)
            {
                comboBox1.Items.Add(arg);
            }
            List<DeviceType> dts = DeviceType.GetAllDeviceTypes();
            foreach (DeviceType dt in dts)
            {
                comboBox2.Items.Add(dt);
            }
            List<PointType> pts = PointType.GetAllPointTypes();
            foreach (PointType pt in pts)
            {
                comboBox3.Items.Add(pt);
            }
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                Argument arg = comboBox1.SelectedItem as Argument;
                if (arg != null)
                {
                    ShowInfo(arg);
                }
            }
        }

        private void ShowInfo(Argument arg)
        {
            for (int i = 0; i < comboBox2.Items.Count; i++)
            {
                DeviceType dt = comboBox2.Items[i] as DeviceType;
                if (dt != null && dt.device_type_id == arg.Device_type_id)
                {
                    comboBox2.SelectedIndex = i;
                    break;
                }
            }
            for (int i = 0; i < comboBox3.Items.Count; i++)
            {
                PointType pt = comboBox3.Items[i] as PointType;
                if (pt != null && pt.point_type_id == arg.Point_type_id)
                {
                    comboBox3.SelectedIndex = i;
                    break;
                }
            }
            textBox1.Text = arg.Argument_name;
            textBox2.Text = arg.Standard_value;
            textBox3.Text = arg.Min_value;
            textBox4.Text = arg.Max_value;
            checkBox1.Checked = arg.ValueIsNumeric;
            checkBox2.Checked = arg.IsRange;
            checkBox3.Checked = arg.IsEnable;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Argument element = new Argument();
            DeviceType dt = comboBox2.SelectedItem as DeviceType;
            if (dt != null)
            {
                element.Device_type_id = dt.device_type_id;
                PointType pt = comboBox3.SelectedItem as PointType;
                if (pt != null)
                {
                    element.Point_type_id = pt.point_type_id;
                    element.Argument_name = textBox1.Text.Trim();
                    element.Standard_value = textBox2.Text.Trim();
                    element.Min_value = textBox3.Text.Trim();
                    element.Max_value = textBox4.Text.Trim();
                    if (Argument.Insert(element) > 0)
                    {
                        MessageBox.Show("添加成功！");
                        RefreshList();
                    }
                    else
                    {
                        MessageBox.Show("添加失败！");
                    }
                }
                else
                {
                    MessageBox.Show("请选定一个归属的监测类型！");
                }
            }
            else
            {
                MessageBox.Show("请选定一个归属的设备类型！");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                Argument element = comboBox1.SelectedItem as Argument;
                if (element != null)
                {
                    DeviceType dt = comboBox2.SelectedItem as DeviceType;
                    if (dt != null)
                    {
                        element.Device_type_id = dt.device_type_id;
                        PointType tr = comboBox3.SelectedItem as PointType;
                        if (tr != null)
                        {
                            element.Point_type_id = tr.point_type_id;
                            element.Argument_name = textBox1.Text.Trim();
                            element.Standard_value = textBox2.Text.Trim();
                            element.Min_value = textBox3.Text.Trim();
                            element.Max_value = textBox4.Text.Trim();
                            if (Argument.Update(element))
                            {
                                MessageBox.Show("修改成功！");
                                RefreshList();
                            }
                            else
                            {
                                MessageBox.Show("修改失败！");
                            }
                        }
                        else
                        {
                            MessageBox.Show("请选定一个归属的监测类型！");
                        }
                    }
                    else
                    {
                        MessageBox.Show("请选定一个归属的设备类型！");
                    }
                }
            }
            else
            {
                MessageBox.Show("请选择一个需要修改的参数！");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                Argument element = comboBox1.SelectedItem as Argument;
                if (element != null)
                {
                    if (MessageBox.Show("确定要删除该参数[" + element.Argument_name + "]？", "删除提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                    {
                        if (Argument.Delete(element.Id))
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
                MessageBox.Show("请选择一个需要删除的参数！");
            }
        }
    }
}
