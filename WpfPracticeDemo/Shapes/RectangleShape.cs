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
        public override string Name => "Rectangle";

        public override ShapeType Type => ShapeType.Rectangle;

        public override Geometry GetGeometry(Point leftButtonDownButton, Point leftButtonUpPoint)
        {
            return new RectangleGeometry()
            {
                Rect = new Rect()
                {
                    Location = GetRectangleLocation(leftButtonDownButton,leftButtonUpPoint),
                    Size = GetRectangleSize(leftButtonDownButton,leftButtonUpPoint)
                }
            };
        }

        private Point GetRectangleLocation(Point leftButtonDownButton, Point leftButtonUpPoint)
        {

            if (leftButtonUpPoint.X > leftButtonDownButton.X)
            {
                if (leftButtonUpPoint.Y > leftButtonDownButton.Y)
                {
                    return leftButtonDownButton;
                }
                else
                {
                    return new Point(leftButtonDownButton.X, leftButtonUpPoint.Y);
                }
            }
            else
            {
                if (leftButtonUpPoint.Y > leftButtonDownButton.Y)
                {
                    return new Point(leftButtonUpPoint.X, leftButtonDownButton.Y);
                }
                else
                {
                    return leftButtonUpPoint;
                }
            }
        }

        private Size GetRectangleSize(Point leftButtonDownButton, Point leftButtonUpPoint)
        {
            Size rectangleSize = new Size()
            {
                Height = Math.Abs(leftButtonDownButton.Y - leftButtonUpPoint.Y),
                Width = Math.Abs(leftButtonDownButton.X - leftButtonUpPoint.X)
            };

            return rectangleSize;
        }
    }
}
