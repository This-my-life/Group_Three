using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIS_ArcEngine_firstApp.function
{
    enum Tools
    {
        Pointer,   // 指针
        ZoomIn,    // 放大
        ZoomOut,   // 缩小
        Pan,       // 平移
        FullExtent,// 全图
        Distance,  // 长度
        Area,      // 面积
        Overlap,    // 叠加
        DrawPolygon, //添加面元素
        PointBuffer, //添加点缓冲
        none,
        pan,
        Identify,
        Measure,
        MeasureArea,
        DrawPoint,
        DrawPolyline,
        CameraShow,
        SelectFeature,
        PolyCutLayer,
        SplitPolygon,
        SplitPolyline,
        BufferPoint,
        BufferPolyline,
        BufferPolygon,
        ExportRegion,
        RoutePlanning
    }
}
