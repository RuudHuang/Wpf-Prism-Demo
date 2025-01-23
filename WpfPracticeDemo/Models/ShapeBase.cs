using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using WpfPracticeDemo.Enums;

namespace WpfPracticeDemo.Models
{
    public abstract class ShapeBase
    {
        public abstract string Name { get; }

        public abstract ShapeType Type { get; }

        public abstract Geometry GetGeometry(Point leftButtonDownButton, Point leftButtonUpPoint);
    }
}
