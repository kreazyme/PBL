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
            Zabbix zabbix = new Zabbix("Admin", "zabbix", "http://192.168.1.6/zabbix/api_jsonrpc.php");
            zabbix.login();
            Response responseObj = zabbix.objectResponse("host.get", new
            {
                output =new String[] { "hostid", "host"},
                       selectInterfaces =  new String[] {"interfaceid","ip"},
            });
            foreach(dynamic data in responseObj.result)
            {
                Host_Item item = new Host_Item();
                item.Text = data.host;
                item.Value = data.hostid;
                CBB_ListHost.Items.Add(item);
            }
            hostid = Convert.ToInt32(responseObj.result[0].hostid);
            CBB_ListHost.SelectedIndex = 0;





            responseObj = zabbix.objectResponse("graph.get", new
            {
                graphid = 910,
                hostids =  hostid,
            });
            zabbix.logout();
        }
    }
}
