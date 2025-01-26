using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
