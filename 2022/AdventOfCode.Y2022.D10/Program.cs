using System.Runtime.CompilerServices;

namespace AdventOfCode.Y2022.D10
{
    internal class Program
    {
        private static readonly HashSet<int> _keyCycles = new HashSet<int>() { 20, 60, 100, 140, 180, 220 };
        private static readonly List<int> _signalStrengths = new List<int>(6);
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");

            var X = 1;
            var cycle = 0;

            foreach (var command in input)
            {
                var commandParts = command.Split(" ");
                Execute(commandParts, ref X, ref cycle);
            }

            Console.WriteLine($"Part One: Total Signal Strength: {_signalStrengths.Sum()}");
        }

        private static void Execute(string[] commandParts, ref int x, ref int cycle)
        {
            switch (commandParts[0])
            {
                case "noop":
                    UpdateDisplay(cycle, x);
                    cycle++;
                    CheckSignalStrength(cycle, x);
                    break;
                case "addx":
                    var v = int.Parse(commandParts[1]);

                    UpdateDisplay(cycle, x);
                    cycle++;
                    CheckSignalStrength(cycle, x);

                    UpdateDisplay(cycle, x);
                    cycle++;
                    CheckSignalStrength(cycle, x);
                    x += v;
                    break;
            }
        }

        private static void CheckSignalStrength(int cycle, int x)
        {
            if (_keyCycles.Contains(cycle))
            {
                _signalStrengths.Add(cycle * x);
            }
        }

        private static void UpdateDisplay(int cycle, int x)
        {
            var crtPosition = cycle % 40;
            if (crtPosition == 0)
            {
                Console.WriteLine("");
            }
            if (crtPosition >= x-1 && crtPosition <= x+1)
            {
                Console.Write('#');
            }
            else
            {
                Console.Write('.');
            }
        }
    }
}