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
using System.Windows.Controls;
using System.Windows.Forms;
using ZabbixApi;

namespace PBL
{
    public partial class Form2 : Form
    {

        private int hostid;
        Zabbix zabbix = null;
        Response responseObj = null;
        public Form2(Zabbix z)
        {
            InitializeComponent();
            zabbix = z;
            zabbix.login();


            //get all host
            responseObj = zabbix.objectResponse("host.get", new
            {
                output = new String[] { "hostid", "host" },
                selectInterfaces = new String[] { "interfaceid", "ip" },
            });
            foreach (dynamic data in responseObj.result)
            {
                Host_Item item = new Host_Item();


                //add host to CBB
                item.Text = data.host;
                item.Value = data.hostid;
                CBB_ListHost.Items.Add(item);
            }
            hostid = Convert.ToInt32(responseObj.result[0].hostid);
            CBB_ListHost.SelectedIndex = 0;

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }


        public void LoadDatagridView(int host)
        {
            responseObj = zabbix.objectResponse("item.get", new
            {
                output = new String[] { "itemid", "name", "description", "lastvalue", "lastclock", "key_" },
                hostids = host,
            });

            foreach (dynamic data in responseObj.result)
            {
                String description = data.description, value = data.lastvalue;
                String thoigian = UnixTimestampToDateTime(Convert.ToDouble(data.lastclock));
                String key = data.key_;
                string[] collection = key.Split('.');
                if(collection.Length > 1)
                {
                    key = collection[0] + collection[1];
                }


                //display if no data
                if (description == "")
                {
                    description = "no data";
                    if(value == "0")
                    {
                        value = "no data";
                    }
                }


                dtgv1.Rows.Add(data.itemid, data.name, description, value, key, thoigian);
            }
        }

        private void CBB_ListHost_SelectedIndexChanged(object sender, EventArgs e)
        {
            dtgv1.Rows.Clear();
            Host_Item emailServer = (Host_Item)CBB_ListHost.SelectedItem;
            LoadDatagridView(Convert.ToInt32(emailServer.Value.ToString()));
        }


        private String UnixTimestampToDateTime(double unixTime)
        {
            if(unixTime < 1007432428)
            {
                return "no data";
            }
            DateTime unixStart = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            long unixTimeStampInTicks = (long)(unixTime * TimeSpan.TicksPerSecond);
            DateTime dt = new DateTime(unixStart.Ticks + unixTimeStampInTicks, System.DateTimeKind.Utc);
            dt = dt.AddHours(7);
            return (dt.Hour.ToString() + ":" + dt.Minute.ToString() + " " + dt.Day.ToString() + "/" + dt.Month.ToString() + "/" + dt.Year.ToString());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Host_Item emailServer = (Host_Item)CBB_ListHost.SelectedItem;
            if(Convert.ToInt32(emailServer.Value.ToString()) != hostid)
            {
                dtgv1.Rows.Clear();
                hostid = Convert.ToInt32(emailServer.Value.ToString());
                LoadDatagridView(hostid);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int rowindex = dtgv1.CurrentCell.RowIndex;
            String itemid = dtgv1.Rows[rowindex].Cells[0].Value.ToString();
            if (dtgv1.Rows[rowindex].Cells[3].Value.ToString() == "no data")
            {
                GetItemInformation(itemid);
                return;
            }

            //get value type
            responseObj = zabbix.objectResponse("item.get", new
            {
                output = new String[] { "value_type" },
                itemids = itemid,
            });
            String valuetype = "";
            foreach (dynamic data in responseObj.result)
            {
                valuetype = data.value_type;
            }


            //Show if not graph
                GetItemInformation(itemid);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Problems p = new Problems(zabbix);
            p.Show();
        }

        private void GetItemInformation(String itemid)
        {
            responseObj = zabbix.objectResponse("item.get", new
            {
                output = "extend",
                itemids = itemid,
            });
            String s = "";
            foreach (dynamic data in responseObj.result)
            {
                try
                {
                    s += "Time: " + UnixTimestampToDateTime(Convert.ToDouble(data.lastclock));
                }
                catch (Exception exx)
                {
                    s += "Time: no data";
                }
                s += "\nDescription: " + data.description;
                s += "\nName: " + data.name;
                s += "\nSNMP OID: " + data.snmp_oid;
                s += "\nKey: " + data.key_;
                s += "\nUnit: " + data.units;
            }
            MessageBox.Show(s, "Information", MessageBoxButtons.OK);
        }

        private void dtgv1_DoubleClick(object sender, EventArgs e)
        {
            int rowindex = dtgv1.CurrentCell.RowIndex;
            String itemid = dtgv1.Rows[rowindex].Cells[0].Value.ToString();
            if(dtgv1.Rows[rowindex].Cells[3].Value.ToString() == "no data")
            {
                GetItemInformation(itemid);
                return;
            }

            //get value type
            responseObj = zabbix.objectResponse("item.get", new
            {
                output = new String[] { "value_type" },
                itemids = itemid,
            });
            String valuetype = "";
            foreach(dynamic data in responseObj.result)
            {
                valuetype = data.value_type;
            }


            //Show if not graph
            if(valuetype == "1")
            {
                GetItemInformation(itemid);
            }


            //Show graph
            else
            {
                Graph g = new Graph(zabbix, Convert.ToInt32(itemid));
                g.Show();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Coming soon", "Notification");
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            Checkbox();
        }

        private void checkBox2_CheckStateChanged(object sender, EventArgs e)
        {
            Checkbox();
        }
        private void Checkbox()
        {
            if (checkBox2.Checked == true)
            {
                Host_Item emailServer = (Host_Item)CBB_ListHost.SelectedItem;
                int host = Convert.ToInt32(emailServer.Value.ToString());
                dtgv1.Rows.Clear();
                responseObj = zabbix.objectResponse("item.get", new
                {
                    output = new String[] { "itemid", "name", "description", "lastvalue", "lastclock", "key_" },
                    hostids = host,
                });

                foreach (dynamic data in responseObj.result)
                {
                    String description = data.description, value = data.lastvalue;
                    String thoigian = UnixTimestampToDateTime(Convert.ToDouble(data.lastclock));
                    String key = data.key_;
                    string[] collection = key.Split('.');
                    if (collection.Length > 1)
                    {
                        key = collection[0] + collection[1];
                    }


                    //display if no data
                    if (description == "")
                    {
                        description = "no data";
                        if (value == "0")
                        {
                            value = "no data";
                        }
                    }
                    if (value != "no data")
                    {
                        dtgv1.Rows.Add(data.itemid, data.name, description, value, key, thoigian);
                    }
                }
            }
            else
            {
                Host_Item emailServer = (Host_Item)CBB_ListHost.SelectedItem;
                int host = Convert.ToInt32(emailServer.Value.ToString());
                LoadDatagridView(host);
            }
        }
    }
}
