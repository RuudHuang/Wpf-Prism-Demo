using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfPracticeDemo.Adorners
{
    internal class ShapeSelectedAdorner : Adorner
    {
        public ShapeSelectedAdorner(UIElement adornedElement) : base(adornedElement)
        {
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            drawingContext.DrawGeometry(new SolidColorBrush(Colors.Transparent), new Pen(new SolidColorBrush(Colors.Blue), 10), (AdornedElement as Path).Data);

            base.OnRender(drawingContext);
        }
    }
}
