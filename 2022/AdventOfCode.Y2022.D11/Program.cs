/* Hacky answer requires changes for each part */

namespace AdventOfCode.Y2022.D11
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Monkey[] allMonkeys = null!; // This must be set before acting
            void ThrowItem(int monkey, Item item)
            {
                allMonkeys![monkey].AddItem(item);
            }

            var input = File.ReadAllLines("input.txt");

            var monkeys = new List<Monkey>();
            List<List<Item>> monkeyItems = new List<List<Item>>();
            int inputCount = 0;
            int worryFactor = 1;
            while (inputCount <= input.Length)
            {
                var itemList = new List<Item>();
                monkeyItems.Add(itemList);
                int monkeyNumber = GetMonkeyNumber(input[inputCount++]);
                itemList.AddRange(GetStartingItems(input[inputCount++]));
                var interestFactor = GetInterestFactor(input[inputCount++]);
                var testValue = GetTestValue(input[inputCount++]);
                worryFactor *= testValue;
                var trueMonkey = GetTargetMonkey(input[inputCount++]);
                var falseMonkey = GetTargetMonkey(input[inputCount++]);
                //skip blank
                inputCount++;
                var monkey = new Monkey(interestFactor, testValue, trueMonkey, falseMonkey, ThrowItem);
                foreach (var item in itemList)
                {
                    monkey.AddItem(item);
                }
                monkeys.Add(monkey);
            }
            allMonkeys = monkeys.ToArray(); // Done it
            foreach (var item in monkeyItems.SelectMany(x => x))
            {
                item.SetWorryFactor(worryFactor);
            }
            // Part One
            //const int maxRounds = 20;
            // Part Two
            const int maxRounds = 10000;
            for (int round = 0; round < maxRounds; round++)
            {
                foreach (var monkey in allMonkeys)
                {
                    monkey.InspectItems();
                }
            }

            var topMonkeys = allMonkeys.OrderByDescending(m => m.InspectionCount).Take(2).ToArray();
            Console.WriteLine(value: $"Part One: Top Monkeys Factor: {(long)topMonkeys[0].InspectionCount * (long)topMonkeys[1].InspectionCount}");
        }

        private static int GetTargetMonkey(string line)
        {
            return int.Parse(line.Trim().Split(" ")[5]);
        }

        private static int GetTestValue(string line)
        {
            return int.Parse(line.Trim().Split(" ")[3]);
        }

        private static Func<long, long> GetInterestFactor(string line)
        {
            var isMultiply = line.Contains("*");
            var factor = line.Trim().Split(" ")[5];
            var isOld = factor.Equals("old");

            if (isOld)
            {
                if (isMultiply)
                {
                    return x => x * x;
                }
                return x => x + x;
            }
            var factorValue = int.Parse(factor);
            if (isMultiply)
            {
                return x => x * factorValue;
            }
            return x => x + factorValue;
        }

        private static Item[] GetStartingItems(string line)
        {
            var items = line.Trim().Split(":")[1];
            return items.Split(",").Select(i => new Item(int.Parse(i))).ToArray();
        }

        private static int GetMonkeyNumber(string line)
        {
            return int.Parse(line.Trim().Split(" ")[1][..^1]);
        }
    }

    internal class Item
    {
        private int _worryFactor;

        public Item(long worryLevel)
        {
            WorryLevel = worryLevel;
        }
        public long WorryLevel { get; set; }

        public void Relax()
        {
            // Part One
            // WorryLevel /= 3;
            // Part Two
            WorryLevel %= _worryFactor;
        }

        internal void SetWorryFactor(int worryFactor)
        {
            _worryFactor = worryFactor;
        }
    }

    internal class Monkey
    {
        private readonly List<Item> _items = new List<Item>();
        private readonly Func<long, long> _interestFactor;
        private readonly int _testMod;
        private readonly int _trueMonkey;
        private readonly int _falseMonkey;
        private readonly Action<int, Item> _throwItem;
        
        public int InspectionCount { get; private set; }

        public Monkey(Func<long, long> interestFactor, int testMod, int trueMonkey, int falseMonkey, Action<int, Item> throwItem)
        {
            _interestFactor = interestFactor;
            _testMod = testMod;
            _trueMonkey = trueMonkey;
            _falseMonkey = falseMonkey;
            _throwItem = throwItem;
        }

        public void AddItem(Item item)
        {
            _items.Add(item);
        }

        public void InspectItems()
        {
            foreach (Item item in _items)
            {
                InspectionCount++;
                item.WorryLevel = _interestFactor(item.WorryLevel);
                item.Relax();
                TestItem(item);
            }
            _items.Clear();
        }

        private void TestItem(Item item)
        {
            var targetMonkey = item.WorryLevel % _testMod == 0
                ? _trueMonkey
                : _falseMonkey;
            _throwItem(targetMonkey,item);
        }
    }
}