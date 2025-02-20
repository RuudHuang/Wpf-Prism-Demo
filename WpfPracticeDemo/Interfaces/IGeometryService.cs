using System.Windows;
using System.Windows.Media;
using WpfPracticeDemo.Enums;
using WpfPracticeDemo.Models;

namespace WpfPracticeDemo.Interfaces
{
    public interface IGeometryService
    {

        Geometry GetGeometry(ShapeBase shape, GeometryType geometryType, Point leftButtonDownPoint, Point leftButtonUpPoint);

        Geometry GetRelativeGeometry(Geometry orignalGeometry, ShapeBase shape, GeometryType geometryType, Point leftButtonDownPoint, Point leftButtonUpPoint);

        bool IsGeometryValidation(ShapeBase shape, Geometry shapeGeometry, Geometry canvasRect);

        bool IsGeometryPointInSelectedRect(ShapeBase shape, Geometry shapeGeometry, Geometry selectedRect);

        void UpdateGeometry(ShapeBase shape, Geometry geometry);
    }
}
