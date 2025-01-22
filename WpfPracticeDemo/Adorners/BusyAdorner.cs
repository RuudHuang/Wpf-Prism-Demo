using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Forms.Design.Behavior;
using Adorner = System.Windows.Documents.Adorner;

namespace WpfPracticeDemo.Adorners
{
    internal class BusyAdorner : Adorner
    {
        public BusyAdorner(UIElement adornedElement) : base(adornedElement)
        {
        }
    }
}
