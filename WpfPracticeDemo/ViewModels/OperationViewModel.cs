using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfPracticeDemo.Models;
using WpfPracticeDemo.Shapes;

namespace WpfPracticeDemo.ViewModels
{
    internal class OperationViewModel:BindableBase
    {

        private const string NormalShapeName = "NormalShape";

        private ShapeBase _selectedShape;

        private readonly ObservableCollection<OperationShapeMenu> _shapeMenus;

        public ObservableCollection<OperationShapeMenu> ShapeMenus
        { 
            get { return _shapeMenus; }
        }

        public ShapeBase SelectedShape
        {
            get { return _selectedShape; }

            set
            {
                SetProperty(ref _selectedShape, value);
            }
        }


        public OperationViewModel()
        { 
           _shapeMenus = new ObservableCollection<OperationShapeMenu>();

            InitializeOperationMenus();
            InitializeSelectedShape();
            ManageSubscribeForOperationShapeMenu(true);
        }

        private void InitializeOperationMenus()
        {
            OperationShapeMenu shapeMenu = new OperationShapeMenu()
            {
                ShapeMenuName = NormalShapeName,
                IsExpanded = true,
            };            

            shapeMenu.Shapes.Add(new LineShape());
            shapeMenu.Shapes.Add(new RectangleShape());
            shapeMenu.Shapes.Add(new CircleShape());

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
            SelectedShape=ShapeMenus.FirstOrDefault(x=>x.ShapeMenuName.Equals(NormalShapeName))?.Shapes[0];
        }

        private void ManageSubscribe(bool subscribe)
        {
            ManageSubscribeForOperationShapeMenu(subscribe);
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

        
    }
}
