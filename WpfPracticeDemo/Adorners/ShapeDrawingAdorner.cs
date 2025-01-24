using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace WpfPracticeDemo.Adorners
{
    internal class ShapeDrawingAdorner : Adorner
    {

        private Geometry _shapeAdorner;

        private Color _adornerColor;
        

        public ShapeDrawingAdorner(UIElement adornedElement,
                                   Geometry shapeAdorner,
                                   Color adornerColor) 
            : base(adornedElement)
        {
            _shapeAdorner = shapeAdorner;
            _adornerColor = adornerColor;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            drawingContext.DrawGeometry(new SolidColorBrush(Colors.Transparent), new Pen(new SolidColorBrush(_adornerColor), 5), _shapeAdorner);           

            base.OnRender(drawingContext);
        }

        protected override HitTestResult HitTestCore(PointHitTestParameters hitTestParameters)
        {
            return null;
        }

    }
}
