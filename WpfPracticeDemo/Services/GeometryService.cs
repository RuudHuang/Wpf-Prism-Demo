using System.Windows;
using System.Windows.Media;
using WpfPracticeDemo.Enums;
using WpfPracticeDemo.Interfaces;
using WpfPracticeDemo.Models;

namespace WpfPracticeDemo.Services
{
    internal class GeometryService : IGeometryService
    {
        public Geometry GetGeometry(ShapeBase shape, GeometryType geometryType, Point leftButtonDownPoint, Point leftButtonUpPoint)
        {
            return shape.CreateGeometry(geometryType, leftButtonDownPoint, leftButtonUpPoint);
        }

        public bool IsGeometryValidation(ShapeBase shape, Geometry shapeGeometry, Geometry canvasRect)
        {
            return shape.IsGeometryValidation(shapeGeometry, canvasRect);
        }

        public Geometry GetRelativeGeometry(Geometry orignalGeometry, ShapeBase shape, GeometryType geometryType, Point leftButtonDownPoint, Point leftButtonUpPoint)
        {
            return shape.GetRelativeGeometry(orignalGeometry, shape, geometryType, leftButtonDownPoint, leftButtonUpPoint);
        }

        public bool IsGeometryPointInSelectedRect(ShapeBase shape, Geometry shapeGeometry, Geometry selectedRect)
        {
            return shape.IsGeometryPointInSelectedRect(shapeGeometry, selectedRect);
        }

        public void UpdateGeometry(ShapeBase shape, Geometry geometry)
        {
            shape.UpdateGeometry(geometry);
        }
    }
}
