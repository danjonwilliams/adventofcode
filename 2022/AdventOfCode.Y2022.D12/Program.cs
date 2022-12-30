using System.Text;

namespace AdventOfCode.Y2022.D12
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("testinput.txt");

            var map = new Map(input);
            var startLocation = map.GetStartLocation();

            var distanceTravelled = FindHighestPoint(map, new[] { startLocation });
            Console.WriteLine($"Part One: Distance travelled : {distanceTravelled}");

            var lowestElevations = map.GetLocations('a').ToArray();
            var shortestPossible = FindHighestPoint(map, lowestElevations);
            Console.WriteLine($"Part Two: Shorted distance travelled : {shortestPossible}");
        }

        private static int FindHighestPoint(Map map, Location[] startLocations)
        {
            var currentLocations = new List<Location>(startLocations);
            var nextLocations = new List<Location>();
            var processedLocations = new HashSet<Location>();
            int stepsTaken = 0;
            bool endReached = false;
            while (!endReached  && currentLocations.Any())
            {
                foreach (var location in currentLocations)
                {
                    foreach (var nextMove in map.GetPossibleMoves(location))
                    {
                        // If we've been here already, skip
                        if (processedLocations.Contains(nextMove))
                        {
                            continue;
                        }

                        // This is a possible contender, so move to that space
                        processedLocations.Add(nextMove);
                        nextLocations.Add(nextMove);
                        if (nextMove.IsEnd)
                        {
                            endReached = true;
                            return stepsTaken + 1;
                        }
                    }
                }
                var tmp = currentLocations;
                currentLocations = nextLocations;
                nextLocations = tmp;
                nextLocations.Clear();
                stepsTaken++;
            }
            return -1;
        }

        internal class Location
        {
            internal static readonly Location Null = new Location(-1, -1, 'Z');

            public Location(int x, int y, char altitude)
            {
                X = x;
                Y = y;
                if (altitude == 'S')
                {
                    Altitude = 'a';
                    IsStart = true;
                }
                else if (altitude == 'E')
                {
                    Altitude = 'z';
                    IsEnd = true;
                }
                else
                {
                    Altitude = altitude;
                }
            }

            public int X { get; private set; }
            public int Y { get; private set; }
            public char Altitude { get; private set; }
            public bool IsStart { get; private set; }
            public bool IsEnd { get; private set; }

            public override bool Equals(object? obj)
            {
                return obj is Location location &&
                       X == location.X &&
                       Y == location.Y;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(X, Y);
            }
        }

        internal class Map
        {
            private readonly Location[,] _map;

            public Map(string[] mapDefinition)
            {
                var sizeX = mapDefinition[0].Length;
                var sizeY = mapDefinition.Length;
                _map = new Location[sizeX, sizeY];
                int x = 0;
                int y = 0;
                foreach (var mapLine in mapDefinition)
                {
                    foreach (var altitude in mapLine)
                    {
                        _map[x, y] = new Location(x, y, altitude);
                        x++;
                    }
                    y++;
                    x = 0;
                }
            }

            public Location GetStartLocation()
            {
                for (int x = 0; x < _map.GetLength(0); x++)
                {
                    for (int y = 0; y < _map.GetLength(1); y++)
                    {
                        if (_map[x, y].IsStart)
                        {
                            return _map[x, y];
                        }
                    }
                }
                throw new InvalidOperationException("Map does not have start location");
            }

            public IEnumerable<Location> GetPossibleMoves(Location currentLocation)
            {
                var x = currentLocation.X - 1;
                var y = currentLocation.Y;
                Location possibleMove;
                if (TryGetLocation(x, y, out possibleMove) && IsValidMove(currentLocation, possibleMove))
                {
                    yield return possibleMove;
                }
                x = currentLocation.X + 1;
                if (TryGetLocation(x, y, out possibleMove) && IsValidMove(currentLocation, possibleMove))
                {
                    yield return possibleMove;
                }
                x = currentLocation.X;
                y = currentLocation.Y - 1;
                if (TryGetLocation(x, y, out possibleMove) && IsValidMove(currentLocation, possibleMove))
                {   
                    yield return possibleMove;
                }
                y = currentLocation.Y + 1;
                if (TryGetLocation(x, y, out possibleMove) && IsValidMove(currentLocation, possibleMove))
                {
                    yield return possibleMove;
                }
            }

            public bool TryGetLocation(int x, int y, out Location location)
            {
                if (x < 0 || y < 0 || x >= _map.GetLength(0) || y >= _map.GetLength(1))
                {
                    location = Location.Null;
                    return false;
                }
                location = _map[x, y];
                return true;
            }

            public IEnumerable<Location> GetLocations(char altitude)
            {
                for (int x = 0; x < _map.GetLength(0); x++)
                {
                    for (int y = 0; y < _map.GetLength(1); y++)
                    {
                        if (_map[x, y].Altitude == altitude)
                        {
                            yield return _map[x, y];
                        }
                    }
                }
            }

            public string PrintMap()
            {
                var sb = new StringBuilder();

                for (int y = 0; y < _map.GetLength(1); y++)
                {
                    for (int x = 0; x < _map.GetLength(0); x++)
                    {
                        sb.Append(_map[x, y]);
                    }
                    sb.AppendLine("");
                }
                return sb.ToString();
            }

            private static bool IsValidMove(Location currentLocation, Location nextLocation)
            {
                return nextLocation.Altitude <= currentLocation.Altitude + 1;
            }
        }
    }
}