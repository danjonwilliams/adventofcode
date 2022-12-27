namespace AdventOfCode.Y2022.D04
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //var input = new[]
            //{
            //    "2-4,6-8",
            //    "2-3,4-5",
            //    "5-7,7-9",
            //    "2-8,3-7",
            //    "6-6,4-6",
            //    "2-6,4-8"
            //};
            var input = File.ReadAllLines("input.txt");
            var assigments = input.Select(GetAssignments).ToList();

            var covered = assigments.Where(a => a.Item1.Covers(a.Item2) || a.Item2.Covers(a.Item1)).ToArray();
            Console.WriteLine($"Part One: Covered Assignments Count = {covered.Length}");

            var overlapped = assigments.Where(a => a.Item1.Overlaps(a.Item2)).ToArray();
            Console.WriteLine($"Part Two: Overlapped Assignments Count = {overlapped.Length}");
        }

        private static Tuple<CleaningAssignment, CleaningAssignment> GetAssignments(string input)
        {
            var assignments = input.Split(',');
            return new Tuple<CleaningAssignment, CleaningAssignment>(GetAssignment(assignments[0]), GetAssignment(assignments[1]));
        }

        private static CleaningAssignment GetAssignment(string assignmentString)
        {
            var assignments = assignmentString.Split("-");
            var start = int.Parse(assignments[0]);
            var end = int.Parse(assignments[1]);
            return new CleaningAssignment(start, end);
        }
    }

    internal class CleaningAssignment
    {
        private readonly int _start;
        private readonly int _end;

        public CleaningAssignment(int start, int end)
        {
            if (end < start)
            {
                throw new ArgumentException("End cannot be less than start");
            }
            _start = start;
            _end = end;
        }

        public bool Covers(CleaningAssignment otherAssignment)
        {
            return otherAssignment._start >= this._start
                && otherAssignment._start <= this._end
                && otherAssignment._end >= this._start
                && otherAssignment._end <= this._end;
        }

        public bool Overlaps(CleaningAssignment otherAssignment)
        {
            return (otherAssignment._start >= this._start && otherAssignment._start <= this._end)
                || (this._start >= otherAssignment._start && this._start <= otherAssignment._end);
        }
    }
}