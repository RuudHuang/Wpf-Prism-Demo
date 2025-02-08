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
            if (!(_currentShapeGeometry is LineGeometry lineGeometry))
            {
                return false;
            }

            return IsLineIntersectRectangle(lineGeometry, selectedRect);
        }

        private static bool IsLineIntersectRectangle(LineGeometry line, Rect rectangle)
        {
            if (rectangle.Contains(line.StartPoint) || rectangle.Contains(line.EndPoint))
            {
                return true;
            }

            var ptStart = line.StartPoint;
            var ptEnd = line.EndPoint;

            var dx = ptEnd.X - ptStart.X;
            var dy = ptEnd.Y - ptStart.Y;

            var checkStep = 1.0f;
            var distance = Math.Sqrt(dx * dx + dy * dy);

            if (distance > 1)
            {
                checkStep = (float)(1 / distance);
            }

            for (float m = 0; m <= 1; m += checkStep)
            {
                var ptX = ptStart.X + m * dx;
                var ptY = ptStart.Y + m * dy;
                if (rectangle.Contains(new Point(ptX, ptY)))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
