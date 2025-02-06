
using System.ComponentModel;

namespace WpfPracticeDemo.Enums
{
    public enum ShapeType
    {
        [Description(nameof(ShapeType.Line))]
        Line,

        [Description(nameof(ShapeType.Rectangle))]
        Rectangle,

        [Description(nameof(ShapeType.Circle))]
        Circle
    }
}
