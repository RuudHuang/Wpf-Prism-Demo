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

        public override Geometry GetRelativeGeometry(Geometry orignalGeometry, ShapeBase shape, GeometryType geometryType, Point leftButtonDownPoint, Point leftButtonUpPoint)
        {
            var deltaPoint = GetDeltaPoint(leftButtonDownPoint, leftButtonUpPoint);
            var baseGeometry = orignalGeometry as LineGeometry;
            var baseGeometryStatPoint = baseGeometry.StartPoint;
            var baseGeometryEndPoint = baseGeometry.EndPoint;

            LineGeometry lineGeometry = new LineGeometry()
            {
                StartPoint = new Point(baseGeometryStatPoint.X + deltaPoint.X, baseGeometryStatPoint.Y + deltaPoint.Y),
                EndPoint = new Point(baseGeometryEndPoint.X + deltaPoint.X, baseGeometryEndPoint.Y + deltaPoint.Y)
            };


            return lineGeometry;
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

        public override bool IsGeometryValidation(Geometry shapeGeometry, Rect canvasRect)
        {
            var lineGeometry = shapeGeometry as LineGeometry;

            if (lineGeometry.StartPoint.X < canvasRect.Location.X
                || lineGeometry.StartPoint.X > canvasRect.Location.X + canvasRect.Width
                || lineGeometry.StartPoint.Y < canvasRect.Location.Y
                || lineGeometry.StartPoint.Y > canvasRect.Location.Y + canvasRect.Height
                || lineGeometry.EndPoint.X < canvasRect.Location.X
                || lineGeometry.EndPoint.X > canvasRect.Location.X + canvasRect.Width
                || lineGeometry.EndPoint.Y < canvasRect.Location.Y
                || lineGeometry.EndPoint.Y > canvasRect.Location.Y + canvasRect.Height)
            {
                return false;
            }

            return base.IsGeometryValidation(shapeGeometry, canvasRect);
        }

        public override bool IsGeometryPointInSelectedRect(Geometry shapeGeometry, Rect selectedRect)
        {
            return false;

        }
    }
}
