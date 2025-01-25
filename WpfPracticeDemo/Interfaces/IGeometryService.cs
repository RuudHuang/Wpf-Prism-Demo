using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using WpfPracticeDemo.Enums;
using WpfPracticeDemo.Models;

namespace WpfPracticeDemo.Interfaces
{
    public interface IGeometryService
    {

        Geometry GetGeometry(ShapeBase shape,GeometryType geometryType, Point leftButtonDownPoint, Point leftButtonUpPoint,bool isUpdateGeometry);      

        Geometry GetRelativeGeometry(Geometry orignalGeometry,ShapeBase shape, GeometryType geometryType, Point leftButtonDownPoint, Point leftButtonUpPoint,bool isUpdateGeometry);

        bool IsGeometryValidation(ShapeBase shape,Geometry shapeGeometry, Rect canvasRect);

        bool IsGeometryPointInSelectedRect(ShapeBase shape, Geometry shapeGeometry, Rect selectedRect);
    }
}
