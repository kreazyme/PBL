using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PBL
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Graph("http://192.168.96.143/zabbix/api_jsonrpc.php", 38678));
            //("http://192.168.96.143/zabbix/api_jsonrpc.php", 38692
        }
    }
}
