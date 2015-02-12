using System;

namespace ImageDiff.Labelers
{
    internal static class LabelerFactory
    {
        public static IDifferenceLabeler Create(LabelerTypes types, int padding)
        {
            switch (types)
            {
                case LabelerTypes.Basic:
                    return new BasicLabeler();
                case LabelerTypes.ConnectedComponentLabeling:
                    return new ConnectedComponentLabeler(padding);
                default:
                    throw new ArgumentException(string.Format("Unrecognized Analyzer Type: {0}", types));
            }
        }
    }
}