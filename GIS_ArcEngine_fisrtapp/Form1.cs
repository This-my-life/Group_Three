using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Geodatabase;
using GIS_ArcEngine_fisrtapp.function;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GIS_ArcEngine_fisrtapp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //axMapControl1.LoadMxFile(@"D:\luwei\test2\八维建筑.mxd");
            //test
        }

        private void mxdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "shp文件|*.shp|mxd文件|*.mxd|ext files (*.txt)|*.txt|All files(*.*)|*>**";
            if (open.ShowDialog() == DialogResult.OK)
            {
                if (open.FileName != null || open.FileName != "")
                {
                    axMapControl1.LoadMxFile(open.FileName);
                }
            }
            else
            {
                MessageBox.Show("请选择文件");
                return;
            }
        }

        private void shpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GeoUtil util = new GeoUtil(axMapControl1);
            OpenFileDialog dialog = new OpenFileDialog();
            if(dialog.ShowDialog() == DialogResult.OK)
            {
                util.loadShapefile(dialog.FileName);
            }
        }

        int i = 0;
        DataTable dt = new DataTable();
        private void button1_Click(object sender, EventArgs e)
        {
            // Column
            dt.Columns.Add(new DataColumn("字段"+i, typeof(string)));
            // Row
            string [] rowStr = { "你好" + i };
            dt.Rows.Add(rowStr);

            dataGridView1.DataSource = dt;
            i++;
        }

        private void mySQLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //122333
        }
    }
}
