using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using WpfPracticeDemo.Enums;
using WpfPracticeDemo.Models;

namespace WpfPracticeDemo.Interfaces
{
    public interface IGeometryService
    {
        Point LeftButtonDownPoint { set; }

        Point LeftButtonUpPoint { set; }

        ShapeBase SelectedShape {set; }

        Geometry GetGeometry();
    }
}
