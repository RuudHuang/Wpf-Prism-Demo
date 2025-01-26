using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;
using WpfPracticeDemo.Adorners;
using WpfPracticeDemo.Enums;
using WpfPracticeDemo.Events;
using WpfPracticeDemo.Interfaces;
using WpfPracticeDemo.Models;
using WpfPracticeDemo.Shapes;
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

        private readonly IOperationTypeService _operationTypeService;

        private readonly IDrawingShapeTypeService _drawingShapeTypeService;

        private readonly Color _colorShapeSelected = Colors.Red;

        private readonly Color _colorShapeDrawing = Colors.Green;

        private readonly Color _colorShapeOutCanvasRange = Colors.Red;

        private readonly Color _colorShapeMove = Colors.Yellow;

        private readonly Color _colorForSelectRectangle = Colors.Gray;

        private Point _canvasLeftButtonDownPoint;

        private Point _canvasLeftButtonUpPoint;

        private Rect _canvasRect;

        private AdornerLayer _canvasAdornerLayer;

        private readonly ObservableCollection<DemoGraphicInfomation> _selectedGraphics = new ObservableCollection<DemoGraphicInfomation>();

        private readonly ObservableCollection<DemoGraphicInfomation> _graphics = new ObservableCollection<DemoGraphicInfomation>();

        private readonly ObservableCollection<Adorner> _shapeDrawingAdorners = new ObservableCollection<Adorner>();

        private readonly ObservableCollection<Adorner> _shapeSelectedAdorners = new ObservableCollection<Adorner>();

        private readonly ObservableCollection<DemoGraphicInfomation> _tempSelectedGraphics = new ObservableCollection<DemoGraphicInfomation>();

        private ShapeType CurrentShapeType
        {
            get { return _drawingShapeTypeService.CurrentSelectedShapeType; }
        }

        private OperationType CurrentOperationType
        {
            get { return _operationTypeService.CurrentOperationType; }
        }


        public UcContentView(IEventAggregator eventAggregator,
                             IGeometryService geometryService,
                             IOperationTypeService operationTypeService,
                             IDrawingShapeTypeService drawingShapeTypeService)
        {
            InitializeComponent();
            _eventAggregator = eventAggregator;
            _geometryService = geometryService;           
            _operationTypeService = operationTypeService;
            _drawingShapeTypeService = drawingShapeTypeService;

            ManageSubscribe(true);
        }

        private void ManageSubscribe(bool subscribe)
        {
            this.Loaded += UcContentView_Loaded;

            ManageCanvasEventSubscribe(subscribe);            

            ManageOperationTypeChangedSubscribe(subscribe);
        }

        private void UcContentView_Loaded(object sender, RoutedEventArgs e)
        {
            _canvasRect = new Rect() { Location = new Point(0, 0), Size = new Size(this.Canvas.ActualWidth, this.Canvas.ActualHeight) };
            _canvasAdornerLayer = AdornerLayer.GetAdornerLayer(this.Canvas);
        }

        private void ManageCanvasEventSubscribe(bool subscribe)
        {
            if (subscribe)
            {
                this.Canvas.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;
                this.Canvas.MouseLeftButtonUp += Canvas_MouseLeftButtonUp;
                this.Canvas.MouseLeave += Canvas_MouseLeave;
                this.Canvas.MouseMove += Canvas_MouseMove;
                this.Canvas.MouseWheel += Canvas_MouseWheel;
            }
            else
            {
                this.Canvas.MouseLeftButtonDown -= Canvas_MouseLeftButtonDown;
                this.Canvas.MouseLeftButtonUp -= Canvas_MouseLeftButtonUp;
                this.Canvas.MouseLeave -= Canvas_MouseLeave;
                this.Canvas.MouseMove -= Canvas_MouseMove;
                this.Canvas.MouseWheel -= Canvas_MouseWheel;
            }
        }

        private void Canvas_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (e.Delta > 0)
            {
                UpdateScaleThresholdForAllGraphics(1.1);
            }
            else
            {
                UpdateScaleThresholdForAllGraphics(0.9);
            }
        }

        private void UpdateScaleThresholdForAllGraphics(double scaleThreshold)
        {
            foreach (var item in _graphics)
            {
                UpdateScaleThreshold(item, scaleThreshold);
            }
        }

        private void ManagePathEventSubscribe(Path path, bool subscribe)
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

        private void AddScaleTransform(DemoGraphicInfomation demoGraphicInfomation)
        {
            demoGraphicInfomation.GraphicPath.RenderTransform = new ScaleTransform();
        }

        private void UpdateScaleThreshold(DemoGraphicInfomation demoGraphicInfomation,double scaleThreshold)
        {
            var shapeScaleTransform = (demoGraphicInfomation.GraphicPath.RenderTransform as ScaleTransform);
            shapeScaleTransform.ScaleX=shapeScaleTransform.ScaleX*scaleThreshold;
            shapeScaleTransform.ScaleY = shapeScaleTransform.ScaleY * scaleThreshold;
        }

        private void ManageOperationTypeChangedSubscribe(bool subscribe)
        {
            if (subscribe)
            {
                _operationTypeService.OperationTypeChanged += OperationTypeChangedHandler;                
            }
            else
            {
                _operationTypeService.OperationTypeChanged -= OperationTypeChangedHandler;
            }
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton.Equals(MouseButtonState.Pressed))
            {
                var currentPosition = e.GetPosition(this.Canvas);

                ///drawing adorner shape
                if (CurrentOperationType.Equals(OperationType.DrawGraphic))
                {                    
                    var shape = CreateShape(CurrentShapeType);
                    var geometry = GetGeometry(shape,currentPosition,GeometryType.Shape);
                    var adornerColor= GetAdornerColor(shape, _colorShapeDrawing, geometry);
                    var adorner=CreateAdorner(geometry, adornerColor,DashStyles.Solid);

                    ClearDrawingAdorners();
                    AddAdorner(adorner);
                    _shapeDrawingAdorners.Add(adorner);                    
                }
                else
                {                    
                    //select mode
                    
                    //have selected element, move selected element
                    if (_selectedGraphics.Any())
                    {                        
                        _operationTypeService.SetOperationType(OperationType.Move);                        

                        ClearDrawingAdorners();

                        Color adornerColor = _colorShapeMove;

                        ObservableCollection<Geometry> tempGeometryCollection = new ObservableCollection<Geometry>();

                        foreach (var item in _selectedGraphics)
                        {
                            var graphic = _geometryService.GetRelativeGeometry(item.GraphicPath.Data, item.Shape, GeometryType.Shape, _canvasLeftButtonDownPoint, currentPosition);

                            if (!_geometryService.IsGeometryValidation(item.Shape, graphic, _canvasRect))
                            {
                                adornerColor = _colorShapeOutCanvasRange;
                            }

                            tempGeometryCollection.Add(graphic);
                        }

                        foreach (var item in tempGeometryCollection)
                        {
                            var adorner=CreateAdorner(item, adornerColor,DashStyles.Solid);
                            AddAdorner(adorner);
                            _shapeDrawingAdorners.Add(adorner);                           
                        }
                    }
                    else
                    {
                        //no selected element, drawing select rectangle to select element

                        _operationTypeService.SetOperationType(OperationType.Select);

                        //select rectangle
                        Rect selectedRect = new Rect()
                        {
                            Location = GetRectangleLocation(_canvasLeftButtonDownPoint, currentPosition),
                            Size = GetRectangleSize(_canvasLeftButtonDownPoint, currentPosition)
                        };

                        SelectShapesInSpecificSelectRectangle(selectedRect);

                        var shape = CreateShape(CurrentShapeType);
                        var geometry = GetGeometry(shape, currentPosition, GeometryType.Shape);

                        var adornerSelectRectangle = CreateAdorner(geometry, _colorForSelectRectangle, DashStyles.Dash);

                        ClearDrawingAdorners();
                        AddAdorner(adornerSelectRectangle);
                        _shapeDrawingAdorners.Add(adornerSelectRectangle);                        
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
                ClearDrawingAdorners();
            }
            //ResetCanvasLeftButtonPoint();
        }

        private void SelectShapesInSpecificSelectRectangle(Rect selectedRect)
        {                        
            foreach (var item in _graphics)
            {
                var isGeometryPointInSelectedRect= _geometryService.IsGeometryPointInSelectedRect(item.Shape, item.GraphicPath.Data, selectedRect);
                if (isGeometryPointInSelectedRect)
                {
                    if (_tempSelectedGraphics.Any(x => x.Equals(item)))
                    {
                        continue;
                    }

                    var shapeSelectedAdornerGeometry = GetGeometry(item.Shape, _canvasLeftButtonUpPoint, GeometryType.Selected);

                    var adorner = CreateAdorner(shapeSelectedAdornerGeometry, _colorShapeSelected, DashStyles.Solid);

                    AddAdorner(adorner);

                    _shapeSelectedAdorners.Add(adorner);

                    _tempSelectedGraphics.Add(item);
                }
            }
        }

        private Point GetRectangleLocation(Point leftButtonDownPoint, Point leftButtonUpPoint)
        {

            if (leftButtonUpPoint.X > leftButtonDownPoint.X)
            {
                if (leftButtonUpPoint.Y > leftButtonDownPoint.Y)
                {
                    return leftButtonDownPoint;
                }
                else
                {
                    return new Point(leftButtonDownPoint.X, leftButtonUpPoint.Y);
                }
            }
            else
            {
                if (leftButtonUpPoint.Y > leftButtonDownPoint.Y)
                {
                    return new Point(leftButtonUpPoint.X, leftButtonDownPoint.Y);
                }
                else
                {
                    return leftButtonUpPoint;
                }
            }
        }

        private Size GetRectangleSize(Point leftButtonDownButtonPoint, Point leftButtonUpPoint)
        {
            Size rectangleSize = new Size()
            {
                Height = Math.Abs(leftButtonDownButtonPoint.Y - leftButtonUpPoint.Y),
                Width = Math.Abs(leftButtonDownButtonPoint.X - leftButtonUpPoint.X)
            };

            return rectangleSize;
        }

        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _canvasLeftButtonUpPoint = e.GetPosition(this.Canvas);
            ClearDrawingAdorners();

            if (CurrentOperationType.Equals(OperationType.DrawGraphic))
            {
                DrawingGeometryToCanvas();
            }
            else
            {                
                ObservableCollection<Geometry> tempGeometryCollection = new ObservableCollection<Geometry>();

                foreach (var item in _selectedGraphics)
                {
                    var geometry = _geometryService.GetRelativeGeometry(item.GraphicPath.Data, item.Shape, GeometryType.Shape, _canvasLeftButtonDownPoint, _canvasLeftButtonUpPoint);

                    if (!_geometryService.IsGeometryValidation(item.Shape, geometry, _canvasRect))
                    {
                        tempGeometryCollection.Clear();
                        break;
                    }

                    tempGeometryCollection.Add(geometry);
                }

                if (tempGeometryCollection.Any())
                {
                    for (int i = 0; i < _selectedGraphics.Count; i++)
                    {
                        var item = _selectedGraphics[i];

                        item.GraphicPath.Data= tempGeometryCollection[i];

                        _geometryService.UpdateGeometry(item.Shape, item.GraphicPath.Data);
                    }
                }

                if (CurrentOperationType.Equals(OperationType.Move))
                {
                    if (_selectedGraphics.Any())
                    {
                        _selectedGraphics.Clear();
                        _tempSelectedGraphics.Clear();
                        ClearSelectedAdorners();
                    }
                    
                    _operationTypeService.SetOperationType(OperationType.Select);
                }

                if (CurrentOperationType.Equals(OperationType.Select))
                {
                    _selectedGraphics.AddRange(_tempSelectedGraphics);
                    
                    _operationTypeService.SetOperationType(OperationType.Move);
                }                
            }

            ResetCanvasLeftButtonPoint();
        }

        private void ClearDrawingAdorners()
        {
            if (_canvasAdornerLayer != null)
            {
                foreach (var item in _shapeDrawingAdorners)
                {
                    _canvasAdornerLayer.Remove(item);
                }               

                _shapeDrawingAdorners.Clear();
            }
        }

        private void ClearSelectedAdorners()
        {
            if (_canvasAdornerLayer != null)
            {
                foreach (var item in _shapeSelectedAdorners)
                {
                    _canvasAdornerLayer.Remove(item);
                }

                _shapeSelectedAdorners.Clear();
            }
        }

        private void AddAdorner(Adorner adorner)
        {            
            if (_canvasAdornerLayer != null)
            {
                _canvasAdornerLayer.Add(adorner);
            }
        }

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _canvasLeftButtonDownPoint = e.GetPosition(this.Canvas);
        }

        private void DrawingGeometryToCanvas()
        {

            var shape = CreateShape(CurrentShapeType);
            var geometry = GetGeometry(shape, _canvasLeftButtonUpPoint, GeometryType.Shape);

            if (_geometryService.IsGeometryValidation(shape, geometry, _canvasRect))
            {
                _geometryService.UpdateGeometry(shape, geometry);

                Path path = new Path()
                {
                    Data = geometry,
                    Stroke = new SolidColorBrush(Colors.Green),
                    StrokeThickness = 5
                };

                var demoGraphicInformation = new DemoGraphicInfomation() { Shape = shape, GraphicPath = path };

                _graphics.Add(demoGraphicInformation);

                AddScaleTransform(demoGraphicInformation);
                this.Canvas.Children.Add(path);
            }
        }

        private void Path_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
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
            
            if (_selectedGraphics.Any(x => x.GraphicPath.Equals(element)))
            {
                return;
            }

            var graphicInfo = _graphics.First(x => x.GraphicPath.Equals(element));

            var shapeSelectedAdornerGeometry = GetGeometry(graphicInfo.Shape, _canvasLeftButtonUpPoint, GeometryType.Selected);

            ShapeDrawingAdorner shapeSelectedAdorner = new ShapeDrawingAdorner(element, shapeSelectedAdornerGeometry, Colors.Red, DashStyles.Solid);

            AdornerLayer.GetAdornerLayer(element)?.Add(shapeSelectedAdorner);

            _shapeSelectedAdorners.Add(shapeSelectedAdorner);

            _selectedGraphics.Add(graphicInfo);

        }

        private void OperationTypeChangedHandler(OperationType operationType)
        {
            if (CurrentOperationType.Equals(OperationType.DrawGraphic))
            {                
                UpdatePathEventSubscribe(false);
                ResetSelectedGraphicColor();
                ClearSelectedGraphicCollection();
            }
            else
            {                            
                UpdatePathEventSubscribe(true);
            }
        }

        private void ResetSelectedGraphicColor()
        {
            foreach (var item in _selectedGraphics)
            {
                item.GraphicPath.Stroke = new SolidColorBrush(Colors.Green);
            }
        }

        private void ClearSelectedGraphicCollection()
        {
            _selectedGraphics.Clear();
        }

        private void UpdatePathEventSubscribe(bool subscribe)
        {
            foreach (var item in _graphics)
            {
                ManagePathEventSubscribe(item.GraphicPath, subscribe);
            }
        }

        private static ShapeBase CreateShape(ShapeType shapeType)
        {
            switch (shapeType)
            {
                case ShapeType.Line:
                    return new LineShape();
                case ShapeType.Rectangle:
                    return new RectangleShape();
                case ShapeType.Circle:
                    return new CircleShape();
                default:
                    return new LineShape();
            }
        }

        private Geometry GetGeometry(ShapeBase shape,Point endPoint,GeometryType geometryType)
        {
            return _geometryService.GetGeometry(shape, geometryType, _canvasLeftButtonDownPoint, endPoint);            
        }

        private Color GetAdornerColor(ShapeBase shape,Color adornerDefaultColor,Geometry geometry)
        {
            Color adornerColor = adornerDefaultColor;

            if (!_geometryService.IsGeometryValidation(shape, geometry, _canvasRect))
            {
                adornerColor = _colorShapeOutCanvasRange;
            }

            return adornerColor;
        }

        private Adorner CreateAdorner(Geometry geometry,Color adornerColor,DashStyle dashStyle)
        {
            ShapeDrawingAdorner shapeDrawingAdorner = new ShapeDrawingAdorner(this.Canvas, geometry, adornerColor, dashStyle);

            return shapeDrawingAdorner;
        }
    }
}
