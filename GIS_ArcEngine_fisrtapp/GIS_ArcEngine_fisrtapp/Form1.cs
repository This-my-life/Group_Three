
using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Display;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
using GIS_ArcEngine_firstApp.function;
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
        private Tools tool;
        private ToolBtn toolbtn;
        private int layerComboBoxSelectedIndex = -1;
        private int fieldComboBoxSelectedIndex = -1;
        private int lAYER_COUNT = -1;
        private int fields_count = -1;
        public Form1()
        {
            InitializeComponent();
            axTOCControl1.SetBuddyControl(axMapControl1);
            tool = Tools.Pointer;
            toolbtn = new ToolBtn(axMapControl1);
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
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                util.loadShapefile(dialog.FileName);
                this.lAYER_COUNT = axMapControl1.LayerCount;
            }
        }

        
        private void button1_Click(object sender, EventArgs e)
        {
            /////////////
            if (axMapControl1.LayerCount == 0)
            {
                MessageBox.Show("请加载图层");
                return;
            }
            /////////////  图层combobox
            //string str = toolStripComboBox1.Items[toolStripComboBox1.SelectedIndex].ToString();
            if (toolStripComboBox1.SelectedIndex == -1)
            {
                this.initToolStripComboBox(toolStripComboBox1);
            }
            else
            {
                this.initToolStripComboBox(toolStripComboBox1);
            }

            {
                DataTable dt = new DataTable();
                IFeatureLayer getLayer = axMapControl1.get_Layer(toolStripComboBox1.SelectedIndex) as IFeatureLayer;
                IFeatureClass featureclass = getLayer.FeatureClass;
                IQueryFilter queryFilter = new QueryFilter();
                if (textBox1.Text == "")
                {
                    MessageBox.Show("请输入");

                }
                else
                {
                    string filter = comboBox1.Items[comboBox1.SelectedIndex].ToString() + "=" + "'" + textBox1.Text + "'";
                    Console.WriteLine(filter); 
                    IQueryFilter Filter = new QueryFilter();
                    Filter.WhereClause = filter;
                    IFeatureCursor featurecursor = getLayer.Search(Filter,false);
                    IFields fields = featurecursor.Fields;
                    for (int i = 0; i<fields.FieldCount; i++)
                    {
                        IField field = fields.Field[i];
                        string name = field.Name;
                        DataColumn dc = new DataColumn(name, typeof(String));
                        dt.Columns.Add(dc);  
                    }
                    IFeature feature = null; 
                    while((feature = featurecursor.NextFeature()) != null)
                    {
                        object[] objs = new object[fields.FieldCount];
                        ITable t = feature.Table;
                        ICursor Cursor = t.Search(Filter, false);
                        IRow row = null;
                        while ((row = Cursor.NextRow()) != null)
                        {
                            for (int i = 0; i < fields.FieldCount; i++)
                            {
                                objs[i] = row.Value[i];
                            }
                        }
                        dt.Rows.Add(objs); 
                    }
                    dataGridView1.DataSource =dt ; 
                }

            }
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

        }

        private void line绘制线ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // 创建图形
            IPolyline pPolyline = new Polyline() as IPolyline;
            IRubberBand pRubberBand = new RubberLine();
            pPolyline = pRubberBand.TrackNew(axMapControl1.ActiveView.ScreenDisplay, null) as IPolyline;
            // 创建符号
            ISimpleLineSymbol pSimpleLineSymbol = new SimpleLineSymbol();
            pSimpleLineSymbol.Width = 2;
            pSimpleLineSymbol.Style = esriSimpleLineStyle.esriSLSSolid;
            // pSimpleLineSymbol.Color = GetRGBColor(44, 44, 44);

            // 创建Element并赋值图形和符号
            ILineElement pLineElement = new LineElement() as ILineElement;
            IElement pElement = pLineElement as IElement;
            pElement.Geometry = pPolyline;
            pLineElement.Symbol = pSimpleLineSymbol;
            // 添加到IGraphicsContainer容器
            IGraphicsContainer pGraphicsContainer = axMapControl1.Map as IGraphicsContainer;
            pGraphicsContainer.AddElement(pLineElement as IElement, 0);
            axMapControl1.ActiveView.PartialRefresh(esriViewDrawPhase.esriViewGraphics, null, null);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }






        private void comboBox1_Click(object sender, EventArgs e)
        {
            if (axMapControl1.LayerCount == 0)
            {
                MessageBox.Show("请加载图层");
            }
            else
            {
                //comboBox1.Items.Clear();
                if (toolStripComboBox1.Text == "")
                {
                    MessageBox.Show("请先选择图层");
                }
                else
                {
                    this.fieldComboBoxSelectedIndex = comboBox1.SelectedIndex;
                    //this.initFieldsComboBox(comboBox1);
                    //ILayer layer = axMapControl1.get_Layer(toolStripComboBox1.SelectedIndex);  // 获取axMapControl1中的图层
                    //if (layer is IFeatureLayer) // 读取矢量数据
                    //{
                    //    IFeatureClass featureClass = (layer as IFeatureLayer).FeatureClass;
                    //    IFeatureCursor feaCursor = featureClass.Search(null, false);//第一个参数表示要搜索的东西
                    //    IFields fields = feaCursor.Fields;  // 字段
                    //    for (int i = 0; i < fields.FieldCount; i++)
                    //    {
                    //        IField field = fields.Field[i];
                    //        comboBox1.Items.Add(field.Name);
                    //    }
                    //}
                    //else
                    //{
                    //    // 栅格数据
                    //    IRasterLayer rasterlayer = layer as IRasterLayer;
                    //}
                }
            }
        }






        private void toolStripComboBox1_Click(object sender, EventArgs e)
        {
            if (axMapControl1.LayerCount == 0)
            {
                MessageBox.Show("当前没有图层");
            }
            else
            {
                this.layerComboBoxSelectedIndex = toolStripComboBox1.SelectedIndex;
                this.initToolStripComboBox(toolStripComboBox1);

                //int layerCount = axMapControl1.LayerCount;
                //toolStripComboBox1.Items.Clear();
                //for (int i = 0; i < layerCount; i++)
                //{
                //    ILayer layer = axMapControl1.get_Layer(i);
                //    if ((layer != null) && (layer is IFeatureLayer))
                //    {
                //        string name = (layer as IDataset).Name;
                //        toolStripComboBox1.Items.Add(name);
                //    }
                //}
            }
        }

        public void initToolStripComboBox(ToolStripComboBox toolStripComboBox1)
        {
            int layerCount = axMapControl1.LayerCount;
            if (layerCount == 0)
            {
                return;
            }

            //toolStripComboBox1.Items.Clear();
            if(this.lAYER_COUNT == axMapControl1.LayerCount && toolStripComboBox1.Items.Count > 0)
            {
                return;
            }
            toolStripComboBox1.Items.Clear();
            for (int i = 0; i < layerCount; i++)
            {
                ILayer layer = axMapControl1.get_Layer(i);
                if ((layer != null) && (layer is IFeatureLayer))
                {
                    string name = (layer as IDataset).Name;
                    toolStripComboBox1.Items.Add(name);
                }
            }

            //private int LAYER_COMBOX_SELECTEDINDEX /fieldComboBoxSelectedIndex
            if (layerComboBoxSelectedIndex != -1)
            {
                if(layerComboBoxSelectedIndex >= toolStripComboBox1.Items.Count)
                {
                    layerComboBoxSelectedIndex = -1;
                    toolStripComboBox1.SelectedIndex = 0;
                }
                else
                {
                    toolStripComboBox1.SelectedIndex = layerComboBoxSelectedIndex;
                }
                
            }
            else {
                toolStripComboBox1.SelectedIndex = 0;
            }
        }

        public void initFieldsComboBox(ComboBox comboBox1)
        {
            int layerCount = axMapControl1.LayerCount;
            if (layerCount == 0)
            {
                return;
            }
            //comboBox1.Items.Clear();
            ILayer layer = axMapControl1.get_Layer(toolStripComboBox1.SelectedIndex);  // 获取axMapControl1中的图层
            
            if (layer is IFeatureLayer) // 读取矢量数据
            {
                IFeatureClass featureClass = (layer as IFeatureLayer).FeatureClass;
                IFeatureCursor feaCursor = featureClass.Search(null, false);//第一个参数表示要搜索的东西
                IFields fields = feaCursor.Fields;  // 字段
                for (int i = 0; i < fields.FieldCount; i++)
                {
                    IField field = fields.Field[i];
                    comboBox1.Items.Add(field.Name);
                }
            }
            else
            {
                // 栅格数据
                IRasterLayer rasterlayer = layer as IRasterLayer;
            }

            //private int fieldComboBoxSelectedIndex = -1;
            if (fieldComboBoxSelectedIndex != -1)
            {
                if (fieldComboBoxSelectedIndex >= comboBox1.Items.Count)
                {
                    fieldComboBoxSelectedIndex = -1;
                    comboBox1.SelectedIndex = 0;
                }
                else
                {
                    comboBox1.SelectedIndex = fieldComboBoxSelectedIndex;
                }

            }
            else
            {
                comboBox1.SelectedIndex = 0;
            }
            
        }

        private void axMapControl1_ControlAdded(object sender, ControlEventArgs e)
        {
            Console.WriteLine("组件添加中");
        }

        private void toolStripComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            this.initFieldsComboBox(comboBox1);
        }

        private void toolStripComboBox1_TextChanged(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            this.initFieldsComboBox(comboBox1);
        }

        private void axMapControl1_OnMouseDown(object sender, ESRI.ArcGIS.Controls.IMapControlEvents2_OnMouseDownEvent e)
        {
            switch (tool)
            {
                case Tools.DrawPolygon:
                    //缓冲区代码
                    this.createDrawPolygon();
                    break;
                case Tools.PointBuffer:
                    //缓冲区代码
                    this.createPointBuffer();
                    break;
                case Tools.ZoomIn:
                    toolbtn.zoomIn();
                    break;
                case Tools.ZoomOut:
                    toolbtn.zoomOut();
                    break;
                case Tools.Pan:
                    toolbtn.pan();
                    break;
                case Tools.Distance:
                    IGeometry pline = axMapControl1.TrackLine();
                    IPointCollection pc = pline as IPointCollection;
                    if (pc != null)
                    {
                        double totalLen = 0.0;
                        int iCount = pc.PointCount;
                        for (int j = 0; j < iCount - 1; j++)
                        {
                            IPoint pt1 = pc.get_Point(j);
                            IPoint pt2 = pc.get_Point(j + 1);
                            totalLen += ToolBtn.Distance(pt1.X, pt1.Y, pt2.X, pt2.Y);
                        }
                        MessageBox.Show(String.Format("{0}米", totalLen));
                    }
                    break;
                case Tools.Area:
                    IGeometry pPoly = axMapControl1.TrackPolygon();
                    ITopologicalOperator pTopo = pPoly as ITopologicalOperator;
                    pTopo.Simplify();
                    ISpatialReferenceFactory spatialReferenceFactory = new SpatialReferenceEnvironment();
                    ISpatialReference earthref = spatialReferenceFactory.CreateGeographicCoordinateSystem((int)esriSRGeoCSType.esriSRGeoCS_WGS1984);
                    pPoly.SpatialReference = earthref;
                    IPointCollection pPc = pPoly as IPointCollection;
                    IPoint pt = pPc.get_Point(0);
                    int dh = (int)((pt.X + 6.0) / 6);
                    int wikiid = 21413 + (dh - 13);
                    pPoly.Project(spatialReferenceFactory.CreateProjectedCoordinateSystem(wikiid));
                    IArea pArea = pPoly as IArea;
                    MessageBox.Show(string.Format("{0}平方米", pArea.Area));
                    break;
                case Tools.Overlap:
                    break;
                case Tools.DrawPoint:
                    IElement pEle;
                    IPoint p = new ESRI.ArcGIS.Geometry.Point();
                    p.X = e.mapX;
                    p.Y = e.mapY;
                    IGraphicsContainer pGra = axMapControl1.Map as IGraphicsContainer;
                    IMarkerElement pMakEle = new MarkerElement() as IMarkerElement;
                    pEle = pMakEle as IElement;
                    IMarkerSymbol pMakSym = new SimpleMarkerSymbol();
                    RgbColor pRgbColor = new RgbColor();
                    pRgbColor.Red = 0;
                    pRgbColor.Green = 0;
                    pRgbColor.Blue = 255;
                    pMakSym.Color = pRgbColor;
                    pMakEle.Symbol = pMakSym;
                    pEle.Geometry = p;
                    pGra.AddElement(pEle, 0);
                    axMapControl1.Refresh(esriViewDrawPhase.esriViewGraphics, null, null);
                    break;
                default:
                    break;
            }
        }

        private void overlapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tool = Tools.Overlap;
            new Overlap(axMapControl1).Show();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            axMapControl1.MousePointer = ESRI.ArcGIS.Controls.esriControlsMousePointer.esriPointerDefault;
            tool = Tools.Pointer;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            axMapControl1.MousePointer = ESRI.ArcGIS.Controls.esriControlsMousePointer.esriPointerZoomIn;
            tool = Tools.ZoomIn;
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            axMapControl1.MousePointer = ESRI.ArcGIS.Controls.esriControlsMousePointer.esriPointerZoomOut;
            tool = Tools.ZoomOut;
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            axMapControl1.MousePointer = ESRI.ArcGIS.Controls.esriControlsMousePointer.esriPointerPan;
            tool = Tools.Pan;
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            toolbtn.fullExtent();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            axMapControl1.MousePointer = esriControlsMousePointer.esriPointerCrosshair;
            tool = Tools.Distance;
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            axMapControl1.MousePointer = esriControlsMousePointer.esriPointerCrosshair;
            tool = Tools.Area;
        }

        private void mergeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FormMerge merge = new FormMerge(axMapControl1))
            {
                merge.ShowDialog();
            }
        }

        private void dissolveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (FormDissolve formdis = new FormDissolve(axMapControl1))
            {
                formdis.ShowDialog();
            }
            
        }

        private void face面ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            tool = Tools.DrawPolygon;
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

        private void point绘制点ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            axMapControl1.MousePointer = esriControlsMousePointer.esriPointerCrosshair;
            tool = Tools.DrawPoint;
        }

        private void bufferToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Form_buffer(axMapControl1).Show();
        }

        private void erase擦除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Form_erase(axMapControl1).Show();
        }

        private void toolStripContainer1_TopToolStripPanel_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton4_Click_1(object sender, EventArgs e)
        {
            tool = Tools.PointBuffer;
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
        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            tool = Tools.Pointer;
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

        private void attributionQueryToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void spatialQueryToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void axMapControl1_OnMouseMove(object sender, IMapControlEvents2_OnMouseMoveEvent e)
        {
            toolStripLabel3.Text = " 当前坐标 X = " + e.mapX.ToString("F8") + " Y = " + e.mapY.ToString("F8");
        }
    }
}
