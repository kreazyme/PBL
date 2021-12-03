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



            responseObj = zabbix.objectResponse("item.get", new
            {
                output = new String[] { "itemid", "name", "description", "lastvalue" },
                hostids = 10084,
            });
            //int i = 0;
            //chart1.Series["line1"].XValueMember = "Thời gian";

            foreach (dynamic data in responseObj.result)
            {
                dtgv1.Rows.Add(data.itemid, data.name, data.description, data.lastvalue);
            }
            //responseObj = zabbix.objectResponse("item.get", new
            //{
            //    hostids = 10084,
            //});
            //chart1.Titles.Add(responseObj.result[0].name);


        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
