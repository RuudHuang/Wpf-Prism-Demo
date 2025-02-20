using System;
using System.Windows;
using System.Windows.Media;
using WpfPracticeDemo.Enums;
using WpfPracticeDemo.Models;

namespace WpfPracticeDemo.Shapes
{
    internal class LineShape : ShapeBase
    {

        public override string Name => "Line";

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
            LineGeometry lineGeometry = new LineGeometry()
            {
                StartPoint = leftButtonDownPoint,
                EndPoint = leftButtonUpPoint
            };

            return lineGeometry;
        }

        protected override Geometry CreateShapeSelectedGeometry(Point leftButtonDownPoint, Point leftButtonUpPoint)
        {
            GeometryGroup geometryGroup = new GeometryGroup();
            var orignalStartPoint = (_currentShapeGeometry as LineGeometry).StartPoint;
            var orignalEndPoint = (_currentShapeGeometry as LineGeometry).EndPoint;

            EllipseGeometry ellipseGeometryStartPoint = new EllipseGeometry()
            {
                Center = orignalStartPoint,
                RadiusX = ShapeSelectedAdornerRadius,
                RadiusY = ShapeSelectedAdornerRadius
            };

            geometryGroup.Children.Add(ellipseGeometryStartPoint);

            EllipseGeometry ellipseGeometryEndPoint = new EllipseGeometry()
            {
                Center = orignalEndPoint,
                RadiusX = ShapeSelectedAdornerRadius,
                RadiusY = ShapeSelectedAdornerRadius
            };
            geometryGroup.Children.Add(ellipseGeometryEndPoint);

            return geometryGroup;
        }                    
    }
}
