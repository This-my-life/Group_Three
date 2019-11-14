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
        ToolEnum tool = ToolEnum.Pointer;
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
            tool = ToolEnum.DrawPolygon;
        }

        public void createDrawPolygon()
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

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            tool = ToolEnum.PointBuffer;
            //axMapControl1.MousePointer = ESRI.ArcGIS.Controls
        }

        public void createPointBuffer()
        {
            // ITopologicalOperator  ===>  IGeometry as ITopologicalOperator
            IGeometry geometry = axMapControl1.TrackLine();  // IRubberBand   ----   橡皮筋
            ITopologicalOperator topoOper = geometry as ITopologicalOperator;
            IPolygon geoPolygon = topoOper.Buffer(1) as IPolygon;
            // IElement

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
            pElement.Geometry = geoPolygon;
            pPolygonElement.Symbol = pSimpleFillSymbol;

            // 添加到IGraphicsContainer容器
            IGraphicsContainer pGraphicsContainer = (IGraphicsContainer)axMapControl1.Map;
            pGraphicsContainer.AddElement((IElement)pPolygonElement, 0);
            axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        }

        private void axMapControl1_OnMouseDown(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnMouseDownEvent e)
        {
            switch (tool)
            {
                case ToolEnum.DrawPolygon:
                    //添加面元素
                    this.createDrawPolygon();
                    break;
                case ToolEnum.PointBuffer:
                    //缓冲区代码
                    this.createPointBuffer();
                    break;
                case ToolEnum.ZoomIn: // 放大
                    // 放大代码
                    break;
                case ToolEnum.ZoomOut:
                    // 缩小代码
                    break;
                default:
                    break;
            }
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            tool = ToolEnum.Pointer;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string sMxdFileNmae = axMapControl1.DocumentFilename;//调用地图控件自身的方法来获取mxd文档的名称
                IMapDocument pMapDocument = new MapDocument();//
                if (sMxdFileNmae != null && axMapControl1.CheckMxFile(sMxdFileNmae))//不等于空且检查文件是否损坏
                {
                    if (pMapDocument.get_IsReadOnly(sMxdFileNmae))//如果文件为只读状态
                    {
                        MessageBox.Show("地图当前为只读状态，不能保存！");
                        pMapDocument.Close();
                        return;//结束方法
                    }
                }
                else
                {
                    SaveFileDialog pSaveFileDiaLog = new System.Windows.Forms.SaveFileDialog();//创建一个保存文件对话框
                    pSaveFileDiaLog.Title = "请选择保存的路径";//标题
                    pSaveFileDiaLog.Filter = "ArcMap文档(*.mxd)|*.mxd|ArcMap模板(*.mxt)|*.mxt";//Filter过滤  过滤除指定格式外文件
                    pSaveFileDiaLog.OverwritePrompt = true;//如果已存在文件是否覆盖
                    pSaveFileDiaLog.RestoreDirectory = true;//保存进程目录之后 是否打开
                    if (pSaveFileDiaLog.ShowDialog() == DialogResult.OK)//确定
                    {
                        sMxdFileNmae = pSaveFileDiaLog.FileName;
                    }
                    else
                    {
                        return;
                    }
                }
                pMapDocument.New(sMxdFileNmae);//创建一个工作空间
                pMapDocument.ReplaceContents(axMapControl1.Map as IMxdContents);//通过axMapControl1.Map获取IMxdContents来更新文档内容
                pMapDocument.Save(pMapDocument.UsesRelativePaths, true);//已当前目录来保存
                pMapDocument.Close();
                MessageBox.Show("保存文档成功");
            }
            catch (Exception ex)
            {//存放错误信息弹出错误信息
                MessageBox.Show(ex.Message);
            }
        }

        private void saveASToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SaveFileDialog pSaveFileDiaLog = new System.Windows.Forms.SaveFileDialog();
                pSaveFileDiaLog.Title = "另存为";
                pSaveFileDiaLog.Filter = "ArcMap文档(*.mxd)|*.mxd|ArcMap模板(*.mxt)|*.mxt";
                pSaveFileDiaLog.RestoreDirectory = true;
                if (pSaveFileDiaLog.ShowDialog() == DialogResult.OK)
                {
                    string sFilePath = pSaveFileDiaLog.FileName;
                    IMapDocument pMapDocument = new MapDocument();
                    pMapDocument.New(sFilePath);
                    pMapDocument.ReplaceContents(axMapControl1.Map as IMxdContents);
                    pMapDocument.Save(true, true);
                    pMapDocument.Close();
                    MessageBox.Show("另存为成功！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
