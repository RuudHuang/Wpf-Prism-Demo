
using System.ComponentModel;
using WpfPracticeDemo.Attributes;

namespace WpfPracticeDemo.Enums
{
    public enum OperationType
    {
        [Description(nameof(OperationType.DrawGraphic))]
        DrawGraphic,

        [Ignore]
        Select,

        [Description(nameof(OperationType.Move))]
        Move,

        [Description(nameof(OperationType.Delete))]
        Delete,

        [Description(nameof(OperationType.AdjustSize))]
        AdjustSize
    }
}
