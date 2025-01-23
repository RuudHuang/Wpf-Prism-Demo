using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using WpfPracticeDemo.Enums;
using WpfPracticeDemo.Interfaces;
using WpfPracticeDemo.Models;

namespace WpfPracticeDemo.Services
{
    internal class GeometryService: IGeometryService
    {
        private Point _leftButtonDownPoint;

        private Point _leftButtonUpPoint;

        private ShapeBase _selectedShape;

        public Point LeftButtonDownPoint 
        {
            set { _leftButtonDownPoint = value; } 
        }
        public Point LeftButtonUpPoint
        {
            set
            {
                _leftButtonUpPoint = value;
            } 
        }       

        public ShapeBase SelectedShape
        {
            set
            { 
               _selectedShape = value;
            }
        }

        public Geometry GetGeometry()
        {
           return _selectedShape.GetGeometry(_leftButtonDownPoint, _leftButtonUpPoint);
        }
        

        
    }
}
