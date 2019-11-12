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
    public partial class FormMerge : Form
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

        public FormMerge()
        {
            InitializeComponent();
        }

        public FormMerge(AxMapControl axMapControl1)
        {
            InitializeComponent();
            this.axMapControl1 = axMapControl1;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < axMapControl1.LayerCount; i++)
            {
                if (axMapControl1.get_Layer(i).Name == comboBox1.SelectedItem.ToString())
                {
                    pFeatureLayer = axMapControl1.get_Layer(i) as IFeatureLayer;
                }
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

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            IPolygon polygon = union(pFeatureLayer.FeatureClass);
            axMapControl1.FlashShape(polygon, 5, 300, null); // 合并完之后再闪烁一下
        }
        public IPolygon union(IFeatureClass featureClass)

        {


            if (featureClass == null)

            { return null; }


            IGeoDataset geoDataset = featureClass as IGeoDataset;

            IGeometry geometryBag = new GeometryBag();

            ISpatialFilter queryFilter = new SpatialFilter();

            geometryBag.SpatialReference = geoDataset.SpatialReference;

            IFeatureCursor featureCursor = featureClass.Search(queryFilter, false);

            IGeometryCollection geometryCollection = geometryBag as IGeometryCollection;

            IFeature currentFeature = featureCursor.NextFeature();

            while (currentFeature != null)
            {

                object missing = Type.Missing;

                geometryCollection.AddGeometry(currentFeature.Shape, ref missing, ref missing);

                currentFeature = featureCursor.NextFeature();

            }

            ITopologicalOperator unionedPolygon = new Polygon() as ITopologicalOperator;

            unionedPolygon.ConstructUnion(geometryBag as IEnumGeometry);

            return unionedPolygon as IPolygon;

        }
    }
}
