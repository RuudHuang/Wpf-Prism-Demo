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
        private Geometry _geometry;

        public ShapeSelectedAdorner(UIElement adornedElement,
            Geometry geometry) : base(adornedElement)
        {
            _geometry = geometry;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            drawingContext.DrawGeometry(new SolidColorBrush(Colors.Transparent), new Pen(new SolidColorBrush(Colors.Gray), 2), _geometry);

            base.OnRender(drawingContext);
        }
    }
}
