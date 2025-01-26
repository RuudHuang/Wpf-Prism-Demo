using Prism.Events;
using System.Collections.ObjectModel;
using System.Linq;
using WpfPracticeDemo.Enums;
using WpfPracticeDemo.Interfaces;
using WpfPracticeDemo.Models;

namespace WpfPracticeDemo.ViewModels
{
    internal class ShapeOptionViewModel : DemoVmBase
    {

        private const string NormalShapeName = "NormalShape";

        private string _selectedShapeType;

        private readonly IDrawingShapeTypeService _drawingShapeTypeService;

        private readonly IOperationTypeService _operationTypeService;

        private readonly ObservableCollection<OperationShapeMenu> _shapeMenus;

        public ObservableCollection<OperationShapeMenu> ShapeMenus
        {
            get { return _shapeMenus; }
        }

        public string SelectedShapeType
        {
            get { return _selectedShapeType; }

            set
            {
                if (SetProperty(ref _selectedShapeType, value))
                {
                    var selectedDrawingShapeType = GetShapeType(value);
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
            };

            shapeMenu.ShapeTypes.Add(nameof(ShapeType.Line));
            shapeMenu.ShapeTypes.Add(nameof(ShapeType.Rectangle));
            shapeMenu.ShapeTypes.Add(nameof(ShapeType.Circle));

            _shapeMenus.Add(shapeMenu);

            OperationShapeMenu shapeMenu1 = new OperationShapeMenu()
            {
                ShapeMenuName = "Shape1"
            };

            _shapeMenus.Add(shapeMenu1);

            OperationShapeMenu shapeMenu2 = new OperationShapeMenu()
            {
                ShapeMenuName = "Shape2"
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
                foreach (var item in ShapeMenus)
                {
                    item.PropertyChanged += OperationShapeMenu_PropertyChanged;
                }
            }
            else
            {
                foreach (var item in ShapeMenus)
                {
                    item.PropertyChanged -= OperationShapeMenu_PropertyChanged;
                }
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
            if (operationType.Equals(OperationType.Select))
            {
                SelectedShapeType = ShapeMenus.FirstOrDefault(x => x.ShapeMenuName.Equals(NormalShapeName)).ShapeTypes.FirstOrDefault(x => x.Equals(nameof(ShapeType.Rectangle)));
            }
        }

        private ShapeType GetShapeType(string shapeTypeName)
        {
            switch (shapeTypeName)
            {
                case nameof(ShapeType.Line):
                    return ShapeType.Line;
                case nameof(ShapeType.Rectangle):
                    return ShapeType.Rectangle;
                case nameof(ShapeType.Circle):
                    return ShapeType.Circle;
                default:
                    return ShapeType.Line;
            }
        }
    }
}
