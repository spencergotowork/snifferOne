using System;
using System.Windows.Forms;
using SharpPcap;

namespace WinformsExample
{
    public partial class DeviceListForm : Form
    {
        public DeviceListForm()
        {
            InitializeComponent();
        }

        private void DeviceListForm_Load(object sender, EventArgs e)
        {
            foreach (var dev in CaptureDeviceList.Instance)
            {
                var str = String.Format("{0}  设备:   {1}", dev.Description, dev.Name);
                deviceList.Items.Add(str);
            }
        }

        public delegate void OnItemSelectedDelegate(int itemIndex);
        public event OnItemSelectedDelegate OnItemSelected;

        public delegate void OnCancelDelegate();
        public event OnCancelDelegate OnCancel;

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            OnCancel();
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            WinformsExample.PcapFilter.total_filter = "";
            string addr = "", proto = "", port = "";
            if(checkedListBox1.CheckedItems.Count > 0)
                proto = string.Join(" or ", WinformsExample.PcapFilter.proto_filter.ToArray());
            
            if(textBox1.Enabled)
                addr = "net " + string.Join(" or ", WinformsExample.PcapFilter.addr_filter.ToArray());

            if(textBox2.Enabled)
                port = "port " + string.Join(" or ", WinformsExample.PcapFilter.port_filter.ToArray());

            WinformsExample.PcapFilter.total_filter = proto + " " + addr + " " + port;

            //listBox1.Items.Add(WinformsExample.PcapFilter.total_filter);


            if (deviceList.SelectedItem != null)
            {
                OnItemSelected(deviceList.SelectedIndex);
            }

            //for test
            //listBox1.Items.Add("proto : ");
            //string proto = string.Join(" or ", WinformsExample.PcapFilter.proto_filter.ToArray());
            //listBox1.Items.Add(proto);

            //if(textBox1.Enabled)
            //{
            //    listBox1.Items.Add("addr : ");
            //    string addr = string.Join(" or ", WinformsExample.PcapFilter.addr_filter.ToArray());
            //    listBox1.Items.Add(addr);
            //}

            //if (textBox2.Enabled)
            //{
            //    listBox1.Items.Add("port : ");
            //    string port = string.Join(" or ", WinformsExample.PcapFilter.port_filter.ToArray());
            //    listBox1.Items.Add(port);
            //}
            //test end
        }

        private void deviceList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            WinformsExample.PcapFilter.total_filter = "";
            string addr = "", proto = "", port = "";
            if (checkedListBox1.CheckedItems.Count > 0)
                proto = string.Join(" or ", WinformsExample.PcapFilter.proto_filter.ToArray());

            if (textBox1.Enabled)
                addr = "net " + string.Join(" or ", WinformsExample.PcapFilter.addr_filter.ToArray());

            if (textBox2.Enabled)
                port = "port " + string.Join(" or ", WinformsExample.PcapFilter.port_filter.ToArray());

            WinformsExample.PcapFilter.total_filter = proto + " " + addr + " " + port;


            if (deviceList.SelectedItem != null)
            {
                OnItemSelected(deviceList.SelectedIndex);
            }
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            WinformsExample.PcapFilter.proto_filter.Clear();
            foreach (var proto in checkedListBox1.CheckedItems)
            {
                WinformsExample.PcapFilter.proto_filter.Add(proto.ToString());
            }
            // address无法查询
            if (WinformsExample.PcapFilter.proto_filter.Exists(str => str == "tcp" || 
                                                                       str == "udp" ||
                                                                       str == "icmp" ||
                                                                       str == "ether"))
            {
                textBox1.Enabled = false;
            }
            else
                textBox1.Enabled = true;

            // port 无法查询
            if (WinformsExample.PcapFilter.proto_filter.Exists(str => str == "ip" ||
                                                                       str == "ip6" ||
                                                                       str == "arp" ||
                                                                       str == "rarp" ||
                                                                       str == "icmp" ||
                                                                       str == "decnet" ||
                                                                       str == "ether"))
            {
                textBox2.Enabled = false;
            }
            else
                textBox2.Enabled = true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if(textBox1.Text != null)
                WinformsExample.PcapFilter.addr_filter = new System.Collections.Generic.List<string>(textBox1.Text.ToString().Split(' '));

            /// for test label
            //listBox1.Items.Clear();
            //foreach (var item in WinformsExample.PcapFilter.addr_filter)
            //    listBox1.Items.Add(item.ToString());
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text != null)
                WinformsExample.PcapFilter.port_filter = new System.Collections.Generic.List<string>(textBox2.Text.ToString().Split(' '));

            /// for test label
            //listBox1.Items.Clear();
            //foreach (var item in WinformsExample.PcapFilter.port_filter)
            //    listBox1.Items.Add(item.ToString());
        }

    }
}
