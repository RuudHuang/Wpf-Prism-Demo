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

        public override bool IsGeometryPointInSelectedRect(Geometry shapeGeometry, Rect selectedRect)
        {
            var circleCenter = (_currentShapeGeometry as EllipseGeometry).Center;
            var circleRadius = (_currentShapeGeometry as EllipseGeometry).RadiusX;

            var selectRectPointLeftTop = selectedRect.Location;
            var selectRectPointLeftBottom = new Point(selectedRect.Location.X, selectedRect.Location.Y + selectedRect.Height);
            var selectRectPointRightTop = new Point(selectedRect.Location.X + selectedRect.Width, selectedRect.Location.Y);
            var selectRectPointRightBottom = new Point(selectedRect.Location.X + selectedRect.Width, selectedRect.Location.Y + selectedRect.Height);

            var distanceToRectTopLeft = CalculateDistanceBetweenTwoPoint(circleCenter, selectRectPointLeftTop);
            var distanceToRectTopRight = CalculateDistanceBetweenTwoPoint(circleCenter, selectRectPointRightTop);
            var distanceToRectBottomLeft = CalculateDistanceBetweenTwoPoint(circleCenter, selectRectPointLeftBottom);
            var distanceToRectBottomRight = CalculateDistanceBetweenTwoPoint(circleCenter, selectRectPointRightBottom);

            if (distanceToRectTopLeft < circleRadius
                || distanceToRectTopRight < circleRadius
                || distanceToRectBottomLeft < circleRadius
                || distanceToRectBottomRight < circleRadius)
            {
                return true;
            }
            else
            {

                var centerPointInTopLine = new Point(selectedRect.Location.X + selectedRect.Width / 2, selectedRect.Location.Y);
                var centerPointInLeftLine = new Point(selectedRect.Location.X, selectedRect.Location.Y + selectedRect.Height / 2);
                var centerPointInRightLine = new Point(selectedRect.Location.X + selectedRect.Width, selectedRect.Location.Y + selectedRect.Height / 2);
                var centerPointInBottomLine = new Point(selectedRect.Location.X + selectedRect.Width / 2, selectedRect.Location.Y + selectedRect.Height);

                var distanToRectTopLine = CalculateDistanceBetweenTwoPoint(circleCenter, centerPointInTopLine);
                var distanToRectLeftLine = CalculateDistanceBetweenTwoPoint(circleCenter, centerPointInLeftLine);
                var distanToRectRightLine = CalculateDistanceBetweenTwoPoint(circleCenter, centerPointInRightLine);
                var distanToRectBottomLine = CalculateDistanceBetweenTwoPoint(circleCenter, centerPointInBottomLine);

                if (distanToRectTopLine < circleRadius
                    || distanToRectLeftLine < circleRadius
                    || distanToRectRightLine < circleRadius
                    || distanToRectBottomLine < circleRadius)
                {
                    return true;
                }

                return false;
            }
        }

        private static double CalculateDistanceBetweenTwoPoint(Point point1, Point point2)
        {
            return Math.Sqrt(Math.Pow(point1.X - point2.X, 2) + Math.Pow(point1.Y - point2.Y, 2));
        }
    }
}

