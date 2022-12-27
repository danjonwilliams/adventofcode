using System;

namespace AdventOfCode.Y2022.D06
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //var input = "nppdvjthqldpwncqszvftbrmjlhg";
            var input = File.ReadAllText("input.txt");

            const int startOfSignalLength = 4;
            var startOfSignalOffset = GetMarkerPosition(input, startOfSignalLength);
            Console.WriteLine($"Part One: End of first signal marker: {startOfSignalOffset}");

            const int startOfMessageLength = 14;
            var startOfMessageOffset = GetMarkerPosition(input, startOfMessageLength);
            Console.WriteLine($"Part Two: End of first message marker: {startOfMessageOffset}");
        }

        static int GetMarkerPosition(string dataStream, int markerLength)
        {
            var buffer = new LinkedList<char>();
            for (int readPosition = 0; readPosition < dataStream.Length; readPosition++)
            {
                buffer.AddFirst(dataStream[readPosition]);
                if (buffer.Count() > markerLength)
                {
                    buffer.RemoveLast();
                }
                var set = new HashSet<char>(buffer);
                if (set.Count == markerLength)
                {
                    return readPosition + 1;
                }
            }
            return -1;
        }
    }
}