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
    public partial class Problems : Form
    {
        private static int hostid = 0;
        Zabbix zabbix = new Zabbix("Admin", "zabbix", "http://192.168.96.143/zabbix/api_jsonrpc.php");
        Response responseObj = null;
        public Problems()
        {
            InitializeComponent();
            zabbix.login();
            LoadProblems();
        }
        public Problems(String address)
        {

            InitializeComponent();
            LoadProblems();
        }
        public void LoadProblems()
        {
            String[] Problem_array = new string[] { "Not classified", "Infomation", "Warning", "Average", "High", "Disater" };
            String[] Problem_color = new String[] { "#97AAB3", "#7499FF", "#FFC859", "#FFA059", "#E97659", "#EA4335" };

            responseObj = zabbix.objectResponse("problem.get", new
            {
                output = new String[]
                {
                    "name", "clock", "eventid", "severity"
                }
            });
            int i = 0;
            foreach (dynamic data in responseObj.result)
            {
                dtgv1.Rows.Add(data.severity, data.eventid, data.name, data.clock);
                String s = Problem_color[2];

                //set Text + Color Severity
                dtgv1.Rows[i].Cells[0].Style.BackColor = ColorTranslator.FromHtml(Problem_color[(Convert.ToInt32(data.severity))]);
                dtgv1.Rows[i].Cells[0].Value = Problem_array[(Convert.ToInt32(data.severity))];
                i++;
            }
        }

        private void Problems_Load(object sender, EventArgs e)
        {

        }
    }
}
