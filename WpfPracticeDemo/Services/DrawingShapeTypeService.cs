using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfPracticeDemo.Enums;
using WpfPracticeDemo.Interfaces;

namespace WpfPracticeDemo.Services
{
    internal class DrawingShapeTypeService : IDrawingShapeTypeService
    {
        private ShapeType _shapeType=ShapeType.Line;

        public ShapeType CurrentSelectedShapeType => _shapeType;

        public event Action<ShapeType> SelectedShapeTypeChanged;

        public void SetSelectedShapeType(ShapeType shapeType)
        {
            if (_shapeType.Equals(shapeType))
            {
                return;
            }

            _shapeType = shapeType;

            SelectedShapeTypeChanged?.Invoke(_shapeType);
        }
    }
}
