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
    public partial class Graph : Form
    {
        private static String ip_address;
        private static int itemid;
        Zabbix zabbix = null;
        Response responseObj = null;
        public Graph(String address, int id)
        {
            InitializeComponent();
            ip_address = address;
            itemid = id;
            zabbix = new Zabbix("Admin", "zabbix", ip_address);
            zabbix.login();


            LoadGraphName();
        }
        private void LoadGraphName()
        {
            responseObj = zabbix.objectResponse("item.get", new
            {
                itemids = itemid,
            });
            foreach(dynamic data in responseObj.result)
            {
                label1.Text = data.name;
            }
        }
        private void LoadGraph()
        {
            responseObj = zabbix.objectResponse("history.get", new
            {
               itemids = itemid
            });
            foreach(dynamic data in responseObj.result)
            {

            }
        }
    }
}
