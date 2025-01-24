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
    internal class RectangleShape : ShapeBase
    {

        private RectangleGeometry _currentShapeGeometry;

        public override string Name => "Rectangle";

        public override ShapeType Type => ShapeType.Rectangle;

        public override Geometry CurrentShapeGeometry => _currentShapeGeometry;

        public override Geometry UpdateGeometry(Geometry orignalGeometry, ShapeBase shape, GeometryType geometryType, Point leftButtonDownPoint, Point leftButtonUpPoint)
        {
            throw new NotImplementedException();
        }

        protected override Geometry CreateMouseOverGeometry(Point leftButtonDownPoint, Point leftButtonUpPoint)
        {
            throw new NotImplementedException();
        }

        protected override Geometry CreateShapeGeometry(Point leftButtonDownPoint, Point leftButtonUpPoint)
        {
            _currentShapeGeometry = new RectangleGeometry()
            {
                Rect = new Rect()
                {
                    Location = GetRectangleLocation(leftButtonDownPoint, leftButtonUpPoint),
                    Size = GetRectangleSize(leftButtonDownPoint, leftButtonUpPoint)
                }
            };


            return _currentShapeGeometry;
        }

        protected override Geometry CreateShapeSelectedGeometry(Point leftButtonDownPoint, Point leftButtonUpPoint)
        {
            throw new NotImplementedException();
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
    }
}
