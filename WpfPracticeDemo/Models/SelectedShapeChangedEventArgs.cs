using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfPracticeDemo.Enums;

namespace WpfPracticeDemo.Models
{
    internal class SelectedShapeChangedEventArgs
    {
        public ShapeType SelectedShapeType { get; set; }
    }
}
