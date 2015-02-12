using System;
using System.Drawing;

namespace ImageDiff.Analyzers
{
    internal class CIE76Analyzer : IBitmapAnalyzer
    {
        private double JustNoticeableDifference { get; set; }

        public CIE76Analyzer(double justNoticeableDifference)
        {
            JustNoticeableDifference = justNoticeableDifference;
        }

        public bool[,] Analyze(Bitmap first, Bitmap second)
        {
            var diff = new bool[first.Width, first.Height];
            for (var x = 0; x < first.Width; x++)
            {
                for (var y = 0; y < first.Height; y++)
                {
                    var firstLab = CIELab.FromRGB(first.GetPixel(x, y));
                    var secondLab = CIELab.FromRGB(second.GetPixel(x, y));

                    var score = Math.Sqrt(Math.Pow(secondLab.L - firstLab.L, 2) +
                                          Math.Pow(secondLab.a - firstLab.a, 2) +
                                          Math.Pow(secondLab.b - firstLab.b, 2));

                    diff[x, y] = (score >= JustNoticeableDifference);
                }
            }
            return diff;
        }
    }
}
