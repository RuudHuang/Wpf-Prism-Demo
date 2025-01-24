using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using WpfPracticeDemo.Enums;

namespace WpfPracticeDemo.Models
{
    public abstract class ShapeBase
    {

        public abstract string Name { get; }

        public abstract ShapeType Type { get; }

        public abstract Geometry CurrentShapeGeometry { get; }

        public Geometry CreateGeometry(GeometryType geometryType, Point leftButtonDownPoint, Point leftButtonUpPoint)
        {
            switch (geometryType)
            {
                case GeometryType.Shape:
                    return CreateShapeGeometry(leftButtonDownPoint, leftButtonUpPoint);
                case GeometryType.Selected:
                    return CreateShapeSelectedGeometry(leftButtonDownPoint, leftButtonUpPoint);
                case GeometryType.MouseOver:
                    return CreateMouseOverGeometry(leftButtonDownPoint, leftButtonUpPoint);
                default:
                    return CreateShapeGeometry(leftButtonDownPoint, leftButtonUpPoint);
            }
        }

        protected abstract Geometry CreateShapeGeometry(Point leftButtonDownPoint, Point leftButtonUpPoint);
        protected abstract Geometry CreateMouseOverGeometry(Point leftButtonDownPoint, Point leftButtonUpPoint);
        protected abstract Geometry CreateShapeSelectedGeometry(Point leftButtonDownPoint, Point leftButtonUpPoint);

        public abstract Geometry UpdateGeometry(Geometry orignalGeometry, ShapeBase shape, GeometryType geometryType, Point leftButtonDownPoint, Point leftButtonUpPoint);


        public virtual bool IsGeometryValidation(Rect canvasRect)
        {
            return true;
        }
    }
}
