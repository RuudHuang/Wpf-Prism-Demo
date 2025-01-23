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
        public override string Name => "Circle";

        public override ShapeType Type => ShapeType.Circle;

        public override Geometry GetGeometry(Point leftButtonDownButton, Point leftButtonUpPoint)
        {
            var circleRadius=GetRadius(leftButtonDownButton, leftButtonUpPoint);
            return new EllipseGeometry()
            {
                Center = leftButtonDownButton,
                RadiusX = circleRadius,
                RadiusY = circleRadius
            };
        }

        private double GetRadius(Point leftButtonDownButton, Point leftButtonUpPoint)
        {
            return Math.Sqrt(Math.Pow(leftButtonDownButton.X - leftButtonUpPoint.X, 2) + Math.Pow(leftButtonDownButton.Y - leftButtonUpPoint.Y, 2));
        }
    }
}
