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
    public partial class DeviceManage : MonitorForm
    {
        public DeviceManage(Index owner)
        : base(owner)
        {
            InitializeComponent();
            owner.ShowInfo("设备管理");
        }

        private void DeviceManage_Load(object sender, EventArgs e)
        {
            RefreshList();
        }

        private void RefreshList()
        {
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            comboBox3.Items.Clear();
            List<Device> devs = Device.GetAllDevices();
            foreach (Device dev in devs)
            {
                comboBox1.Items.Add(dev);
            }
            List<DeviceType> dts = DeviceType.GetAllDeviceTypes();
            foreach (DeviceType dt in dts)
            {
                comboBox2.Items.Add(dt);
            }
            List<Train> trs = Train.GetAllTrains();
            foreach (Train tr in trs)
            {
                comboBox3.Items.Add(tr);
            }
            textBox1.Clear();
            textBox2.Clear();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                Device dev = comboBox1.SelectedItem as Device;
                if (dev != null)
                {
                    ShowInfo(dev);
                }
            }
        }

        private void ShowInfo(Device dev)
        {
            for (int i = 0; i < comboBox2.Items.Count; i++)
            {
                DeviceType dt = comboBox2.Items[i] as DeviceType;
                if (dt != null && dt.device_type_id == dev.device_type_id)
                {
                    comboBox2.SelectedIndex = i;
                    break;
                }
            }
            for (int i = 0; i < comboBox3.Items.Count; i++)
            {
                Train tr = comboBox3.Items[i] as Train;
                if (tr != null && tr.id == dev.train_id)
                {
                    comboBox3.SelectedIndex = i;
                    break;
                }
            }
            textBox1.Text = dev.device_name;
            textBox2.Text = dev.address;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Device element = new Device();
            DeviceType dt = comboBox2.SelectedItem as DeviceType;
            if (dt != null)
            {
                element.device_type_id = dt.device_type_id;
                Train tr = comboBox3.SelectedItem as Train;
                if (tr != null)
                {
                    element.train_id = tr.id;
                    element.device_name = textBox1.Text.Trim();
                    element.address = textBox2.Text.Trim();
                    if (Device.Insert(element) > 0)
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
                    MessageBox.Show("请选定一个归属的列车！");
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
                Device element = comboBox1.SelectedItem as Device;
                if (element != null)
                {
                    DeviceType dt = comboBox2.SelectedItem as DeviceType;
                    if (dt != null)
                    {
                        element.device_type_id = dt.device_type_id;
                        Train tr = comboBox3.SelectedItem as Train;
                        if (tr != null)
                        {
                            element.train_id = tr.id;
                            element.device_name = textBox1.Text.Trim();
                            element.address = textBox2.Text.Trim();
                            if (Device.Update(element))
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
                            MessageBox.Show("请选定一个归属的列车！");
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
                MessageBox.Show("请选择一个需要修改的设备！");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                Device element = comboBox1.SelectedItem as Device;
                if (element != null)
                {
                    if (MessageBox.Show("确定要删除该设备[" + element.device_name + "]？", "删除提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.OK)
                    {
                        if (Device.Delete(element.id))
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
                MessageBox.Show("请选择一个需要删除的设备！");
            }
        }
    }
}
