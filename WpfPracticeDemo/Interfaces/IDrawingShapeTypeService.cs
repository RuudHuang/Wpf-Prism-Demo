using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
