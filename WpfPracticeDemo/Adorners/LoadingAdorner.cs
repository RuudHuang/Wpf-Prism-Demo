using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Forms.Design.Behavior;
using System.Windows.Media;
using Adorner = System.Windows.Documents.Adorner;

namespace WpfPracticeDemo.Adorners
{
    internal class LoadingAdorner : Adorner
    {
        private TextBlock _textLoading;
        public LoadingAdorner(UIElement adornedElement) : base(adornedElement)
        {
            InitializeLoadingTextBlock();            
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            //var adornedElementSize = AdornedElement.;

            var displayedTextFormat = new FormattedText("Loading....", 
                CultureInfo.CurrentCulture, 
                FlowDirection.LeftToRight, 
                new Typeface(new FontFamily("Consolas"), FontStyles.Normal, FontWeights.Normal, new FontStretch()), 
                12, 
                new SolidColorBrush(Colors.Black));

            drawingContext.DrawText(displayedTextFormat, new Point(0, 0));
        }

        

        private void InitializeLoadingTextBlock()
        {
            _textLoading = new TextBlock();
            _textLoading.Text = "Loading....";
        }
    }
}
