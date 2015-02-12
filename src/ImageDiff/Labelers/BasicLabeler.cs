namespace ImageDiff.Labelers
{
    internal class BasicLabeler : IDifferenceLabeler
    {
        public int[,] Label(bool[,] differenceMap)
        {
            var width = differenceMap.GetLength(0);
            var height = differenceMap.GetLength(1);
            var analyzedMap = new int[width, height];

            for (var x = 0; x < width; x++)
            {
                for (var y = 0; y < height; y++)
                {
                    if (differenceMap[x, y])
                        analyzedMap[x, y] = 1;
                }
            }
            return analyzedMap;
        }
    }
}