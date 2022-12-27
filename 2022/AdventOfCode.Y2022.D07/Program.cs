using System.Net.Http.Headers;
using System.Reflection.Metadata.Ecma335;

namespace AdventOfCode.Y2022.D07
{
    internal class Program
    {
        private static Directory? _root = null;

        static void Main(string[] args)
        {
            var input = File.ReadAllLines("input.txt");

            Directory? current = null;
            foreach (var line in input)
            {
                if (line[0] == '$')
                {
                    current = ProcessCommandLine(line, current);
                }
                else
                {
                    ProcessResultsLine(line, current);
                }
            }
            var totalFileSize = _root!.GetTotalFileSize();
            Console.WriteLine($"Part One: Total Filesystem: {totalFileSize}");

            var directorySizes = _root!.GetDirectorySizes().ToArray();
            var smallDiretorySizes = directorySizes.Where(ds => ds.Item2 <= 100000).ToArray();
            foreach (var ds in smallDiretorySizes)
            {
                Console.WriteLine($"Name: {ds.Item1} : Size: {ds.Item2}");
            }
            var directorySizeTotal = smallDiretorySizes.Sum(ds => ds.Item2);
            Console.WriteLine($"Part One: Total Chosen Directories: {directorySizeTotal}");

            long freeSpace = 70000000 - totalFileSize;
            const long requiredFreeSpace = 30000000;
            long requiredSpace = requiredFreeSpace - freeSpace;
            var orderedDirectorySises = directorySizes.Select(ds => ds.Item2).Order().ToArray();
            var deleteCandidate = orderedDirectorySises.First(x => x > requiredSpace);
            Console.WriteLine($"Part Two: Best Directory Size: {deleteCandidate}");
        }

        private static Directory? ProcessCommandLine(string line, Directory? current)
        {
            var symbols = line.Split(' ');
            var command = symbols[1];
            switch (command)
            {
                case "ls":
                    return current;
                case "cd":
                    var target = symbols[2];
                    if (target.Equals("/"))
                    {
                        return _root = _root ?? new Directory("/", null);
                    }
                    else if (target.Equals(".."))
                    {
                        return current?.Parent;
                    }
                    else
                    {
                        return current?.GetDirectory(target);
                    }
            }

            throw new InvalidOperationException("Unable to parse command line");
        }

        private static void ProcessResultsLine(string line, Directory? current)
        {
            var symbols = line.Split(' ');
            if (symbols[0].Equals("dir"))
            {
                var directoryName = symbols[1];
                current?.AddDirectory(new Directory(directoryName, current));
            }
            else
            {
                var filename = symbols[1];
                var size = int.Parse(symbols[0]);
                current?.AddFile(new FileDefinition(filename, size));
            }
        }
    }

    internal class Directory
    {
        private readonly List<FileDefinition> _files = new List<FileDefinition>();
        private readonly List<Directory> _subdirectories = new List<Directory>();

        public string Name { get; }
        public Directory? Parent { get; }

        public Directory(string name, Directory? parent)
        {
            Name = name;
            Parent = parent;
        }
        public void AddFile(FileDefinition file)
        {
            _files.Add(file);
        }

        public void AddDirectory(Directory directory)
        {
            _subdirectories.Add(directory);
        }

        public Directory GetDirectory(string name)
        {
            return _subdirectories.Single(sd => sd.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public long GetTotalFileSize()
        {
            long size = _files.Sum(f => f.Size);
            size += _subdirectories.Sum(sd => sd.GetTotalFileSize());
            return size;
        }

        public IEnumerable<Tuple<string,long>> GetDirectorySizes()
        {
            yield return new Tuple<string, long> (Name, GetTotalFileSize());
            foreach (var subdirectory in _subdirectories)
            {
                foreach (var size in subdirectory.GetDirectorySizes())
                {
                    yield return size;
                }
            }
        }
    }

    internal record FileDefinition
    {
        public int Size { get; init; }
        public string Name { get; init; }

        public FileDefinition(string name, int size)
        {
            Name = name;
            Size = size;
        }
    }
}