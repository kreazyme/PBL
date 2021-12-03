using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZabbixApi;

namespace PBL
{
    public partial class Form2 : Form
    {
        private int hostid;
        public Form2()
        {
            InitializeComponent();
            Zabbix zabbix = new Zabbix("Admin", "zabbix", "http://192.168.96.143/zabbix/api_jsonrpc.php");
            zabbix.login();
            Response responseObj = zabbix.objectResponse("host.get", new
            {
                output = new String[] { "hostid", "host" },
                selectInterfaces = new String[] { "interfaceid", "ip" },
            });
            foreach (dynamic data in responseObj.result)
            {
                Host_Item item = new Host_Item();
                item.Text = data.host;
                item.Value = data.hostid;
                CBB_ListHost.Items.Add(item);
            }
            hostid = Convert.ToInt32(responseObj.result[0].hostid);
            CBB_ListHost.SelectedIndex = 0;
            responseObj = zabbix.objectResponse("history.get", new
            {
                hostids = 10460,
                itemids = 38698,
                limit = 10,
            });
            int i = 0;
            chart1.Series["line1"].XValueMember = "Thời gian";

            foreach (dynamic data in responseObj.result)
            {
                i++;
                int y = Convert.ToInt32(data.value);
                chart1.Series["line1"].Points.AddXY(i, y);
            }
            responseObj = zabbix.objectResponse("item.get", new
            {
                itemids = 38698,
            });
            chart1.Titles.Add(responseObj.result[0].name);


        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
