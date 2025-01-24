using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using System.Xml.Linq;
using WpfPracticeDemo.Adorners;
using WpfPracticeDemo.Enums;
using WpfPracticeDemo.Events;
using WpfPracticeDemo.Interfaces;
using WpfPracticeDemo.Models;
using Path = System.Windows.Shapes.Path;

namespace WpfPracticeDemo.Views
{
    /// <summary>
    /// Interaction logic for UcContentView.xaml
    /// </summary>
    public partial class UcContentView : UserControl
    {

        private readonly IEventAggregator _eventAggregator;

        private readonly IGeometryService _geometryService;

        private Point _canvasLeftButtonDownPoint;

        private Point _canvasLeftButtonUpPoint;

        private Point _pathPreviewMouseLeftButtonDownPoint;

        private Point _pathPreviewMouseLeftButtonUpPoint;

        private OperationType _operationType;

        private ShapeBase _selectedShapeType;

        private ShapeDrawingAdorner _shapeDrawingAdorner;

        private readonly ObservableCollection<DemoGraphicInfomation> _selectedPaths = new ObservableCollection<DemoGraphicInfomation>();

        private readonly ObservableCollection<DemoGraphicInfomation> _graphics = new ObservableCollection<DemoGraphicInfomation>();

        private readonly ObservableCollection<ShapeDrawingAdorner> _shapeDrawingAdorners=new ObservableCollection<ShapeDrawingAdorner>();

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

            ManageCanvasEventSubscribe(subscribe);

            ManageSelectedShapeChangedSubscribe(true);

            ManageOperationTypeChangedSubscribe(subscribe);
            
        }

        private void ManageCanvasEventSubscribe(bool subscribe)
        {
            if (subscribe)
            {
                this.Canvas.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;
                this.Canvas.MouseLeftButtonUp += Canvas_MouseLeftButtonUp;
                this.Canvas.MouseLeave += Canvas_MouseLeave;
                this.Canvas.MouseMove += Canvas_MouseMove;
            }
            else
            {
                this.Canvas.MouseLeftButtonDown -= Canvas_MouseLeftButtonDown;
                this.Canvas.MouseLeftButtonUp -= Canvas_MouseLeftButtonUp;
                this.Canvas.MouseLeave -= Canvas_MouseLeave;
                this.Canvas.MouseMove -= Canvas_MouseMove;
            }
        }

        private void ManagePathEventSubscribe(Path path,bool subscribe)
        {
            if (subscribe)
            {
                path.PreviewMouseLeftButtonDown += Path_PreviewMouseLeftButtonDown;
                path.PreviewMouseLeftButtonUp += Path_PreviewMouseLeftButtonUp;
                path.MouseEnter += Path_MouseEnter;
                path.MouseLeave += Path_MouseLeave;
            }
            else
            {
                path.PreviewMouseLeftButtonDown -= Path_PreviewMouseLeftButtonDown;
                path.PreviewMouseLeftButtonUp -= Path_PreviewMouseLeftButtonUp;
                path.MouseEnter -= Path_MouseEnter;
                path.MouseLeave -= Path_MouseLeave;
            }
        }

        private void ManageSelectedShapeChangedSubscribe(bool subscribe)
        {
            if (subscribe)
            {
                _eventAggregator.GetEvent<SelectedShapeChangedEvent>().Subscribe(SelectedShapeChangedHandler);                
            }
            else
            {
                _eventAggregator.GetEvent<SelectedShapeChangedEvent>().Unsubscribe(SelectedShapeChangedHandler);                
            }
        }

