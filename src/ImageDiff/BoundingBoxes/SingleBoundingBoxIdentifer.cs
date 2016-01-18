using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ImageDiff.BoundingBoxes
{
    internal class SingleBoundingBoxIdentifer : IBoundingBoxIdentifier
    {
        private int Padding { get; set; }

        public SingleBoundingBoxIdentifer(int padding)
        {
            Padding = padding;
        }

        public IEnumerable<Rectangle> CreateBoundingBoxes(int[,] labelMap)
        {
            var points = FindLabeledPoints(labelMap);

            if (!points.Any())
                return new List<Rectangle>();

            var minPoint = new Point(points.Min(x => x.X), points.Min(y => y.Y));
            var maxPoint = new Point(points.Max(x => x.X), points.Max(y => y.Y));

            var rectangle = new Rectangle(minPoint.X - Padding,
                minPoint.Y - Padding,
                (maxPoint.X - minPoint.X) + (Padding * 2),
                (maxPoint.Y - minPoint.Y) + (Padding * 2));

            return new List<Rectangle> { rectangle };
        }

        private static List<Point> FindLabeledPoints(int[,] labelMap)
        {
            var width = labelMap.GetLength(0);
            var height = labelMap.GetLength(1);
            var points = new List<Point>();
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    if (labelMap[x, y] > 0)
                        points.Add(new Point(x, y));
                }
            }
            return points;
        }
    }
}