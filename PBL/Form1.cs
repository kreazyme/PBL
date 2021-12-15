using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZabbixApi;

namespace PBL
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void txtIP4_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtIP1_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtIP2_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtIP3_TextChanged(object sender, EventArgs e)
        {
                
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtIP1.Clear();
            txtIP2.Clear();
            txtIP3.Clear();
            txtIP4.Clear();
            txtPassword.Clear();
            txtUsername.Clear();
            txtIP1.Focus();
        }
        private Zabbix zabbix;
        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                int i = Convert.ToInt32(txtIP3.Text);
                if (i < 0 || i > 256)
                {
                    txtIP3.Clear();
                    MessageBox.Show("Wrong IP Address", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch (Exception ex)
            {
                txtIP3.Clear();
                MessageBox.Show("Wrong IP Address", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                int i = Convert.ToInt32(txtIP2.Text);
                if (i < 0 || i > 256)
                {
                    txtIP2.Clear();
                    MessageBox.Show("Wrong IP Address", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                txtIP2.Clear();
                MessageBox.Show("Wrong IP Address", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            try
            {
                int i = Convert.ToInt32(txtIP1.Text);
                if (i < 0 || i > 256)
                {
                    txtIP1.Clear();
                    MessageBox.Show("Wrong IP Address", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                txtIP1.Clear();
                MessageBox.Show("Wrong IP Address", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            try
            {
                int i = Convert.ToInt32(txtIP1.Text);
                if (i < 0 || i > 256)
                {
                    txtIP4.Clear();
                    MessageBox.Show("Wrong IP Address", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                txtIP4.Clear();
                MessageBox.Show("Wrong IP Address", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            String s = txtIP1.Text + "." + txtIP2.Text + "." + txtIP3.Text + "." + txtIP4.Text;
            if (PingtoIP(s))
            {
                Zabbix zabbix = new Zabbix(txtUsername.Text, txtPassword.Text,"http://" +  s + "/zabbix/api_jsonrpc.php");
                Boolean Logged = zabbix.login();
                if (Logged)
                {

                    zabbix = new Zabbix(txtUsername.Text.ToString(), txtPassword.Text.ToString(), "http://" + s + "/zabbix/api_jsonrpc.php");
                    Form2 f = new Form2(zabbix);
                    this.Hide();
                    f.ShowDialog();
                    this.Show();

                }
                else
                {
                    MessageBox.Show("Cannot login to Zabbix\n Wrong username or password");
                }
                //zabbix.logout();
            }
            else
            {
                MessageBox.Show("Cannot connect to this IP address " + s + "\nConnect with other IP.", "Error", MessageBoxButtons.OK);
            }
        }

        public bool PingtoIP(String host)
        {
            Ping p = new Ping();
            try
            {
                PingReply pr = p.Send(host, 3000);
                if (pr.Status == IPStatus.Success)
                {
                    return true;
                }
            }
            catch (Exception)
            {


            }
            return false;
        }
    }
}
