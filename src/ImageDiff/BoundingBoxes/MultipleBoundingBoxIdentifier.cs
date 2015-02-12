using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ImageDiff.BoundingBoxes
{
    internal class MultipleBoundingBoxIdentifier : IBoundingBoxIdentifier
    {
        private int Padding { get; set; }

        public MultipleBoundingBoxIdentifier(int padding)
        {
            Padding = padding;
        }

        public IEnumerable<Rectangle> CreateBoundingBoxes(int[,] labelMap)
        {
            var boundedPoints = FindLabeledPointGroups(labelMap);
            var boundingRectangles = CreateBoundingBoxes(boundedPoints);
            return boundingRectangles;
        }

        private IEnumerable<Rectangle> CreateBoundingBoxes(Dictionary<int, List<Point>> boundedPoints)
        {
            if (boundedPoints == null || boundedPoints.Count == 0)
                yield break;

            foreach (var kvp in boundedPoints)
            {
                var points = kvp.Value;
                var minPoint = new Point(points.Min(x => x.X), points.Min(y => y.Y));
                var maxPoint = new Point(points.Max(x => x.X), points.Max(y => y.Y));
                var rectangle = new Rectangle(minPoint.X - Padding,
                    minPoint.Y - Padding,
                    (maxPoint.X - minPoint.X) + (Padding * 2),
                    (maxPoint.Y - minPoint.Y) + (Padding * 2));

                yield return rectangle;
            }
        }

        private static Dictionary<int, List<Point>> FindLabeledPointGroups(int[,] labelMap)
        {
            var width = labelMap.GetLength(0);
            var height = labelMap.GetLength(1);

            var boundedPoints = new Dictionary<int, List<Point>>();
            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    if (labelMap[x, y] == 0) continue;

                    var label = labelMap[x, y];
                    if (!boundedPoints.ContainsKey(label))
                        boundedPoints.Add(label, new List<Point> { new Point(x, y) });
                    else
                        boundedPoints[label].Add(new Point(x, y));
                }
            }
            return boundedPoints;
        }
    }
}