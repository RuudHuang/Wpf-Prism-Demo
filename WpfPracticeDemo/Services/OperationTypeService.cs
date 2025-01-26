using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfPracticeDemo.Enums;
using WpfPracticeDemo.Interfaces;

namespace WpfPracticeDemo.Services
{
    internal class OperationTypeService : IOperationTypeService
    {
        private OperationType _operationType=OperationType.DrawGraphic;

        public OperationType CurrentOperationType => _operationType;

        public event Action<OperationType> OperationTypeChanged;

        public void SetOperationType(OperationType operationType)
        {
            if (_operationType.Equals(operationType))
            {
                return;
            }

            _operationType = operationType;

            OperationTypeChanged?.Invoke(_operationType);
        }
    }
}
