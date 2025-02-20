using System;
using System.Windows;
using System.Windows.Media;
using WpfPracticeDemo.Enums;
using WpfPracticeDemo.Models;

namespace WpfPracticeDemo.Shapes
{
    internal class CircleShape : ShapeBase
    {
        public override string Name => "Circle";

        public override ShapeType Type => ShapeType.Circle;

        protected override Geometry CreateShapeGeometry(Point leftButtonDownPoint, Point leftButtonUpPoint)
        {
            var circleRadius = GetRadius(leftButtonDownPoint, leftButtonUpPoint);

            EllipseGeometry ellipseGeometry = new EllipseGeometry()
            {
                Center = leftButtonDownPoint,
                RadiusX = circleRadius,
                RadiusY = circleRadius
            };

            return ellipseGeometry;
        }

        private double GetRadius(Point leftButtonDownPoint, Point leftButtonUpPoint)
        {
            return Math.Sqrt(Math.Pow(leftButtonDownPoint.X - leftButtonUpPoint.X, 2) + Math.Pow(leftButtonDownPoint.Y - leftButtonUpPoint.Y, 2));
        }        

        protected override Geometry CreateMouseOverGeometry(Point leftButtonDownPoint, Point leftButtonUpPoint)
        {
            throw new NotImplementedException();
        }

        protected override Geometry CreateShapeSelectedGeometry(Point leftButtonDownPoint, Point leftButtonUpPoint)
        {
            GeometryGroup geometryGroup = new GeometryGroup();
            var circleGeometryCenter = (_currentShapeGeometry as EllipseGeometry).Center;
            var circleGeometryRadius = (_currentShapeGeometry as EllipseGeometry).RadiusX;

            EllipseGeometry ellipseGeometryLeft = new EllipseGeometry()
            {
                Center = new Point(circleGeometryCenter.X - circleGeometryRadius, circleGeometryCenter.Y),
                RadiusX = ShapeSelectedAdornerRadius,
                RadiusY = ShapeSelectedAdornerRadius
            };

            geometryGroup.Children.Add(ellipseGeometryLeft);

            EllipseGeometry ellipseGeometryBottom = new EllipseGeometry()
            {
                Center = new Point(circleGeometryCenter.X, circleGeometryCenter.Y + circleGeometryRadius),
                RadiusX = ShapeSelectedAdornerRadius,
                RadiusY = ShapeSelectedAdornerRadius
            };
            geometryGroup.Children.Add(ellipseGeometryBottom);

            EllipseGeometry ellipseGeometryRight = new EllipseGeometry()
            {
                Center = new Point(circleGeometryCenter.X + circleGeometryRadius, circleGeometryCenter.Y),
                RadiusX = ShapeSelectedAdornerRadius,
                RadiusY = ShapeSelectedAdornerRadius
            };
            geometryGroup.Children.Add(ellipseGeometryRight);

            EllipseGeometry ellipseGeometryTop = new EllipseGeometry()
            {
                Center = new Point(circleGeometryCenter.X, circleGeometryCenter.Y - circleGeometryRadius),
                RadiusX = ShapeSelectedAdornerRadius,
                RadiusY = ShapeSelectedAdornerRadius
            };
            geometryGroup.Children.Add(ellipseGeometryTop);

            return geometryGroup;
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
        
    }
}

