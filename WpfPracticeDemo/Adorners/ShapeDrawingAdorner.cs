using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace WpfPracticeDemo.Adorners
{
    internal class ShapeDrawingAdorner : Adorner
    {

        private Geometry _shapeAdornerGeometry;

        private Color _adornerColor;

        private DashStyle _adornerDashStyle;

        private double _shapeThickness;

        private bool _useHitPoint = false;


        public ShapeDrawingAdorner(UIElement adornedElement,
                                   Geometry shapeAdornerGeometry,
                                   Color adornerColor,
                                   DashStyle dashStyle,
                                   double shapeThickness,
                                   bool useHitPoint)
            : base(adornedElement)
        {
            _shapeAdornerGeometry = shapeAdornerGeometry;
            _adornerColor = adornerColor;
            _adornerDashStyle = dashStyle;
            _shapeThickness = shapeThickness;
            _useHitPoint = useHitPoint;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            var shapeBrush = new SolidColorBrush(Colors.Transparent);
            var shapePen = new Pen(new SolidColorBrush(_adornerColor), _shapeThickness);
            shapePen.DashStyle = _adornerDashStyle;

            drawingContext.DrawGeometry(shapeBrush, shapePen, _shapeAdornerGeometry);

            base.OnRender(drawingContext);
        }

        protected override HitTestResult HitTestCore(PointHitTestParameters hitTestParameters)
        {
            //if (_useHitPoint)
            //{
            //    if (_shapeAdornerGeometry.FillContains(hitTestParameters.HitPoint))
            //    {
            //        return new PointHitTestResult(this, hitTestParameters.HitPoint);
            //    }
            //}
            return null;
        }

    }
}
