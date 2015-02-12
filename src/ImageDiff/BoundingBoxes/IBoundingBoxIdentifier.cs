using System.Collections.Generic;
using System.Drawing;

namespace ImageDiff.BoundingBoxes
{
    internal interface IBoundingBoxIdentifier
    {
        IEnumerable<Rectangle> CreateBoundingBoxes(int[,] labelMap);
    }
}