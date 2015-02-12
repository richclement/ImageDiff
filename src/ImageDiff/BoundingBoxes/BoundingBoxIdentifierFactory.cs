using System;

namespace ImageDiff.BoundingBoxes
{
    internal static class BoundingBoxIdentifierFactory
    {
        public static IBoundingBoxIdentifier Create(BoundingBoxModes mode, int padding)
        {
            switch (mode)
            {
                case BoundingBoxModes.Single:
                    return new SingleBoundingBoxIdentifer(padding);
                case BoundingBoxModes.Multiple:
                    return new MultipleBoundingBoxIdentifier(padding);
                default:
                    throw new ArgumentException(string.Format("Unrecognized Bounding Box Mode: {0}", mode));
            }
        }
    }
}