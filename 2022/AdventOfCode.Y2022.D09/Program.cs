namespace AdventOfCode.Y2022.D09
{
    internal class Program
    {
        private readonly static HashSet<Position> _tailPositions = new HashSet<Position>();

        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");

            PartOne(input);
            PartTwo(input);
        }
        private static void PartTwo(string[] input)
        { 
            _tailPositions.Clear();
            var knotsCount = 10;
            var longRope = Enumerable.Range(0, knotsCount).Select(_ => new Position()).ToArray();
            foreach (var command in input)
            {
                var commandParts = command.Split(" ");
                var direction = GetDirection(commandParts[0]);
                var distance = int.Parse(commandParts[1]);

                for (int i = 0; i < distance; ++i)
                {
                    longRope[0] = MoveHead(longRope[0], direction);
                    for (int j = 1; j < knotsCount; j++)
                    {
                        longRope[j] = MoveTail(longRope[j - 1], longRope[j]);
                    }
                    _tailPositions.Add(longRope[knotsCount-1]);
                }
            }
            Console.WriteLine($"Part Two: Total locations for tail of long rope: {_tailPositions.Count}");
        }

        private static void PartOne(string[] input)
        {
            var headPosition = new Position();
            var tailPosition = new Position();
            _tailPositions.Add(tailPosition);

            foreach (var command in input)
            {
                var commandParts = command.Split(" ");
                var direction = GetDirection(commandParts[0]);
                var distance = int.Parse(commandParts[1]);

                for (int i = 0; i < distance; ++i)
                {
                    headPosition = MoveHead(headPosition, direction);
                    tailPosition = MoveTail(headPosition, tailPosition);
                    _tailPositions.Add(tailPosition);
                }
            }
            Console.WriteLine($"Part One: Total locations for tail: {_tailPositions.Count}");
        }

        private static Position MoveHead(Position currentPosition, Direction direction)
        {
            switch (direction)
            {
                case Direction.Up:
                    currentPosition.Y += 1;
                    break;
                case Direction.Down:
                    currentPosition.Y -= 1;
                    break;
                case Direction.Left:
                    currentPosition.X -= 1;
                    break;
                case Direction.Right:
                    currentPosition.X += 1;
                    break;
            }
            return currentPosition;
        }

        private static Position MoveTail(Position headPosition, Position tailPosition)
        {
            // if already adjacent/in same place
            if (Math.Abs(headPosition.X - tailPosition.X) <= 1 && Math.Abs(headPosition.Y - tailPosition.Y) <= 1)
            {
                return tailPosition;
            }
            tailPosition = new Position()
            {
                X = tailPosition.X,
                Y = tailPosition.Y
            };
            // if on same row, move closer
            if (headPosition.X == tailPosition.X)
            {
                // move tail to be one more/less than head
                tailPosition.Y = (headPosition.Y > tailPosition.Y)
                    ? headPosition.Y - 1
                    : headPosition.Y + 1;
                return tailPosition;
            }
            // if on same column, move closer
            if (headPosition.Y == tailPosition.Y)
            {
                tailPosition.X = (headPosition.X > tailPosition.X)
                    ? headPosition.X - 1
                    : headPosition.X + 1;
                return tailPosition;
            }
            // On different rows and columns, move diagonally
            tailPosition.X = (headPosition.X > tailPosition.X)
                    ? tailPosition.X + 1
                    : tailPosition.X - 1;
            tailPosition.Y = (headPosition.Y > tailPosition.Y)
                    ? tailPosition.Y + 1
                    : tailPosition.Y - 1;
            return tailPosition;
        }

        private static Direction GetDirection(string directionString)
        {
            return directionString switch
            {
                "U" => Direction.Up,
                "D" => Direction.Down,
                "L" => Direction.Left,
                "R" => Direction.Right,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public enum Direction
        {
            Up,
            Down,
            Left,
            Right
        };

        internal class Position
        {
            public int X { get; set; }
            public int Y { get; set; }

            public override bool Equals(object? obj)
            {
                return obj is Position position &&
                       X == position.X &&
                       Y == position.Y;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(X, Y);
            }

            public override string ToString()
            {
                return $"[{X}, {Y}]";
            }
        }
    }
}