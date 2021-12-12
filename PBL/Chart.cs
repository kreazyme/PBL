using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PBL
{
    public partial class Chart : Form
    {
        public Chart()
        {
            InitializeComponent(); LoadIMG();
        }
        private void LoadIMG()
        {
            pictureBox1.ImageLocation = "http://192.168.96.143/zabbix/chart.php?from=now-1h&to=now&itemids%5B0%5D=38692&type=0&profileIdx=web.item.graph.filter&profileIdx2=38678&width=1000&height=200&_=v6w0fr7d"; //path to image
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
        }
    }
}
