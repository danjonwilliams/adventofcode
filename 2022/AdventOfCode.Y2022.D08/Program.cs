namespace AdventOfCode.Y2022.D08
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");

            var forest = CreateForest(input);

            var visibilities = CalculateVisibilites(forest);

            var visibleCount = 0;
            for (int x = 0; x < visibilities.GetLength(0); x++)
            {
                for (int y = 0; y < visibilities.GetLength(1); y++)
                {
                    if (visibilities[x,y].IsVisible())
                    {
                        visibleCount++;
                    }
                }
            }
            Console.WriteLine($"Part One: Total Visible: {visibleCount}");

            var spaces = CalculateSpaces(forest);
            var mostSpace = 0;
            for (int x = 0; x < spaces.GetLength(0); x++)
            {
                for (int y = 0; y < spaces.GetLength(1); y++)
                {
                    var space = spaces[x, y].GetSpace();
                    if (space > mostSpace)
                    {
                        mostSpace = space;
                    }
                }
            }
            Console.WriteLine($"Part Two: Most space: {mostSpace}");
        }

        private static TreeVisibility[,] CalculateVisibilites(int[,] forest)
        {
            var visibility = CreateVisibilityTracker(forest);

            for (int x = 0; x < forest.GetLength(0); x++)
            {
                for (int y = 0; y < forest.GetLength(1); y++)
                {
                    visibility[x, y].FromNorth = IsVisible(forest, x, y, Direction.FromNorth);
                    visibility[x, y].FromSouth = IsVisible(forest, x, y, Direction.FromSouth);
                    visibility[x, y].FromEast = IsVisible(forest, x, y, Direction.FromEast);
                    visibility[x, y].FromWest = IsVisible(forest, x, y, Direction.FromWest);
                }
            }

            return visibility;
        }

        private static TreeSpace[,] CalculateSpaces(int[,] forest)
        {
            var spaces = CreateSpaceTracker(forest);

            for (int x = 0; x < forest.GetLength(0); x++)
            {
                for (int y = 0; y < forest.GetLength(1); y++)
                {
                    spaces[x, y].SmallerTreesNorth = SmallerTreeCount(forest, x, y, Direction.FromNorth);
                    spaces[x, y].SmallerTreesSouth = SmallerTreeCount(forest, x, y, Direction.FromSouth);
                    spaces[x, y].SmallerTreesEast = SmallerTreeCount(forest, x, y, Direction.FromEast);
                    spaces[x, y].SmallerTreesWest = SmallerTreeCount(forest, x, y, Direction.FromWest);
                }
            }

            return spaces;
        }

        private static bool IsVisible(int[,] forest, int x, int y, Direction direction)
        {
            var maxX = forest.GetLength(0)-1;
            var maxY = forest.GetLength(1)-1;
            var height = forest[x, y];
            NextCoordinates(ref x, ref y, direction);
            while (x >= 0 && y >= 0 && x <= maxX && y <= maxY)
            {
                if (forest[x, y] >= height)
                {
                    return false;
                }
                NextCoordinates(ref x, ref y, direction);
            }
            return true;
        }

        private static int SmallerTreeCount(int[,] forest, int x, int y, Direction direction)
        {
            var maxX = forest.GetLength(0) - 1;
            var maxY = forest.GetLength(1) - 1;
            var height = forest[x, y];
            int treeCount = 0;
            if (x == 0 || y == 0 || x == maxX || y == maxY)
            {
                return 0;
            }
            
            while (x > 0 && y > 0 && x < maxX && y < maxY)
            {
                treeCount++;
                NextCoordinates(ref x, ref y, direction);
                if (forest[x, y] >= height)
                {
                    break;
                }
            }
            return treeCount;
        }

        private static void NextCoordinates(ref int x, ref int y, Direction direction)
        {
            switch (direction)
            {
                case Direction.FromNorth:
                    y--;
                    break;
                case Direction.FromSouth:
                    y++;
                    break;
                case Direction.FromEast:
                    x++;
                    break;
                case Direction.FromWest:
                    x--;
                    break;
            }
        }

        private static int[,] CreateForest(string[] input)
        {
            var x = input[0].Length;
            var y = input.Length;
            var forest = new int[x, y];
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    forest[i, j] = int.Parse(input[j][i].ToString());
                }
            }
            return forest;
        }

        private static TreeVisibility[,] CreateVisibilityTracker(int[,] forest)
        {
            var maxX = forest.GetLength(0);
            var maxY = forest.GetLength(1); 
            var visibility = new TreeVisibility[maxX, maxY];
            for (int x = 0; x < maxX; x++)
            {
                for (int y = 0; y < maxY; y++)
                {
                    visibility[x, y] = new TreeVisibility();
                }
            }
            return visibility;
        }

        private static TreeSpace[,] CreateSpaceTracker(int[,] forest)
        {
            var maxX = forest.GetLength(0);
            var maxY = forest.GetLength(1);
            var spaces = new TreeSpace[maxX, maxY];
            for (int x = 0; x < maxX; x++)
            {
                for (int y = 0; y < maxY; y++)
                {
                    spaces[x, y] = new TreeSpace();
                }
            }
            return spaces;
        }
    }

    internal enum Direction
    {
        FromNorth,
        FromSouth,
        FromEast,
        FromWest
    }

    internal record TreeVisibility
    {
        public bool FromNorth { get; set; }
        public bool FromSouth { get; set; }
        public bool FromEast { get; set; }
        public bool FromWest { get; set; }

        public bool IsVisible()
        {
            return FromNorth || FromSouth || FromEast || FromWest;
        }
    }

    internal record TreeSpace
    {
        public int SmallerTreesNorth { get; set; }
        public int SmallerTreesSouth{ get; set; }
        public int SmallerTreesEast { get; set; }
        public int SmallerTreesWest { get; set; }

        public int GetSpace()
        {
            return SmallerTreesNorth * SmallerTreesSouth * SmallerTreesEast * SmallerTreesWest;
        }
    }
}