using System;

namespace ImageDiff.Analyzers
{
    internal static class BitmapAnalyzerFactory
    {
        public static IBitmapAnalyzer Create(AnalyzerTypes type, double justNoticeableDifference)
        {
            switch (type)
            {
                case AnalyzerTypes.ExactMatch:
                    return new ExactMatchAnalyzer();
                case AnalyzerTypes.CIE76:
                    return new CIE76Analyzer(justNoticeableDifference);
                default:
                    throw new ArgumentException(string.Format("Unrecognized Difference Detection Mode: {0}", type));
            }
        }
    }
}