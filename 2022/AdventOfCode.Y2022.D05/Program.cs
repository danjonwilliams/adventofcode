namespace AdventOfCode.Y2022.D05
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt").Skip(10).ToArray();
            // Hardcode starting stacks for convenience
            var stacks = new Stack<char>[]
                {
                    new Stack<char>(new[] { 'L', 'N', 'W', 'T', 'D' }),
                    new Stack<char>(new[] {'C', 'P', 'H' }),
                    new Stack<char>(new[] {'W','P','H','N','D','G','M','J'}),
                    new Stack<char>(new[] {'C', 'W', 'S', 'N', 'T', 'Q', 'L' }),
                    new Stack<char>(new[] {'P' ,'H', 'C', 'N' }),
                    new Stack<char>(new[] { 'T','H','N','D','M','W','Q','B'}),
                    new Stack<char>(new[] {'M','B','R','J','G','S','L' }),
                    new Stack<char>(new[] { 'Z','N','W','G','V','B','R','T'}),
                    new Stack<char>(new[] {'W','G','D','N','P','L' }),
                };
            var moves = input.Select(i => new Move(i)).ToArray();

            foreach (var move in moves)
            {
                ApplyMove9000(stacks, move);
            }
            var topCrates = stacks.Select(s => s.Peek()).ToArray();
            Console.WriteLine($"Part One: Top Crates: {string.Join("", topCrates)}");

            stacks = new Stack<char>[]
                {
                    new Stack<char>(new[] { 'L', 'N', 'W', 'T', 'D' }),
                    new Stack<char>(new[] {'C', 'P', 'H' }),
                    new Stack<char>(new[] {'W','P','H','N','D','G','M','J'}),
                    new Stack<char>(new[] {'C', 'W', 'S', 'N', 'T', 'Q', 'L' }),
                    new Stack<char>(new[] {'P' ,'H', 'C', 'N' }),
                    new Stack<char>(new[] { 'T','H','N','D','M','W','Q','B'}),
                    new Stack<char>(new[] {'M','B','R','J','G','S','L' }),
                    new Stack<char>(new[] { 'Z','N','W','G','V','B','R','T'}),
                    new Stack<char>(new[] {'W','G','D','N','P','L' }),
                };

            foreach (var move in moves)
            {
                ApplyMove9001(stacks, move);
            }
            topCrates = stacks.Select(s => s.Peek()).ToArray();
            Console.WriteLine($"Part Two: Top Crates: {string.Join("", topCrates)}");
        }

        private static void ApplyMove9000(Stack<char>[] stacks, Move move)
        {
            for (int i = 0; i < move.Amount; i++)
            {
                char crate = stacks[move.From].Pop();
                stacks[move.To].Push(crate);
            }
        }
        private static void ApplyMove9001(Stack<char>[] stacks, Move move)
        {
            var tempStack = new Stack<char>();
            for (int i = 0; i < move.Amount; i++)
            {
                char crate = stacks[move.From].Pop();
                tempStack.Push(crate);
            }
            while (tempStack.Count > 0)
            {
                char crate = tempStack.Pop();
                stacks[move.To].Push(crate);
            }
        }
    }

    internal record Move
    {
        public int Amount { get; init; }
        public int From { get; init; }
        public int To { get; init; }

        public Move(string definition)
        {
            var steps = definition.Split(" ");
            Amount = int.Parse(steps[1]);
            From = int.Parse(steps[3])-1;
            To = int.Parse(steps[5])-1;
        }
    }
}