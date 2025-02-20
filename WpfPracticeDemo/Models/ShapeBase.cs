using System.Windows;
using System.Windows.Media;
using WpfPracticeDemo.Enums;

namespace WpfPracticeDemo.Models
{
    public abstract class ShapeBase
    {
        protected const int ShapeSelectedAdornerRadius = 10;
        public abstract string Name { get; }

        public abstract ShapeType Type { get; }

        protected Geometry _currentShapeGeometry;

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

        public abstract Geometry GetRelativeGeometry(Geometry orignalGeometry, ShapeBase shape, GeometryType geometryType, Point leftButtonDownPoint, Point leftButtonUpPoint);

        public bool IsGeometryPointInSelectedRect(Geometry shapeGeometry, Geometry selectedRect)
        {
            if (shapeGeometry.FillContainsWithDetail(selectedRect).Equals(IntersectionDetail.Intersects))
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        public void UpdateGeometry(Geometry geometry)
        {
            _currentShapeGeometry = geometry;
        }

        public bool IsGeometryValidation(Geometry shapeGeometry, Geometry canvasRect)
        {
            var intersectionDetail = canvasRect.FillContainsWithDetail(shapeGeometry);
            if (intersectionDetail.Equals(IntersectionDetail.FullyContains))
            {
                return true;
            }

            else
            {
                return false;
            }
        }

        protected Point GetDeltaPoint(Point leftButtonDownPoint, Point leftButtonUpPoint)
        {
            return new Point(leftButtonUpPoint.X - leftButtonDownPoint.X, leftButtonUpPoint.Y - leftButtonDownPoint.Y);
        }
    }
}
