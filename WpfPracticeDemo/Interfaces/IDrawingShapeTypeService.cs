using System;
using WpfPracticeDemo.Enums;

namespace WpfPracticeDemo.Interfaces
{
    public interface IDrawingShapeTypeService
    {
        ShapeType CurrentSelectedShapeType { get; }

        void SetSelectedShapeType(ShapeType shapeType);

        event Action<ShapeType> SelectedShapeTypeChanged;
    }
}