        private void ManageOperationTypeChangedSubscribe(bool subscribe)
        {
            if (subscribe)
            {
                _eventAggregator.GetEvent<OperationTypeChangedEvent>().Subscribe(OperationTypeChangedHandler);
            }
            else
            {
                _eventAggregator.GetEvent<OperationTypeChangedEvent>().Unsubscribe(OperationTypeChangedHandler);
            }
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {

            if (e.LeftButton.Equals(MouseButtonState.Pressed) && _operationType.Equals(OperationType.DrawGraphic))
            { 
               var currentPosition= e.GetPosition(this.Canvas);
                
                Rect canvasRect = new Rect()
                {
                    Location = new Point(0, 0),
                    Size = new Size(Canvas.ActualWidth, Canvas.ActualHeight)
                };

                var graphic = _geometryService.GetGeometry(_selectedShapeType,GeometryType.Shape,_canvasLeftButtonDownPoint, currentPosition);

                Color adornerColor = Colors.Green;

                if (!_geometryService.IsGeometryValidation(_selectedShapeType,canvasRect))
                {
                    adornerColor = Colors.Red;
                }
                var adornerLayer = AdornerLayer.GetAdornerLayer(this.Canvas);
                if (adornerLayer != null)
                {
                    if (_shapeDrawingAdorner != null)
                    {
                        adornerLayer.Remove(_shapeDrawingAdorner);
                    }

                    _shapeDrawingAdorner = new ShapeDrawingAdorner(this.Canvas, graphic, adornerColor);

                    adornerLayer.Add(_shapeDrawingAdorner);
                }
            }

            if (e.LeftButton.Equals(MouseButtonState.Pressed) && _operationType.Equals(OperationType.Select))
            {
                var currentPosition = e.GetPosition(this.Canvas);

                Rect canvasRect = new Rect()
                {
                    Location = new Point(0, 0),
                    Size = new Size(Canvas.ActualWidth, Canvas.ActualHeight)
                };

                _shapeDrawingAdorners.Clear();

                foreach (var item in _selectedPaths)
                {
                    var graphic = _geometryService.UpdateGeometry(item.GraphicPath.Data,item.Shape, GeometryType.Shape, _canvasLeftButtonDownPoint, currentPosition);

                    Color adornerColor = Colors.Green;

                    if (!_geometryService.IsGeometryValidation(item.Shape, canvasRect))
                    {
                        adornerColor = Colors.Red;
                    }

                    var adornerLayer = AdornerLayer.GetAdornerLayer(this.Canvas);
                    if (adornerLayer != null)
                    {                        
                        _shapeDrawingAdorner = new ShapeDrawingAdorner(this.Canvas, graphic, adornerColor);

                        adornerLayer.Add(_shapeDrawingAdorner);
                    }
                }

            }
        }

        private void ResetCanvasLeftButtonPoint()
        {
            _canvasLeftButtonDownPoint = new Point(0, 0);
            _canvasLeftButtonUpPoint = new Point(0, 0);
        }

        private void Canvas_MouseLeave(object sender, MouseEventArgs e)
        {
            if (e.LeftButton.Equals(MouseButtonState.Pressed))
            {
                AdornerLayer.GetAdornerLayer(this.Canvas)?.Remove(_shapeDrawingAdorner);
            }
            ResetCanvasLeftButtonPoint();
        }

        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _canvasLeftButtonUpPoint = e.GetPosition(this.Canvas);

            if (_operationType.Equals(OperationType.DrawGraphic))
            {
                if (_shapeDrawingAdorner != null)
                {
                    AdornerLayer.GetAdornerLayer(this.Canvas)?.Remove(_shapeDrawingAdorner);
                }

                DrawingGeometryToCanvas();
            }            

            ResetCanvasLeftButtonPoint();
        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _canvasLeftButtonDownPoint = e.GetPosition(this.Canvas);
        }

        private void SelectedShapeChangedHandler(SelectedShapeChangedEventArgs args)
        {
            _selectedShapeType = args.SelectedShape;
        }

        private void DrawingGeometryToCanvas()
        {
            Rect canvasRect = new Rect()
            {
                Location = new Point(0, 0),
                Size = new Size(Canvas.ActualWidth,Canvas.ActualHeight)
            };

            var graphic = _geometryService.GetGeometry(_selectedShapeType, GeometryType.Shape,_canvasLeftButtonDownPoint, _canvasLeftButtonUpPoint);

            if (_geometryService.IsGeometryValidation(_selectedShapeType,canvasRect))
            {
                Path path = new Path()
                {
                    Data = graphic,
                    Stroke = new SolidColorBrush(Colors.Green),
                    StrokeThickness = 5
                };

                _graphics.Add(new DemoGraphicInfomation() {  Shape=_selectedShapeType, GraphicPath=path});
                ManagePathEventSubscribe(path, true);

                this.Canvas.Children.Add(path);
            }                       
        }

        private void Path_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void Path_MouseLeave(object sender, MouseEventArgs e)
        {
            
        }

        private void Path_MouseEnter(object sender, MouseEventArgs e)
        {
            
        }

        private void Path_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var element = sender as Path;

            element.Stroke = new SolidColorBrush(Colors.Blue);

            _selectedPaths.Add(_graphics.First(x => x.GraphicPath.Equals(element)));            
        }

        private void OperationTypeChangedHandler(OperationType operationType)
        {
            _operationType=operationType;

            if (_operationType.Equals(OperationType.DrawGraphic))
            {
                ManageSelectedShapeChangedSubscribe(true);
                UpdatePathEventSubscribe(false);
                ResetSelectedGraphicColor();
                ClearSelectedGraphicCollection();
            }
            else
            {                
                ManageSelectedShapeChangedSubscribe(false);
                UpdatePathEventSubscribe(true);
            }
        }

        private void ResetSelectedGraphicColor()
        {
            foreach (var item in _selectedPaths)
            {
                item.GraphicPath.Stroke = new SolidColorBrush(Colors.Green);
            }
        }

        private void ClearSelectedGraphicCollection()
        {
            _selectedPaths.Clear();
        }

        private void UpdatePathEventSubscribe(bool subscribe)
        {
            foreach (var item in _graphics)
            {
                ManagePathEventSubscribe(item.GraphicPath, subscribe);
            }
        }
    }
}
