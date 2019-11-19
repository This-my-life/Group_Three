using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geodatabase;
using ESRI.ArcGIS.Geometry;
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
    public partial class Overlap : Form
    {
        private AxMapControl axMapControl1;
        private IFeatureLayer layer1 = null;
        private IFeatureLayer layer2 = null;
        private bool flag1 = true;
        private bool flag2 = true;

        public Overlap()
        {
            InitializeComponent();
        }

        public Overlap(AxMapControl axMapControl1)
        {
            InitializeComponent();
            this.axMapControl1 = axMapControl1;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < axMapControl1.LayerCount; i++)
            {
                if (axMapControl1.get_Layer(i).Name == comboBox1.SelectedItem.ToString())
                {
                    layer1 = axMapControl1.get_Layer(i) as IFeatureLayer;
                }
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < axMapControl1.LayerCount; i++)
            {
                if (axMapControl1.get_Layer(i).Name == comboBox2.SelectedItem.ToString())
                {
                    layer2 = axMapControl1.get_Layer(i) as IFeatureLayer;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (layer1 != null && layer2 != null)
            {
                bool f = true;
                IGeometry UnionGeometry = null; // 将叠加的图层合并到一起
                List<string> resList = new List<string>(); // 存放分析的结果
                // 通过图层的name属性来获得ilayer接口
                IFeatureLayer pFeatureLayer = (IFeatureLayer)layer1; // 转换为ifeaturelayer接口
                                                                     // 把每个面都取出来
                IFeatureCursor pFeatureCursor = pFeatureLayer.FeatureClass.Search(null, false);
                IFeature pFeature = pFeatureCursor.NextFeature(); // 指向结果集的下一个要素
                while (pFeature != null) // 如果不为空
                {
                    // 获取被叠加图层的ilayer接口
                    IFeatureLayer pByFeatureLayer = (IFeatureLayer)layer2; // 再获取ifeaturelayer接口
                                                                           // new 一个空间查询过滤器
                    ISpatialFilter pSpatialFilter = new SpatialFilter();
                    if (pByFeatureLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPoint)
                    {
                        pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelContains; // 包含
                    }
                    else if (pByFeatureLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolyline)
                    {
                        pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects; // 相交
                    }
                    else if (pByFeatureLayer.FeatureClass.ShapeType == esriGeometryType.esriGeometryPolygon)
                    {
                        pSpatialFilter.SpatialRel = esriSpatialRelEnum.esriSpatialRelIntersects; // 相交
                    }
                    pSpatialFilter.Geometry = pFeature.Shape; // 把每一个叠加图层赋值给空间过滤器
                                                              // 调用被叠加图层的search方法查询有多少图元
                    IFeatureCursor pByFeatureCursor = pByFeatureLayer.FeatureClass.Search(pSpatialFilter, false);
                    IFeature pByFeature = pByFeatureCursor.NextFeature(); // 指向结果集的下一个要素
                    while (pByFeature != null)
                    {
                        if (f)
                        {
                            UnionGeometry = pByFeature.Shape;
                            pFeature = pByFeatureCursor.NextFeature(); // 指向下一个
                            f = false;
                        }
                        else
                        {
                            // 获取拓扑接口
                            ITopologicalOperator pTopologicalOperator = pByFeature.Shape as ITopologicalOperator;
                            // 调用拓扑接口的合并方法
                            UnionGeometry = pTopologicalOperator.Union(UnionGeometry);
                            // 闪烁图元
                            // this.axMapControl1.FlashShape(UnionGeometry, 5, 300, null);
                            pByFeature = pByFeatureCursor.NextFeature(); // 指向下一个
                        }
                    }
                    pFeature = pFeatureCursor.NextFeature(); // 指向结果集的下一个要素
                }
                this.axMapControl1.FlashShape(UnionGeometry, 5, 300, null);

            }
            else
            {
                if (layer1 == null)
                    MessageBox.Show("请选择叠加的图层");
                if (layer2 == null)
                    MessageBox.Show("请选择被叠加的图层");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Overlap_Load(object sender, EventArgs e)
        {

        }

        private void comboBox1_Click(object sender, EventArgs e)
        {
            if (axMapControl1.LayerCount > 0 && flag1)
            {
                for (int i = 0; i < axMapControl1.LayerCount; i++)
                {
                    comboBox1.Items.Add(axMapControl1.get_Layer(i).Name);
                }
                flag1 = false;
            }
        }

        private void comboBox2_Click(object sender, EventArgs e)
        {
            if (axMapControl1.LayerCount > 0 && flag2)
            {
                for (int i = 0; i < axMapControl1.LayerCount; i++)
                {
                    comboBox2.Items.Add(axMapControl1.get_Layer(i).Name);
                }
                flag2 = false;
            }
        }

        private void comboBox1_Click_1(object sender, EventArgs e)
        {
            if (axMapControl1.LayerCount > 0 && flag1)
            {
                for (int i = 0; i < axMapControl1.LayerCount; i++)
                {
                    comboBox1.Items.Add(axMapControl1.get_Layer(i).Name);
                }
                flag1 = false;
            }
        }

        private void comboBox2_Click_1(object sender, EventArgs e)
        {
            if (axMapControl1.LayerCount > 0 && flag2)
            {
                for (int i = 0; i < axMapControl1.LayerCount; i++)
                {
                    comboBox2.Items.Add(axMapControl1.get_Layer(i).Name);
                }
                flag2 = false;
            }
        }
    }
}
