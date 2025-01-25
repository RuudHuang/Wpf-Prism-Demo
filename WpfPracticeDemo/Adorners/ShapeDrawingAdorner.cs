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

        private DashStyle _adornerDashStyle;
        

        public ShapeDrawingAdorner(UIElement adornedElement,
                                   Geometry shapeAdorner,
                                   Color adornerColor,
                                   DashStyle dashStyle) 
            : base(adornedElement)
        {
            _shapeAdorner = shapeAdorner;
            _adornerColor = adornerColor;
            _adornerDashStyle = dashStyle;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            var shapeBrush = new SolidColorBrush(Colors.Transparent);
            var shapePen = new Pen(new SolidColorBrush(_adornerColor), 5);
            shapePen.DashStyle = _adornerDashStyle;

            drawingContext.DrawGeometry(shapeBrush, shapePen, _shapeAdorner);
            base.OnRender(drawingContext);
        }

        protected override HitTestResult HitTestCore(PointHitTestParameters hitTestParameters)
        {
            return null;
        }

    }
}
