using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using WpfPracticeDemo.Enums;
using WpfPracticeDemo.Models;

namespace WpfPracticeDemo.Shapes
{
    internal class LineShape : ShapeBase
    {

        private LineGeometry _currentShapeGeometry;

        public override string Name => "Line";

        public override Geometry CurrentShapeGeometry => _currentShapeGeometry;

        public override ShapeType Type => ShapeType.Line;

        public override Geometry UpdateGeometry(Geometry orignalGeometry, ShapeBase shape, GeometryType geometryType, Point leftButtonDownPoint, Point leftButtonUpPoint)
        {
            var deltaPoint = GetDeltaPoint(leftButtonDownPoint, leftButtonUpPoint);
            var geometry = orignalGeometry as LineGeometry;
            geometry.StartPoint =new Point(geometry.StartPoint.X+deltaPoint.X,geometry.StartPoint.Y+deltaPoint.Y);
            geometry.EndPoint = new Point(geometry.EndPoint.X + deltaPoint.X, geometry.EndPoint.Y + deltaPoint.Y);

            return geometry;
        }

        private Point GetDeltaPoint(Point leftButtonDownPoint, Point leftButtonUpPoint)
        {
            return new Point(leftButtonUpPoint.X - leftButtonDownPoint.X, leftButtonUpPoint.Y - leftButtonDownPoint.Y);            
        }

        protected override Geometry CreateMouseOverGeometry(Point leftButtonDownPoint, Point leftButtonUpPoint)
        {
            throw new NotImplementedException();
        }

        protected override Geometry CreateShapeGeometry(Point leftButtonDownPoint, Point leftButtonUpPoint)
        {
            _currentShapeGeometry = new LineGeometry()
            {
                StartPoint = leftButtonDownPoint,
                EndPoint = leftButtonUpPoint
            };

            return _currentShapeGeometry;
        }

        protected override Geometry CreateShapeSelectedGeometry(Point leftButtonDownPoint, Point leftButtonUpPoint)
        {
            throw new NotImplementedException();
        }
    }
}
