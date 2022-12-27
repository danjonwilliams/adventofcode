using System.Security.Cryptography.X509Certificates;

namespace AdventOfCode.Y2022.D03
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //var input = new[]
            //{
            //    "vJrwpWtwJgWrhcsFMMfFFhFp",
            //    "jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL",
            //    "PmmdzqPrVvPwwTWBwg",
            //    "wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn",
            //    "ttgJtRGJQctTZtZT",
            //    "CrZsJsPPZsGzwwsLwLmpwMDw",
            //};
            //var input = new[]
            //{
            //    "vJrwpWtwJgWrhcsFMMfFFhFp",
            //    "jqHRNqRjqzjGDLGLrsFMfFZSrLrFZsSL",
            //    "PmmdzqPrVvPwwTWBwg",
            //    "wMqvLMZHhHMvwLHjbvcjnnSBnvTQFn",
            //    "ttgJtRGJQctTZtZT",
            //    "CrZsJsPPZsGzwwsLwLmpwMDw"
            //};
            var input = File.ReadAllLines("input.txt");

            var rucksacks = input.Select(i => new Rucksack(i)).ToArray();
            var duplicateItems = rucksacks.Select(r =>r.GetDuplicateItem());
            var priorities = duplicateItems.Select(GetPriority);
            Console.WriteLine($"Part One: Rucksack Priorities Total = {priorities.Sum()}");

            var elfTeams = new List<Team>(rucksacks.Length / 3);
            for (int i = 0; i < rucksacks.Length; i+=3)
            {
                elfTeams.Add(new Team(new[] { rucksacks[i], rucksacks[i+1], rucksacks[i+2] }));
            }
            Console.WriteLine($"Part Two: Badge Priorities Total = {elfTeams.Select(t => GetPriority(t.GetCommonItem())).Sum()}");
        }

        private static int GetPriority(char item)
        {
            if (item >= 'a' && item <= 'z')
            {
                return item - 'a' + 1;
            }
            if (item >= 'A' && item <= 'Z')
            {
                return item - 'A' + 27;
            }
            throw new ArgumentOutOfRangeException(nameof(item));
        }
    }

    internal class Team
    {
        private char _common;
        public Team(IEnumerable<Rucksack> rucksacks)
        {
            var firstElf = rucksacks.First();
            var otherElves = rucksacks.Skip(1).ToArray();

            foreach (var item in firstElf.GetContents())
            {
                if (otherElves.All(r => r.ContainsItem(item)))
                {
                    _common = item;
                    return;
                }
            }
            throw new InvalidOperationException("Elf team doesn't have common badge item");
        }

        public char GetCommonItem()
        {
            return _common;
        }
    }

    internal class Rucksack
    {
        private char _duplicate;
        private HashSet<char> _contents;

        public Rucksack(string contents)
        {
            if (contents.Length % 2 != 0)
            {
                throw new ArgumentException($"Invalid contents length {contents.Length}");
            }
            
            _contents = new HashSet<char>(contents);
            
            var compartment1 = contents.Substring(0, contents.Length / 2);
            var compartment2 = new HashSet<char>(contents.Substring(contents.Length / 2));
            for (int i = 0; i < compartment1.Length; i++)
            {
                if (compartment2.Contains(compartment1[i]))
                {
                    _duplicate = compartment1[i];
                    return;
                }
            }
            throw new InvalidOperationException($"{contents} does not contain a duplicate");
        }

        public char GetDuplicateItem()
        {
            return _duplicate;
        }

        public IEnumerable<char> GetContents()
        {
            return _contents;
        }

        public bool ContainsItem(char item)
        {
            return _contents.Contains(item);
        }
    }
}