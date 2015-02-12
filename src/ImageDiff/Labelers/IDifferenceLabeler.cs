namespace ImageDiff.Labelers
{
    internal interface IDifferenceLabeler
    {
        int[,] Label(bool[,] differenceMap);
    }
}