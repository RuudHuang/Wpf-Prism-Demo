using Prism.Events;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using WpfPracticeDemo.Adorners;
using WpfPracticeDemo.Enums;
using WpfPracticeDemo.Helpers;
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

        private Geometry _canvasRect;

        private AdornerLayer _canvasAdornerLayer;

        private bool _useHitPoint = false;

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
            _canvasRect = new RectangleGeometry()
            {
                Rect = new Rect() { Location = new Point(0, 0), Size = new Size(this.Canvas.ActualWidth, this.Canvas.ActualHeight) }
            };

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

        private void UpdateScaleThreshold(DemoGraphicInfomation demoGraphicInfomation, double scaleThreshold)
        {
            var shapeScaleTransform = (demoGraphicInfomation.GraphicPath.RenderTransform as ScaleTransform);
            shapeScaleTransform.ScaleX = shapeScaleTransform.ScaleX * scaleThreshold;
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

                ClearDrawingAdorners();

                CanvasMouseMoveHandler(currentPosition);                
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
            RectangleGeometry rectangleGeometry = new RectangleGeometry()
            {
                Rect = selectedRect
            };            

            foreach (var item in _graphics)
            {                
                var isGeometryPointInSelectedRect = _geometryService.IsGeometryPointInSelectedRect(item.Shape, item.GraphicPath.Data, rectangleGeometry);
                if (isGeometryPointInSelectedRect)
                {
                    if (_tempSelectedGraphics.Any(x => x.Equals(item)))
                    {
                        continue;
                    }

                    var shapeSelectedAdornerGeometry = GetGeometry(item.Shape, _canvasLeftButtonUpPoint, GeometryType.Selected);

                    var adorner = CreateAdorner(shapeSelectedAdornerGeometry, _colorShapeSelected, DashStyles.Solid);

                    AddAdornerToAdornerLayer(adorner);

                    _shapeSelectedAdorners.Add(adorner);

                    _tempSelectedGraphics.Add(item);
                }
            }
        }        

        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            _canvasLeftButtonUpPoint = e.GetPosition(this.Canvas);
            ClearDrawingAdorners();

            CanvasMouseUpHandler(_canvasLeftButtonUpPoint);            
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

        private void SaveShapeDrawingAdorner(Adorner adorner)
        { 
          _shapeDrawingAdorners.Add(adorner);
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

        private void AddAdornerToAdornerLayer(Adorner adorner)
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
            _canvasLeftButtonUpPoint = e.GetPosition(this.Canvas);
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
            _canvasLeftButtonDownPoint = e.GetPosition(this.Canvas);
            var element = sender as Path;

            if (CurrentOperationType.Equals(OperationType.Move))
            {
                if (_selectedGraphics.Any(x => x.GraphicPath.Equals(element)))
                {
                    return;
                }

                var graphicInfo = _graphics.First(x => x.GraphicPath.Equals(element));

                var shapeSelectedAdornerGeometry = GetGeometry(graphicInfo.Shape, _canvasLeftButtonUpPoint, GeometryType.Selected);

                var adorner= CreateAdorner(shapeSelectedAdornerGeometry, _colorShapeSelected, DashStyles.Solid);

                AddAdornerToAdornerLayer(adorner);

                _shapeSelectedAdorners.Add(adorner);

                _selectedGraphics.Add(graphicInfo);
            }
            else
            {
                this.Canvas.Children.Remove(element);
                var item = _graphics.FirstOrDefault(x => x.GraphicPath.Equals(element));
                _graphics.Remove(item);
            }

            //e.Handled = true;
        }

        private void OperationTypeChangedHandler(OperationType operationType)
        {            
            ClearSelectedAdorners();
            ClearSelectedGraphicCollection();

            if (CurrentOperationType.Equals(OperationType.DrawGraphic))
            {                                
                UpdatePathEventSubscribe(false);
                ResetSelectedGraphicColor();                
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

        private void ClearTempSelectedGraphicCollection()
        {
            _tempSelectedGraphics.Clear();
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

        private Geometry GetGeometry(ShapeBase shape, Point endPoint, GeometryType geometryType)
        {
            return _geometryService.GetGeometry(shape, geometryType, _canvasLeftButtonDownPoint, endPoint);
        }

        private Color GetAdornerColor(ShapeBase shape, Color adornerDefaultColor, Geometry geometry)
        {
            Color adornerColor = adornerDefaultColor;

            if (!_geometryService.IsGeometryValidation(shape, geometry, _canvasRect))
            {
                adornerColor = _colorShapeOutCanvasRange;
            }

            return adornerColor;
        }

        private Adorner CreateAdorner(Geometry geometry, Color adornerColor, DashStyle dashStyle)
        {
            ShapeDrawingAdorner shapeDrawingAdorner = new ShapeDrawingAdorner(this.Canvas, geometry, adornerColor, dashStyle, 5, _useHitPoint);

            if (_useHitPoint)
            {
                shapeDrawingAdorner.MouseEnter += ShapeDrawingAdorner_MouseEnter;
            }

            return shapeDrawingAdorner;
        }

        private void ShapeDrawingAdorner_MouseEnter(object sender, MouseEventArgs e)
        {
            
        }

        private Adorner CreateShapeDrawingAdorner(Point endpoint, GeometryType geometryType, Color adornerDefaultColor, DashStyle dashStyle, bool isNeedValidateGeometry)
        {
            var shape = CreateShape(CurrentShapeType);
            var geometry = GetGeometry(shape, endpoint, geometryType);
            Color adornerColor = adornerDefaultColor;
            if (isNeedValidateGeometry)
            {
                adornerColor = GetAdornerColor(shape, adornerDefaultColor, geometry);
            }
            var adorner = CreateAdorner(geometry, adornerColor, dashStyle);

            return adorner;
        }

        private void CanvasMouseUpHandler(Point mousePoint)
        {
            switch (CurrentOperationType)
            {
                case OperationType.DrawGraphic:
                    DrawingGeometryToCanvas();
                    break;
                case OperationType.Move:
                    MoveShapeCanvasMouseUp(mousePoint);
                    break;
                case OperationType.Delete:
                    DeleteShapeCanvasMouseUp(mousePoint);
                    break;
                case OperationType.AdjustSize:
                    AdjustShapeCanvasMouseUp(mousePoint);
                    break;
                default: break;
            }            
        }

        private void AdjustShapeCanvasMouseUp(Point mousePoint)
        {
            if (!_selectedGraphics.Any())
            {
                _selectedGraphics.AddRange(_tempSelectedGraphics);
                ClearTempSelectedGraphicCollection();
            }
            else
            {
                foreach (var item in _selectedGraphics)
                {
                    
                }
            }
        }

        private void DeleteShapeCanvasMouseUp(Point mousePoint)
        {
            foreach (var item in _tempSelectedGraphics)
            {
                this.Canvas.Children.Remove(item.GraphicPath);

                _graphics.Remove(item);
            }

            ClearTempSelectedGraphicCollection();

            ClearSelectedAdorners();
        }

        private void MoveShapeCanvasMouseUp(Point mousePoint)
        {
            if (!_selectedGraphics.Any())
            {
                _selectedGraphics.AddRange(_tempSelectedGraphics);
                ClearTempSelectedGraphicCollection();
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

                        item.GraphicPath.Data = tempGeometryCollection[i];

                        _geometryService.UpdateGeometry(item.Shape, item.GraphicPath.Data);
                    }
                }

                ClearSelectedGraphicCollection();                
                ClearSelectedAdorners();
            }
        }


        private void CanvasMouseMoveHandler(Point mousePoint)
        {
            switch (CurrentOperationType)
            {
                case OperationType.DrawGraphic:
                    DrawShapeAdorner(mousePoint);
                    break;
                case OperationType.Move:
                    MoveShape(mousePoint);
                    break;
                case OperationType.Delete:
                    DeleteShape(mousePoint);
                    break;
                case OperationType.AdjustSize:
                    AdjustShape(mousePoint);
                    break;
                default:break;
            }
        }

        private void DrawShapeAdorner(Point mousePoint)
        {
            var adorner = CreateShapeDrawingAdorner(mousePoint, GeometryType.Shape, _colorShapeDrawing, DashStyles.Solid, true);

            AddAdornerToAdornerLayer(adorner);

            SaveShapeDrawingAdorner(adorner);
        }

        private void MoveShape(Point mousePoint)
        {
            if (_selectedGraphics.Any())
            {
                MoveSelectedShapes(mousePoint);
            }
            else
            {
                SelectShapes(mousePoint);
            }
        }

        private void SelectShapes(Point mousePoint)
        {
            DrawSelectRectangleAdorner(mousePoint);

            DrawShapeSelectedAdorner(mousePoint);
        }

        private void MoveSelectedShapes(Point mousePoint)
        {
            Color adornerColor = _colorShapeMove;

            ObservableCollection<Geometry> tempGeometryCollection = new ObservableCollection<Geometry>();

            foreach (var item in _selectedGraphics)
            {
                var graphic = _geometryService.GetRelativeGeometry(item.GraphicPath.Data, item.Shape, GeometryType.Shape, _canvasLeftButtonDownPoint, mousePoint);

                if (!_geometryService.IsGeometryValidation(item.Shape, graphic, _canvasRect))
                {
                    adornerColor = _colorShapeOutCanvasRange;
                }

                tempGeometryCollection.Add(graphic);
            }

            foreach (var item in tempGeometryCollection)
            {
                var adorner = CreateAdorner(item, adornerColor, DashStyles.Solid);
                AddAdornerToAdornerLayer(adorner);
                SaveShapeDrawingAdorner(adorner);
            }
        }

        private void DeleteSelectedShapes(Point mousePoint)
        {
            foreach (var item in _selectedGraphics)
            {
                Canvas.Children.Remove(item.GraphicPath);
            }

            ClearSelectedGraphicCollection();
        }

        private void DrawSelectRectangleAdorner(Point mousePoint)
        {      
            var adorner = CreateShapeDrawingAdorner(mousePoint, GeometryType.Shape, _colorForSelectRectangle, DashStyles.Dash, false);

            AddAdornerToAdornerLayer(adorner);

            SaveShapeDrawingAdorner(adorner);
        }

        private void DrawShapeSelectedAdorner(Point mousePoint)
        {
            Rect selectedRect = new Rect()
            {
                Location = RectangleHelper.GetRectangleLocation(_canvasLeftButtonDownPoint, mousePoint),
                Size = RectangleHelper.GetRectangleSize(_canvasLeftButtonDownPoint, mousePoint)
            };

            SelectShapesInSpecificSelectRectangle(selectedRect);
        }

        private void DeleteShape(Point mousePoint)
        {
            if (_selectedGraphics.Any())
            {
                DeleteSelectedShapes(mousePoint);
            }
            else
            {
                SelectShapes(mousePoint);
            }
        }

        private void AdjustShape(Point mousePoint)
        {
            if (_selectedGraphics.Any())
            {

            }
            else
            {
                SelectShapes(mousePoint);
            }
        }

    }
}
