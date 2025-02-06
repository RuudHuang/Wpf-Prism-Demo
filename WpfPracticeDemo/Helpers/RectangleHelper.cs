using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfPracticeDemo.Helpers
{
    public class RectangleHelper
    {
        public static Point GetRectangleLocation(Point leftButtonDownPoint, Point leftButtonUpPoint)
        {

            if (leftButtonUpPoint.X > leftButtonDownPoint.X)
            {
                if (leftButtonUpPoint.Y > leftButtonDownPoint.Y)
                {
                    return leftButtonDownPoint;
                }
                else
                {
                    return new Point(leftButtonDownPoint.X, leftButtonUpPoint.Y);
                }
            }
            else
            {
                if (leftButtonUpPoint.Y > leftButtonDownPoint.Y)
                {
                    return new Point(leftButtonUpPoint.X, leftButtonDownPoint.Y);
                }
                else
                {
                    return leftButtonUpPoint;
                }
            }
        }

        public static Size GetRectangleSize(Point leftButtonDownButtonPoint, Point leftButtonUpPoint)
        {
            Size rectangleSize = new Size()
            {
                Height = Math.Abs(leftButtonDownButtonPoint.Y - leftButtonUpPoint.Y),
                Width = Math.Abs(leftButtonDownButtonPoint.X - leftButtonUpPoint.X)
            };

            return rectangleSize;
        }
    }
}
