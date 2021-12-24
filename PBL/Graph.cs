using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using ZabbixApi;

namespace PBL
{
    public partial class Graph : Form
    {
        private static String Chartname = "";
        private static String valuetype = "3";
        private static int itemid;
        Zabbix zabbix = null;
        Response responseObj = null;
        public Graph(Zabbix z, int id)
        {
            InitializeComponent();
            zabbix = z;
            itemid = id;
            zabbix.login();

            LoadGraphName();
            LoadGraph();
        }
        private void LoadGraphName()
        {
            responseObj = zabbix.objectResponse("item.get", new
            {
                output = new String[]
                {
                    "value_type", "name", "units"
                },
                itemids = itemid,
            });
            foreach(dynamic data in responseObj.result)
            {
                chart1.ChartAreas["ChartArea1"].AxisY.Title = data.units;
                Chartname = data.name;
                chart1.Series["line1"].Name = Chartname;
                valuetype = data.value_type;
            }
        }
        private void LoadGraph()
        {
            responseObj = zabbix.objectResponse("history.get", new
            {
                itemids = itemid,
                limit = 25,
                sortfield = "clock",
                sortorder = "DESC",
                history = valuetype
            });
            int i = 0;
            String s = null;
            foreach(dynamic data in responseObj.result)
            {

                chart1.Series[Chartname].Points.AddXY(UnixTimestampToHourMinutes(Convert.ToDouble(data.clock)), Convert.ToDouble(data.value));
                s = data.clock;
                i++;
            }
            chart1.ChartAreas["ChartArea1"].AxisX.Title = "Time from "  + UnixTimestampToDateTime(Convert.ToDouble(s)) + " to " + UnixTimestampToDateTime(Convert.ToDouble(responseObj.result[0].clock));

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

        private String UnixTimestampToHourMinutes(double unixTime)
        {
            if (unixTime < 1007432428)
            {
                return "no data";
            }
            DateTime unixStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            long unixTimeStampInTicks = (long)(unixTime * TimeSpan.TicksPerSecond);
            DateTime dt = new DateTime(unixStart.Ticks + unixTimeStampInTicks, System.DateTimeKind.Utc);
            return dt.Hour.ToString() + ":" + dt.Minute.ToString();
        }
    }
}
