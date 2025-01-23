using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfPracticeDemo.Enums;
using WpfPracticeDemo.Events;
using WpfPracticeDemo.Interfaces;
using WpfPracticeDemo.Models;

namespace WpfPracticeDemo.Views
{
    /// <summary>
    /// Interaction logic for UcContentView.xaml
    /// </summary>
    public partial class UcContentView : UserControl
    {

        private readonly IEventAggregator _eventAggregator;

        private readonly IGeometryService _geometryService;

        public UcContentView(IEventAggregator eventAggregator,
            IGeometryService geometryService)
        {
            InitializeComponent();
            _eventAggregator = eventAggregator;
            _geometryService = geometryService;
            ManageSubscribe(true);
        }

        private void ManageSubscribe(bool subscribe)
        {
            if (subscribe)
            {
                this.Canvas.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;
                this.Canvas.MouseLeftButtonUp += Canvas_MouseLeftButtonUp;
                this.Canvas.MouseLeave += Canvas_MouseLeave;

                _eventAggregator.GetEvent<SelectedShapeChangedEvent>().Subscribe(SelectedShapeChangedHandler);
            }
            else
            {
                this.Canvas.MouseLeftButtonDown -= Canvas_MouseLeftButtonDown;
                this.Canvas.MouseLeftButtonUp -= Canvas_MouseLeftButtonUp;
                this.Canvas.MouseLeave -= Canvas_MouseLeave;

                _eventAggregator.GetEvent<SelectedShapeChangedEvent>().Unsubscribe(SelectedShapeChangedHandler);
            }
        }

        private void Canvas_MouseLeave(object sender, MouseEventArgs e)
        {
            _geometryService.LeftButtonDownPoint = new Point(0, 0);
            _geometryService.LeftButtonUpPoint = new Point(0, 0);
        }

        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _geometryService.LeftButtonUpPoint = e.GetPosition(this.Canvas);

            DrawingGeometryToCanvas();
        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _geometryService.LeftButtonDownPoint = e.GetPosition(this.Canvas);
        }

        private void SelectedShapeChangedHandler(SelectedShapeChangedEventArgs args)
        {
            _geometryService.SelectedShape = args.SelectedShape;
        }

        private void DrawingGeometryToCanvas()
        {
            var graphic = _geometryService.GetGeometry();

            Path path = new Path()
            {
                Data = graphic,
                Stroke=new SolidColorBrush(Colors.Green),
                StrokeThickness=5
            };

            this.Canvas.Children.Add(path);            
        }
        
    }
}
