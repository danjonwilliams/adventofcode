using System;

namespace AdventOfCode.Y2022.D01
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");
            var elfTotals = new List<int>();
            int currentTotal = 0;

            foreach (var calorieValue in input)
            {
                if (string.IsNullOrWhiteSpace(calorieValue))
                {
                    elfTotals.Add(currentTotal);
                    currentTotal = 0;
                    continue;
                }
                currentTotal += int.Parse(calorieValue);
            }
            elfTotals.Add(currentTotal);

            Console.WriteLine(elfTotals.Max());

            elfTotals.Sort((x, y) => x < y ? 1 : x == y ? 0 : -1);

            var top3 = elfTotals.Take(3);
            Console.WriteLine(top3.Sum());
        }
    }
}