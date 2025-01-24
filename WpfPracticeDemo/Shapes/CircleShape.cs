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
    internal class CircleShape : ShapeBase
    {
        private EllipseGeometry _currentShapeGeometry;
        public override string Name => "Circle";

        public override ShapeType Type => ShapeType.Circle;

        public override Geometry CurrentShapeGeometry => _currentShapeGeometry;

        protected override Geometry CreateShapeGeometry(Point leftButtonDownPoint, Point leftButtonUpPoint)
        {
            var circleRadius = GetRadius(leftButtonDownPoint, leftButtonUpPoint);

            _currentShapeGeometry = new EllipseGeometry()
            {
                Center = leftButtonDownPoint,
                RadiusX = circleRadius,
                RadiusY = circleRadius
            };

            return _currentShapeGeometry;
        }

        private double GetRadius(Point leftButtonDownPoint, Point leftButtonUpPoint)
        {
            return Math.Sqrt(Math.Pow(leftButtonDownPoint.X - leftButtonUpPoint.X, 2) + Math.Pow(leftButtonDownPoint.Y - leftButtonUpPoint.Y, 2));
        }

        public override bool IsGeometryValidation(Geometry shapeGeometry, Rect canvasRect)
        {
            var circleShapeGeometry = shapeGeometry as EllipseGeometry;
            var circleRadius = circleShapeGeometry.RadiusX;
            var pointLeft = circleShapeGeometry.Center.X - circleRadius;
            var pointTop = circleShapeGeometry.Center.Y - circleRadius;
            var pointRight = circleShapeGeometry.Center.X + circleRadius;
            var pointBottom = circleShapeGeometry.Center.Y + circleRadius;

            if (pointLeft < canvasRect.X
                || pointTop < canvasRect.Y
                || pointRight > canvasRect.X + canvasRect.Width
                || pointBottom > canvasRect.Y + canvasRect.Height)
            {
                return false;
            }

            return base.IsGeometryValidation(shapeGeometry, canvasRect);
        }

        protected override Geometry CreateMouseOverGeometry(Point leftButtonDownPoint, Point leftButtonUpPoint)
        {
            throw new NotImplementedException();
        }

        protected override Geometry CreateShapeSelectedGeometry(Point leftButtonDownPoint, Point leftButtonUpPoint)
        {
            throw new NotImplementedException();
        }

        public override Geometry GetRelativeGeometry(Geometry orignalGeometry, ShapeBase shape, GeometryType geometryType, Point leftButtonDownPoint, Point leftButtonUpPoint)
        {
            var deltaPoint = GetDeltaPoint(leftButtonDownPoint, leftButtonUpPoint);
            var orignalGeometryCenterPoint = (orignalGeometry as EllipseGeometry).Center;
            var circleRadius = (orignalGeometry as EllipseGeometry).RadiusX;

            EllipseGeometry ellipseGeometry = new EllipseGeometry()
            {
                Center = new Point(orignalGeometryCenterPoint.X + deltaPoint.X, orignalGeometryCenterPoint.Y + deltaPoint.Y),
                RadiusX = circleRadius,
                RadiusY = circleRadius
            };

            return ellipseGeometry;
        }

        public override bool IsGeometryPointInSelectedRect(Geometry shapeGeometry, Rect selectedRect)
        {
            return false;
        }
    }
}

