using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using ESRI.ArcGIS.SystemUI;
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
        
        private void clipToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (Clip_Window CW_01 = new Clip_Window(axMapControl1))
            {
                CW_01.ShowDialog();
            }
        }
        private void mySQLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //122333
        }

        private void mergeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FormMerge(axMapControl1).Show();
        }

        private void dissolveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new FormDissolve(axMapControl1).Show();
        }

        private void face面ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建图形
            IPolygon pPolygon = new Polygon() as IPolygon;
            IRubberBand pRubberBand = new RubberPolygon();
            pPolygon = (IPolygon)pRubberBand.TrackNew(axMapControl1.ActiveView.ScreenDisplay, null);
            // 创建符号
            ISimpleLineSymbol pSimpleLineSymbol = new SimpleLineSymbol();
            pSimpleLineSymbol.Width = 2;
            pSimpleLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
            //pSimpleLineSymbol.Color = GetRGBColor(46, 24, 63);// Common.GetRGBColor(46, 24, 63);

            ISimpleFillSymbol pSimpleFillSymbol = new SimpleFillSymbol();
            //pSimpleFillSymbol.Color = GetRGBColor(11, 200, 145);
            pSimpleFillSymbol.Outline = pSimpleLineSymbol;
            // 创建Element并赋值图形和符号
            IFillShapeElement pPolygonElement = new PolygonElement() as IFillShapeElement;
            IElement pElement = (IElement)pPolygonElement;
            pElement.Geometry = pPolygon;
            pPolygonElement.Symbol = pSimpleFillSymbol;

            // 添加到IGraphicsContainer容器
            IGraphicsContainer pGraphicsContainer = (IGraphicsContainer)axMapControl1.Map;
            pGraphicsContainer.AddElement((IElement)pPolygonElement, 0);
            axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);

        }

        private void point绘制点ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
