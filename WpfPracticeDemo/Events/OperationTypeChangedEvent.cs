using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfPracticeDemo.Enums;

namespace WpfPracticeDemo.Events
{
    internal class OperationTypeChangedEvent:PubSubEvent<OperationType>
    {
    }
}
