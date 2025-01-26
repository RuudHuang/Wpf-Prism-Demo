using System;
using WpfPracticeDemo.Enums;

namespace WpfPracticeDemo.Interfaces
{
    public interface IOperationTypeService
    {
        OperationType CurrentOperationType { get; }

        void SetOperationType(OperationType operationType);

        event Action<OperationType> OperationTypeChanged;
    }
}
