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
        protected const int ShapeSelectedAdornerRadius = 10;
        public abstract string Name { get; }

        public abstract ShapeType Type { get; }

        protected Geometry _currentShapeGeometry;

        public Geometry CreateGeometry(GeometryType geometryType, Point leftButtonDownPoint, Point leftButtonUpPoint,bool updateOrignalGeometry)
        {
            switch (geometryType)
            {
                case GeometryType.Shape:
                    return CreateShapeGeometry(leftButtonDownPoint, leftButtonUpPoint,updateOrignalGeometry);
                case GeometryType.Selected:
                    return CreateShapeSelectedGeometry(leftButtonDownPoint, leftButtonUpPoint);
                case GeometryType.MouseOver:
                    return CreateMouseOverGeometry(leftButtonDownPoint, leftButtonUpPoint);
                default:
                    return CreateShapeGeometry(leftButtonDownPoint, leftButtonUpPoint, updateOrignalGeometry);
            }
        }

        protected abstract Geometry CreateShapeGeometry(Point leftButtonDownPoint, Point leftButtonUpPoint, bool updateOrignalGeometry);       

        protected abstract Geometry CreateMouseOverGeometry(Point leftButtonDownPoint, Point leftButtonUpPoint);
        protected abstract Geometry CreateShapeSelectedGeometry(Point leftButtonDownPoint, Point leftButtonUpPoint);

        public abstract Geometry GetRelativeGeometry(Geometry orignalGeometry, ShapeBase shape, GeometryType geometryType, Point leftButtonDownPoint, Point leftButtonUpPoint, bool isUpdateGeometry);

        public abstract bool IsGeometryPointInSelectedRect(Geometry shapeGeometry, Rect selectedRect);

        public virtual bool IsGeometryValidation(Geometry shapeGeometry,Rect canvasRect)
        {
            return true;
        }

        protected Point GetDeltaPoint(Point leftButtonDownPoint, Point leftButtonUpPoint)
        {
            return new Point(leftButtonUpPoint.X - leftButtonDownPoint.X, leftButtonUpPoint.Y - leftButtonDownPoint.Y);
        }
    }
}
