using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using WpfPracticeDemo.Enums;
using WpfPracticeDemo.Models;

namespace WpfPracticeDemo.Shapes
{
    internal class RectangleShape : ShapeBase
    {       

        public override string Name => "Rectangle";

        public override ShapeType Type => ShapeType.Rectangle;

        public override Geometry GetRelativeGeometry(Geometry orignalGeometry, ShapeBase shape, GeometryType geometryType, Point leftButtonDownPoint, Point leftButtonUpPoint)
        {
            var deltaPoint = GetDeltaPoint(leftButtonDownPoint, leftButtonUpPoint);
            var orignalGeometryRect = (orignalGeometry as RectangleGeometry).Rect;

            RectangleGeometry rectangleGeometry = new RectangleGeometry()
            {
                Rect = new Rect()
                {
                    Location = new Point(orignalGeometryRect.X + deltaPoint.X, orignalGeometryRect.Y + deltaPoint.Y),
                    Height = orignalGeometryRect.Height,
                    Width = orignalGeometryRect.Width,
                }
            };

            return rectangleGeometry;
        }

        protected override Geometry CreateMouseOverGeometry(Point leftButtonDownPoint, Point leftButtonUpPoint)
        {
            throw new NotImplementedException();
        }

        protected override Geometry CreateShapeGeometry(Point leftButtonDownPoint, Point leftButtonUpPoint)
        {
            RectangleGeometry rectangleGeometry = new RectangleGeometry()
            {
                Rect = new Rect()
                {
                    Location = GetRectangleLocation(leftButtonDownPoint, leftButtonUpPoint),
                    Size = GetRectangleSize(leftButtonDownPoint, leftButtonUpPoint)
                }
            };

            return rectangleGeometry;
        }

        protected override Geometry CreateShapeSelectedGeometry(Point leftButtonDownPoint, Point leftButtonUpPoint)
        {
            GeometryGroup geometryGroup = new GeometryGroup();
            var orignalRect = (_currentShapeGeometry as RectangleGeometry).Rect;            

            EllipseGeometry ellipseGeometryLeftTop = new EllipseGeometry()
            {
                Center = orignalRect.Location,
                RadiusX = ShapeSelectedAdornerRadius,
                RadiusY = ShapeSelectedAdornerRadius
            };

            geometryGroup.Children.Add(ellipseGeometryLeftTop);

            EllipseGeometry ellipseGeometryLeftBottom = new EllipseGeometry()
            {
                Center =new Point(orignalRect.Location.X,orignalRect.Location.Y+orignalRect.Height),
                RadiusX = ShapeSelectedAdornerRadius,
                RadiusY = ShapeSelectedAdornerRadius
            };
            geometryGroup.Children.Add(ellipseGeometryLeftBottom);

            EllipseGeometry ellipseGeometryRightTop = new EllipseGeometry()
            {
                Center = new Point(orignalRect.Location.X+orignalRect.Width, orignalRect.Location.Y),
                RadiusX = ShapeSelectedAdornerRadius,
                RadiusY = ShapeSelectedAdornerRadius
            };
            geometryGroup.Children.Add(ellipseGeometryRightTop);

            EllipseGeometry ellipseGeometryRightBottom = new EllipseGeometry()
            {
                Center = new Point(orignalRect.Location.X + orignalRect.Width, orignalRect.Location.Y + orignalRect.Height),
                RadiusX = ShapeSelectedAdornerRadius,
                RadiusY = ShapeSelectedAdornerRadius
            };
            geometryGroup.Children.Add(ellipseGeometryRightBottom);

            return geometryGroup;

        }

        private Point GetRectangleLocation(Point leftButtonDownPoint, Point leftButtonUpPoint)
        {

            if (leftButtonUpPoint.X > leftButtonDownPoint.X)
            {
                if (leftButtonUpPoint.Y > leftButtonDownPoint.Y)
                {
                    return leftButtonDownPoint;
                }
                else
                {
                    return new Point(leftButtonDownPoint.X, leftButtonUpPoint.Y);
                }
            }
            else
            {
                if (leftButtonUpPoint.Y > leftButtonDownPoint.Y)
                {
                    return new Point(leftButtonUpPoint.X, leftButtonDownPoint.Y);
                }
                else
                {
                    return leftButtonUpPoint;
                }
            }
        }

        private Size GetRectangleSize(Point leftButtonDownButtonPoint, Point leftButtonUpPoint)
        {
            Size rectangleSize = new Size()
            {
                Height = Math.Abs(leftButtonDownButtonPoint.Y - leftButtonUpPoint.Y),
                Width = Math.Abs(leftButtonDownButtonPoint.X - leftButtonUpPoint.X)
            };

            return rectangleSize;
        }

        public override bool IsGeometryValidation(Geometry shapeGeometry, Rect canvasRect)
        {
            var rectangleGeometryRect = (shapeGeometry as RectangleGeometry).Rect;

            if (rectangleGeometryRect.Location.X < canvasRect.Location.X
                || rectangleGeometryRect.Location.Y < canvasRect.Y
                || rectangleGeometryRect.Location.X + rectangleGeometryRect.Width > canvasRect.Location.X + canvasRect.Width
                || rectangleGeometryRect.Location.Y + rectangleGeometryRect.Height > canvasRect.Location.Y + canvasRect.Height)
            {
                return false;
            }

            return base.IsGeometryValidation(shapeGeometry, canvasRect);
        }

        public override bool IsGeometryPointInSelectedRect(Geometry shapeGeometry, Rect selectedRect)
        {

            var shapeRect = (_currentShapeGeometry as RectangleGeometry).Rect;

            if (shapeRect.IntersectsWith(selectedRect))
            {
                return true;
            }
            else
            {
                return false;
            }            
        }
    }
}
