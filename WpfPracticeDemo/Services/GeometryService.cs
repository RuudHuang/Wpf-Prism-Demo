using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
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

        public bool IsGeometryValidation(ShapeBase shape, Rect canvasRect)
        {
            return shape.IsGeometryValidation(canvasRect);
        }

        public Geometry UpdateGeometry(Geometry orignalGeometry, ShapeBase shape, GeometryType geometryType, Point leftButtonDownPoint, Point leftButtonUpPoint)
        {
            return shape.UpdateGeometry(orignalGeometry, shape, geometryType, leftButtonDownPoint, leftButtonUpPoint);
        }
    }
}
