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

        private double _shapeThickness;


        public ShapeDrawingAdorner(UIElement adornedElement,
                                   Geometry shapeAdorner,
                                   Color adornerColor,
                                   DashStyle dashStyle,
                                   double shapeThickness)
            : base(adornedElement)
        {
            _shapeAdorner = shapeAdorner;
            _adornerColor = adornerColor;
            _adornerDashStyle = dashStyle;
            _shapeThickness = shapeThickness;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            var shapeBrush = new SolidColorBrush(Colors.Transparent);
            var shapePen = new Pen(new SolidColorBrush(_adornerColor), _shapeThickness);
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
