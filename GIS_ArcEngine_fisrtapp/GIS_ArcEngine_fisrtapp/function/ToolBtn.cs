using ESRI.ArcGIS.Controls;
using ESRI.ArcGIS.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIS_ArcEngine_firstApp.function
{
    class ToolBtn
    {
        private AxMapControl mapControl;
        public ToolBtn(AxMapControl mapControl)
        {
            this.mapControl = mapControl;
        }
        public void pan()
        {
            mapControl.Pan();
        }
        public void fullExtent()
        {
            mapControl.Extent = mapControl.FullExtent;
        }
        public void zoomIn()
        {
            IEnvelope pEnvelope = mapControl.TrackRectangle();
            mapControl.Extent = pEnvelope;
            mapControl.Refresh();
        }

        public void zoomOut()
        {
            IEnvelope pEnvelope1 = mapControl.TrackRectangle();
            IEnvelope pEnvelope2 = mapControl.Extent;

            double w = pEnvelope2.Width / pEnvelope1.Width;  // 动态设置缩放比例
            double h = pEnvelope2.Height / pEnvelope1.Height;  // 动态设置缩放比例

            pEnvelope2.Expand(w, h, true);
            mapControl.Extent = pEnvelope2;
            mapControl.Refresh();
        }
        public static double Distance(double logitude1, double latitude1, double logitude2, double latitude2)
        {
            double lat1 = rad(latitude1);
            double lat2 = rad(latitude1);
            double a = lat1 - lat2;
            double b = rad(logitude1) - rad(logitude2);
            double s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(a / 2), 2) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Pow(Math.Sin(b / 2), 2)));
            s = s * 6378137.0;
            s = Math.Round(s * 10000d) / 10000d;
            return s;
        }
        public static double rad(double d)
        {
            // 角度转换成弧度
            return d * Math.PI / 180.00;
        }
    }
}
