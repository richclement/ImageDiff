using System.Drawing;

namespace ImageDiff.Analyzers
{
    internal interface IBitmapAnalyzer
    {
        bool[,] Analyze(Bitmap first, Bitmap second);
    }
}