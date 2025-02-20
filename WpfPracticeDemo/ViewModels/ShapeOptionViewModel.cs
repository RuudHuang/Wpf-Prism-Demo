using Prism.Events;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media;
using WpfPracticeDemo.Enums;
using WpfPracticeDemo.Helpers;
using WpfPracticeDemo.Interfaces;
using WpfPracticeDemo.Models;

namespace WpfPracticeDemo.ViewModels
{
    internal class ShapeOptionViewModel : DemoVmBase
    {

        private const string NormalShapeName = "NormalShape";

        private ShapeTypeDisplayModel _selectedShapeType;

        private readonly IDrawingShapeTypeService _drawingShapeTypeService;

        private readonly IOperationTypeService _operationTypeService;

        private readonly ObservableCollection<OperationShapeMenu> _shapeMenus;

        public ObservableCollection<OperationShapeMenu> ShapeMenus
        {
            get { return _shapeMenus; }
        }

        public ShapeTypeDisplayModel SelectedShapeType
        {
            get { return _selectedShapeType; }

            set
            {
                if (SetProperty(ref _selectedShapeType, value))
                {
                    var selectedDrawingShapeType = EnumHelper.GetEnum<ShapeType>(value.ShapeTypeName);
                    _drawingShapeTypeService.SetSelectedShapeType(selectedDrawingShapeType);
                }
            }
        }

        public ShapeOptionViewModel(IEventAggregator eventAggregator,
                                  IDrawingShapeTypeService drawingShapeTypeService,
                                  IOperationTypeService operationTypeService)
                           : base(eventAggregator)
        {
            _drawingShapeTypeService = drawingShapeTypeService;
            _operationTypeService = operationTypeService;
            _shapeMenus = new ObservableCollection<OperationShapeMenu>();
        }

        private void InitializeOperationMenus()
        {
            OperationShapeMenu shapeMenu = new OperationShapeMenu()
            {
                ShapeMenuName = NormalShapeName,
                IsExpanded = true,
                IsEnable = true,
            };

            ShapeTypeDisplayModel line = new ShapeTypeDisplayModel()
            {
                GraphicGeometry = new LineGeometry()
                {
                    StartPoint = new System.Windows.Point(0, 0),
                    EndPoint = new System.Windows.Point(10, 0)
                },
                ShapeTypeName = nameof(ShapeType.Line)
            };

            ShapeTypeDisplayModel rectangle = new ShapeTypeDisplayModel()
            {
                GraphicGeometry = new RectangleGeometry()
                {
                    Rect=new System.Windows.Rect()
                    { 
                     Location=new System.Windows.Point(0,0),
                      Height=10,
                       Width=10
                    }
                },
                ShapeTypeName = nameof(ShapeType.Rectangle)
            };

            ShapeTypeDisplayModel circle = new ShapeTypeDisplayModel()
            {
                GraphicGeometry = new EllipseGeometry()
                {
                     Center=new System.Windows.Point(10,10),
                      RadiusX=10,
                      RadiusY=10,
                },
                ShapeTypeName = nameof(ShapeType.Circle)
            };

            shapeMenu.ShapeTypes.Add(line);
            shapeMenu.ShapeTypes.Add(rectangle);
            shapeMenu.ShapeTypes.Add(circle);

            _shapeMenus.Add(shapeMenu);

            OperationShapeMenu shapeMenu1 = new OperationShapeMenu()
            {
                ShapeMenuName = "Shape1",
                IsEnable=true
            };

            _shapeMenus.Add(shapeMenu1);

            OperationShapeMenu shapeMenu2 = new OperationShapeMenu()
            {
                ShapeMenuName = "Shape2",
                IsEnable=true

            };

            _shapeMenus.Add(shapeMenu2);
        }

        private void InitializeSelectedShape()
        {
            SelectedShapeType = ShapeMenus.FirstOrDefault(x => x.ShapeMenuName.Equals(NormalShapeName)).ShapeTypes[0];
        }

        private void ManageSubscribeForOperationShapeMenu(bool subscribe)
        {
            if (subscribe)
            {
                _operationTypeService.OperationTypeChanged += OperationTypeService_OperationTypeChanged;
                foreach (var item in ShapeMenus)
                {
                    item.PropertyChanged += OperationShapeMenu_PropertyChanged;
                }
            }
            else
            {
                _operationTypeService.OperationTypeChanged -= OperationTypeService_OperationTypeChanged;
                foreach (var item in ShapeMenus)
                {
                    item.PropertyChanged -= OperationShapeMenu_PropertyChanged;
                }
            }
        }

        private void OperationTypeService_OperationTypeChanged(OperationType obj)
        {
            if (obj.Equals(OperationType.DrawGraphic))
            {
                ManageShapeMenuVisibility(true);
            }
            else
            {
                ManageShapeMenuVisibility(false);
            }
        }

        private void OperationShapeMenu_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var operationShapeMenu = sender as OperationShapeMenu;

            if (e.PropertyName.Equals(nameof(operationShapeMenu.IsExpanded)) && operationShapeMenu.IsExpanded)
            {
                ManageSubscribeForOperationShapeMenu(false);

                foreach (var item in ShapeMenus.Where(x => x.ShapeMenuName != operationShapeMenu.ShapeMenuName))
                {
                    item.IsExpanded = false;
                }

                ManageSubscribeForOperationShapeMenu(true);
            }
        }

        private void InitializeData()
        {
            InitializeSelectedShape();
        }

        protected override void OnLoaded(object parameter)
        {
            InitializeOperationMenus();
            InitializeData();
            ManageSubscribe(true);
        }

        private void ManageSubscribe(bool subscribe)
        {
            if (subscribe)
            {
                _operationTypeService.OperationTypeChanged += OperationTypeChangedHandler;
            }
            else
            {
                _operationTypeService.OperationTypeChanged += OperationTypeChangedHandler;
            }
            ManageSubscribeForOperationShapeMenu(true);
        }

        private void OperationTypeChangedHandler(OperationType operationType)
        {
            if (operationType.Equals(OperationType.Move) || operationType.Equals(OperationType.Delete))
            {
                SelectedShapeType = ShapeMenus.FirstOrDefault(x => x.ShapeMenuName.Equals(NormalShapeName)).ShapeTypes.FirstOrDefault(x => x.Equals(nameof(ShapeType.Rectangle)));
            }
        }

        private void ManageShapeMenuVisibility(bool visibility)
        {
            foreach (var item in ShapeMenus)
            {
                item.IsEnable = visibility;
            }
        }
    }
}
