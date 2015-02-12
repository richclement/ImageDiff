using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ImageDiff.Labelers
{
    internal class ConnectedComponentLabeler : IDifferenceLabeler
    {
        private int Padding { get; set; }
        private int[,] Labels { get; set; }
        private Dictionary<int, List<int>> Linked { get; set; }

        public ConnectedComponentLabeler(int padding)
        {
            Padding = padding;
            Linked = new Dictionary<int, List<int>>();
        }

        public int[,] Label(bool[,] differenceMap)
        {
            var width = differenceMap.GetLength(0);
            var height = differenceMap.GetLength(1);

            Labels = new int[width, height];

            var nextLabel = 1;
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    if (differenceMap[x, y] == false) continue;

                    var neighbors = Neighbors(differenceMap, x, y);
                    if (neighbors == null || neighbors.Count == 0)
                    {
                        Linked.Add(nextLabel, new List<int> { nextLabel });
                        Labels[x, y] = nextLabel;
                        nextLabel += 1;
                    }
                    else
                    {
                        var neighborsLabels = NeighborsLabels(neighbors);
                        Labels[x, y] = neighborsLabels.Min();
                        foreach (var label in neighborsLabels)
                        {
                            Linked[label] = Linked[label].Union(neighborsLabels).ToList();
                        }
                    }
                }
            }

            // second pass
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var currentLabel = Labels[x, y];
                    if (currentLabel == 0) continue;

                    Labels[x, y] = FindLowestEquivalentLabel(currentLabel);
                }
            }

            return Labels;
        }

        private int FindLowestEquivalentLabel(int currentLabel)
        {
            var equivalentLabels = Linked[currentLabel];
            return equivalentLabels.Min();
        }

        private List<int> NeighborsLabels(IEnumerable<Point> neighbors)
        {
            return neighbors.Select(n => Labels[n.X, n.Y]).ToList();
        }

        private List<Point> Neighbors(bool[,] bitmap, int x, int y)
        {
            var points = GenerateNeighbors(bitmap.GetLength(0), x, y);
            return points.Where(p => bitmap[p.X, p.Y]).ToList();
        }

        private IEnumerable<Point> GenerateNeighbors(int width, int x, int y)
        {
            var points = new List<Point>();
            var counter = 0;
            while (counter <= Padding)
            {
                var offset = counter + 1;

                if (x > counter)
                {
                    points.Add(new Point(x - offset, y));
                    if (y > counter) points.Add(new Point(x - offset, y - offset));
                }
                if (y > counter)
                {
                    points.Add(new Point(x, y - offset));
                    if (x < (width - offset)) points.Add(new Point(x + offset, y - offset));
                }
                counter += 1;
            }
            return points;
        }
    }
}