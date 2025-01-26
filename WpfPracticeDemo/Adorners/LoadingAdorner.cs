using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace WpfPracticeDemo.Adorners
{
    internal class LoadingAdorner : Adorner
    {
        private UIElement _elementOnAdorner;

        public LoadingAdorner(UIElement adornedElement,
                              UIElement elementOnAdorner1)
                       : base(adornedElement)
        {
            _elementOnAdorner = elementOnAdorner1;

            AddVisualChild(elementOnAdorner1);
        }

        protected override Visual GetVisualChild(int index)
        {
            if (index == 0)
            {
                return _elementOnAdorner;
            }

            return base.GetVisualChild(index);
        }

        protected override int VisualChildrenCount => 1;

        protected override Size MeasureOverride(Size constraint)
        {
            _elementOnAdorner.Measure(constraint);
            return base.MeasureOverride(constraint);
        }

        protected override HitTestResult HitTestCore(PointHitTestParameters hitTestParameters)
        {
            return null;
        }
        protected override Size ArrangeOverride(Size finalSize)
        {
            return base.ArrangeOverride(finalSize);
        }


    }
}
