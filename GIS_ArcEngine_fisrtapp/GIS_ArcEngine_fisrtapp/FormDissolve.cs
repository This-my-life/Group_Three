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
    public partial class FormDissolve : Form
    {
        private AxMapControl axMapControl1;
        private bool flag = true;
        // 三个关键参数
        private IFeatureLayer pFeatureLayer = null;
        private string ShapeType = "";
        private String path = null;
        private String name = null;
        private int count = 0;
        private IFeatureLayer[] arr = new IFeatureLayer[8];
        public FormDissolve()
        {
            InitializeComponent();
        }
        public FormDissolve(AxMapControl axMapControl1)
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
                    pFeatureLayer = axMapControl1.get_Layer(i) as IFeatureLayer;
                    string temp = pFeatureLayer.FeatureClass.ShapeType.ToString();
                    if (ShapeType == "")
                    {
                        ShapeType = temp;
                        Console.WriteLine(temp);
                        listBox1.Items.Add(comboBox1.SelectedItem.ToString());
                        arr[this.count++] = pFeatureLayer;
                        // AddMerge(pFeatureLayer);
                    }
                    else if (ShapeType == temp)
                    {
                        listBox1.Items.Add(comboBox1.SelectedItem.ToString());
                        // AddMerge(pFeatureLayer);
                        arr[this.count++] = pFeatureLayer;
                    }
                    else
                    {
                        MessageBox.Show("请选择相同的Shape文件");
                    }
                }

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool f = true;
            if (this.ShapeType == "esriGeometryPolygon")
            {
                Console.WriteLine("开始");
                IFeature pFeature = null;
                IFeatureCursor pFeatureCursor = null;
                IFeatureLayer pFeatureLayer = null;
                IGeometry UnionGeometry = null;
                for (int i = 0; i < this.count; i++)
                {

                    // 获取ifeaturelayer接口
                    pFeatureLayer = (IFeatureLayer)arr[i];
                    // 空间查询返回所有的面
                    pFeatureCursor = pFeatureLayer.FeatureClass.Search(null, false);
                    pFeature = pFeatureCursor.NextFeature(); // 获取每一个feature
                    if (pFeature != null)
                    {
                        // 初始化存放合并图元的变量，初始形状为第一个图元
                        if (f)
                        {
                            UnionGeometry = pFeature.Shape;
                            pFeature = pFeatureCursor.NextFeature(); // 指向下一个
                            f = false;
                        }
                        while (pFeature != null)
                        {
                            // 获取拓扑接口
                            ITopologicalOperator pTopologicalOperator = pFeature.Shape as ITopologicalOperator;
                            // 调用拓扑接口的合并方法
                            UnionGeometry = pTopologicalOperator.Union(UnionGeometry);
                            // 闪烁图元
                            // axMapControl1.FlashShape(UnionGeometry, 5, 300, null);
                            pFeature = pFeatureCursor.NextFeature(); // 指向下一个
                        }

                    }
                }
                axMapControl1.FlashShape(UnionGeometry, 5, 300, null); // 合并完之后再闪烁一下
            }
        }

        private void comboBox1_Click(object sender, EventArgs e)
        {
            if (axMapControl1.LayerCount > 0 && flag)
            {
                for (int i = 0; i < axMapControl1.LayerCount; i++)
                {
                    comboBox1.Items.Add(axMapControl1.get_Layer(i).Name);
                    // pFeatureLayer = axMapControl1.get_Layer(i) as IFeatureLayer;
                }
                flag = false;
            }
        }
    }
}
