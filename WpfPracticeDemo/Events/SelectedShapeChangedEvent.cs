using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WpfPracticeDemo.Models;

namespace WpfPracticeDemo.Events
{
    internal class SelectedShapeChangedEvent:PubSubEvent<SelectedShapeChangedEventArgs>
    {
    }
}
