using System.Text;

namespace AdventOfCode.Y2022.D13
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");

            var signalPairs = GetSignalPairs(input).ToArray();

            var orderedCount = 0;
            for (int i = 0; i < signalPairs.Length; i++)
            {
                var pair = signalPairs[i];
                if (pair.Item1.Compare(pair.Item2) == Relationship.LessThan)
                {
                    orderedCount += i + 1;
                }
            }
            Console.WriteLine($"Part One: Number of correctly ordered pairs: {orderedCount}");

            var dividerPackets = new[]
            {
                GetListSignalItem("[[2]]"),
                GetListSignalItem("[[6]]")
            };
            var allSignals = signalPairs
                .SelectMany(sp => new[] { sp.Item1, sp.Item2 })
                .Concat(dividerPackets)
                .Order()
                .ToArray();

            var divider1Offset = 0;
            var divider2Offset = 0;
            // Would binary search be faster? ReferenceEquals from start may be quicker that complex comparison checks
            for (int i = 0; i < allSignals.Length; i++)
            {
                if (ReferenceEquals(allSignals[i], dividerPackets[0]))
                {
                    divider1Offset = i + 1;
                    continue;
                }
                if (ReferenceEquals(allSignals[i], dividerPackets[1]))
                {
                    divider2Offset = i + 1;
                    break;
                }
            }
            Console.WriteLine($"Part Two: Signal divider factor: {divider1Offset * divider2Offset}");
        }

        private static IEnumerable<Tuple<SignalItem, SignalItem>> GetSignalPairs(string[] input)
        {
            var line = 0;
            while (line < input.Length)
            {
                var input1 = GetListSignalItem(input[line]);
                line++;
                var input2 = GetListSignalItem(input[line]);
                line += 2;
                yield return new Tuple<SignalItem, SignalItem>(input1, input2);
            }
        }

        private static SignalItem GetListSignalItem(string itemDefinition)
        {
            itemDefinition = itemDefinition.Substring(1, itemDefinition.Length - 2);
            var listStack = new Stack<ListSignalItem>();
            ListSignalItem currentList = new ListSignalItem();
            StringBuilder buffer = new StringBuilder();
            for (int i = 0; i < itemDefinition.Length; i++)
            {
                if (itemDefinition[i] == '[')
                {
                    listStack.Push(currentList);
                    currentList = new ListSignalItem();
                }
                else if (itemDefinition[i] == ']')
                {
                    if (buffer.Length > 0)
                    {
                        currentList.AddListItem(new NumberSignalItem(int.Parse(buffer.ToString())));
                        buffer.Clear();
                    }

                    var outer = listStack.Pop();
                    outer.AddListItem(currentList!);
                    currentList = outer;
                }
                else if (itemDefinition[i] == ',')
                {
                    if (buffer.Length > 0)
                    {
                        currentList.AddListItem(new NumberSignalItem(int.Parse(buffer.ToString())));
                        buffer.Clear();
                    }
                }
                else
                {
                    buffer.Append(itemDefinition[i]);
                }
            }
            if (buffer.Length > 0)
            {
                currentList.AddListItem(new NumberSignalItem(int.Parse(buffer.ToString())));
                buffer.Clear();
            }
            return currentList;
        }
    }

    internal abstract class SignalItem : IComparable<SignalItem>
    {
        public abstract Relationship Compare(SignalItem other);
        public abstract Relationship Compare(NumberSignalItem other);
        public abstract Relationship Compare(ListSignalItem other);

        public int CompareTo(SignalItem? other)
        {
            if (other == null)
            {
                return 1;
            }

            var relationship = Compare(other);
            return relationship switch
            {
                Relationship.LessThan => -1,
                Relationship.GreaterThan => 1,
                _ => 0
            };
        }

        protected Relationship ReverseRelationship(Relationship relationship)
        {
            return relationship switch
            {
                Relationship.LessThan => Relationship.GreaterThan,
                Relationship.GreaterThan => Relationship.LessThan,
                _ => relationship
            };
        }
    }

    internal class ListSignalItem : SignalItem
    {
        private List<SignalItem> valueList;

        public ListSignalItem(SignalItem value)
        {
            valueList = new List<SignalItem>{ value };
        }

        public ListSignalItem(IEnumerable<SignalItem> values)
        {
            valueList = values.ToList();
        }

        public ListSignalItem()
        {
            valueList = new List<SignalItem>();
        }

        public void AddListItem(SignalItem item)
        {
            valueList.Add(item);
        }

        public override Relationship Compare(NumberSignalItem other)
        {
            var otherList = new ListSignalItem(other);
            return Compare(otherList);
        }

        public override Relationship Compare(ListSignalItem other)
        {
            for (int i = 0; i < valueList.Count; i++)
            {
                if (i >= other.valueList.Count)
                {
                    return Relationship.GreaterThan;
                }
                var relationship = valueList[i].Compare(other.valueList[i]);
                if (relationship != Relationship.Equal)
                {
                    return relationship;
                }
            }
            return valueList.Count == other.valueList.Count
                ? Relationship.Equal
                : Relationship.LessThan;
        }

        public override Relationship Compare(SignalItem other)
        {
            return ReverseRelationship(other.Compare(this));
        }
    }

    internal class NumberSignalItem : SignalItem
    {
        private readonly int value;

        public NumberSignalItem(int value)
        {
            this.value = value;
        }

        public override Relationship Compare(NumberSignalItem other)
        {
            if (value > other.value)
            {
                return Relationship.GreaterThan;
            }
            if (value < other.value)
            {
                return Relationship.LessThan;
            }
            return Relationship.Equal;
        }

        public override Relationship Compare(ListSignalItem other)
        {
            var valueList = new ListSignalItem(this);
            return valueList.Compare(other);
        }

        public int Value
        {
            get
            {
                return value;
            }
        }

        public override Relationship Compare(SignalItem other)
        {
            return ReverseRelationship(other.Compare(this));
        }
    }

    internal enum Relationship
    {
        LessThan,
        Equal,
        GreaterThan
    }
}