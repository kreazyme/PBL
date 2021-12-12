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
                    "name", "clock", "eventid", "severity",
                },
                //history = 4,
            }) ;
            int i = 0;
            foreach (dynamic data in responseObj.result)
            {
                String date = UnixTimestampToDateTime(Convert.ToDouble(data.clock));
                Zabbix c = new Zabbix("admin", "zabbix", "http://192.168.96.143/zabbix/api_jsonrpc.php");
                c.login();
                string hostname = null;
                string s = data.eventid;
                Response responseobj2 = c.objectResponse("event.get", new
                {
                    evetids = s,
                    limit = 10,
                    selectedhosts = new string[]
                    {
                        "name",
                        "hostid",
                    },

                });
                //hostname = responseobj2.result.hosts.name;
                hostname = "unknown";
                dtgv1.Rows.Add(data.severity, hostname, data.name, date);



                //set Text + Color Severity
                dtgv1.Rows[i].Cells[0].Style.BackColor = ColorTranslator.FromHtml(Problem_color[(Convert.ToInt32(data.severity))]);
                dtgv1.Rows[i].Cells[0].Value = Problem_array[(Convert.ToInt32(data.severity))];
                i++;
            }
        }


        private String UnixTimestampToDateTime(double unixTime)
        {
            if (unixTime < 1007432428)
            {
                return "no data";
            }
            DateTime unixStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            long unixTimeStampInTicks = (long)(unixTime * TimeSpan.TicksPerSecond);
            DateTime dt = new DateTime(unixStart.Ticks + unixTimeStampInTicks, System.DateTimeKind.Utc);
            return dt.ToString();
        }

        private void Problems_Load(object sender, EventArgs e)
        {

        }
    }
}
