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
        public override string Name => "Line";

        public override ShapeType Type => ShapeType.Line;

        public override Geometry GetGeometry(Point leftButtonDownButton, Point leftButtonUpPoint)
        {
            return new LineGeometry()
            {
                StartPoint = leftButtonDownButton,
                EndPoint = leftButtonUpPoint
            };
        }
    }
}
