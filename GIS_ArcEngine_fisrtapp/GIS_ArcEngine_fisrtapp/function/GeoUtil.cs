using ESRI.ArcGIS.Carto;
using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.DataSourcesFile;
using ESRI.ArcGIS.Geodatabase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GIS_ArcEngine_fisrtapp.function
{
    class GeoUtil
    {
        private AxMapControl mapControl;
        public GeoUtil()
        {
            //
        }
        public GeoUtil(AxMapControl mapControl)
        {
            this.mapControl = mapControl;
        }
        public void loadShapefile(string shpFileName)
        {
            // 文件格式判断
            if(shpFileName.Substring(shpFileName.LastIndexOf(".")) != ".shp")
            {
                MessageBox.Show("请选择shp文件");
                return;
            }
            // 文件打开
            string filePath = "";
            string fileName = "";
            int index = shpFileName.LastIndexOf("\\");  // 14
            Console.WriteLine(index);
            if(index > 0)
            {
                filePath = shpFileName.Substring(0, index);
                string temp = shpFileName.Substring(index + 1);
                fileName = temp.Substring(0, temp.LastIndexOf("."));
            }

            IWorkspaceFactory pWorkspaceFactory;
            IFeatureWorkspace pFeatureWorkspace;
            IFeatureClass pFeatureClass;
            IFeatureLayer pFeatureLayer;
            pWorkspaceFactory = new ShapefileWorkspaceFactory();
            IWorkspace pWorkspace = pWorkspaceFactory.OpenFromFile(filePath, 0);
            pFeatureWorkspace = pWorkspace as IFeatureWorkspace;
            // 将shp数据读入内存 --- >> pFeatureClass
            pFeatureClass = pFeatureWorkspace.OpenFeatureClass(fileName);  // 练习ArcMap

            //pFeatureLayer = new FeatureLayer();
            //pFeatureLayer.FeatureClass = pFeatureClass;
            //pFeatureLayer.Name = "region";
            //axMapControl1.AddLayer(pFeatureLayer);
            //axMapControl1.Refresh();

            // 用于图形渲染   ---->   pFeatureLayer
            pFeatureLayer = new FeatureLayer();
            pFeatureLayer.FeatureClass = pFeatureClass;
            IDataset pDataset = pFeatureClass as IDataset;
            pFeatureLayer.Name = pDataset.Name;  // set
            //pFeatureLayer.Name = "afdsafdsafs";  // set
            //string s = pFeatureLayer.Name; // get
            ILayer pLayer = pFeatureLayer as ILayer;
            this.mapControl.Map.AddLayer(pLayer);
            this.mapControl.Refresh();
        }
    }
}
